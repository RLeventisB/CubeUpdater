using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using Octokit;

namespace CubeUpdater
{
    public class Program
    {
        public static IProgress<long> downloadProgress = new Progress<long>(delegate (long bytes)
        {
            lock (progressLock)
            {
                Console.CursorTop = currentDownload.top;
                Write($"Descargando {currentDownload.fileName} ({FormatMemory(bytes)} / {FormatMemory(currentDownload.size)})    ");
            }
        });
        public static object progressLock = new object();
        public static DownloadData currentDownload = default;
        public static string[] MemPrefixes = { "B", "KB", "MB", "GB", "TB" };
        public static GitHubClient client;
        public static CubeRelease[] cubeReleases;
        public static DateTime lastReload;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(object text)
        {
            Console.WriteLine(text);
        }
        public static string FormatMemory(long bytes)
        {
            int prefix = 0;
            while (bytes > 1024)
            {
                bytes >>= 10;
                prefix++;
            }
            return bytes + " " + MemPrefixes[prefix];
        }
        public static void Main()
        {
            Write("Hola bienvenido a cubitos descargador!!!!\nCargando...");
            try
            {
                client = new GitHubClient(new Octokit.ProductHeaderValue("CubeUpdater"))
                {
                    Credentials = new Credentials(Token.secret)
                };
            }
            catch (Exception e)
            {
                Write($"Error al crear sesion del usuario:\n {e}\nMandar a bloque de notasp rofavor :))");
                Console.ReadKey(false);
                return;
            }
            Write("Sesion iniciada!!! Obteniendo versiones :)");
            GetAllReleases();
            Directory.CreateDirectory("Descargas");
            int releaseCount = cubeReleases.Length;
            Write($"Carga completa!!!! Encontrado {releaseCount} versiones");
            bool draw = true, showingWeekly = false, selectedVersion = false;
            int offset = 0, selectedIndex = 0;
            ReadOnlySpan<CubeRelease> sortedReleases = cubeReleases.Where(v => !v.isWeekly).ToArray();
            releaseCount = sortedReleases.Length;
            Thread.Sleep(1000);
            while (draw)
            {
                Console.Clear();
                if (selectedVersion)
                {
                    CubeRelease release = sortedReleases[selectedIndex];
                    Write($"Nombre de la version: {release.release.Name}\n{release.release.Body}\nSubida el: {release.release.CreatedAt.ToLocalTime()}");
                    Write("D: Descargar version");
                    Write("S: Volver al selector de menus");
                    var key = Console.ReadKey(false).Key;
                    switch (key)
                    {
                        case ConsoleKey.D:
                            Download(ref release, false);
                            break;
                        case ConsoleKey.S:
                            selectedVersion = false;
                            break;
                    }
                }
                else
                {
                    Write($"Porfavor eliga la version.\nPagina {offset} de {releaseCount}");
                    for (int i = 0; i + offset < releaseCount && i < 10; i++)
                    {
                        CubeRelease release = sortedReleases[i + offset];
                        Write($"{i}: {release.release.Name} {(release.isWeekly ? "(Semanal)" : "")}");
                    }
                    Write("Flechita izquierda: Ir a la anterior pagina");
                    Write("Flechita derecha: Ir a la siguiente pagina");
                    Write($"W: {(showingWeekly ? "Desactivar" : "Activar")} versiones semanales");
                    Write("R: Recargar las versiones actuales (especificamente no spammear esto)");
                    Write("S: Salir");
                    bool notValidKey = true;
                    while (notValidKey)
                    {
                        var key = Console.ReadKey(false).Key;
                        switch (key)
                        {
                            case ConsoleKey.NumPad0:
                            case ConsoleKey.NumPad1:
                            case ConsoleKey.NumPad2:
                            case ConsoleKey.NumPad3:
                            case ConsoleKey.NumPad4:
                            case ConsoleKey.NumPad5:
                            case ConsoleKey.NumPad6:
                            case ConsoleKey.NumPad7:
                            case ConsoleKey.NumPad8:
                            case ConsoleKey.NumPad9:
                                selectedIndex = (int)key - 96 + offset;
                                if (selectedIndex < releaseCount)
                                {
                                    selectedVersion = true;
                                    notValidKey = false;
                                }
                                break;
                            case ConsoleKey.D0:
                            case ConsoleKey.D1:
                            case ConsoleKey.D2:
                            case ConsoleKey.D3:
                            case ConsoleKey.D4:
                            case ConsoleKey.D5:
                            case ConsoleKey.D6:
                            case ConsoleKey.D7:
                            case ConsoleKey.D8:
                            case ConsoleKey.D9:
                                selectedIndex = (int)key - 48 + offset;
                                if (selectedIndex < releaseCount)
                                {
                                    selectedVersion = true;
                                    notValidKey = false;
                                }
                                break;
                            case ConsoleKey.LeftArrow:
                                if (offset > 9)
                                {
                                    offset -= 10;
                                    notValidKey = false;
                                }
                                break;
                            case ConsoleKey.RightArrow:
                                if (offset + 10 < releaseCount)
                                {
                                    offset += 10;
                                    notValidKey = false;
                                }
                                break;
                            case ConsoleKey.W:
                                showingWeekly = !showingWeekly;
                                if (showingWeekly)
                                {
                                    sortedReleases = cubeReleases.ToArray();
                                    releaseCount = sortedReleases.Length;
                                }
                                else
                                {
                                    sortedReleases = cubeReleases.Where(v => !v.isWeekly).ToArray();
                                    releaseCount = sortedReleases.Length;
                                    if (offset >= releaseCount)
                                    {
                                        offset = (releaseCount / 10 - 1) * 10;
                                    }
                                }
                                notValidKey = false;
                                break;
                            case ConsoleKey.R:
                                int secondsLeft = (int)DateTime.UtcNow.Subtract(lastReload).TotalSeconds;
                                if (secondsLeft > 60)
                                {
                                    GetAllReleases();
                                    notValidKey = false;
                                }
                                else
                                {
                                    Write($"Debes de esperar {secondsLeft} segundos mas para recargar");
                                }
                                break;
                            case ConsoleKey.S:
                                draw = false;
                                notValidKey = false;
                                break;
                        }
                    }
                }
            }
            Write("Adiositos!!!\nPresione una tecla para continuar.");
            Console.ReadKey(false);
        }
        public static void GetAllReleases()
        {
            try
            {
                IReadOnlyList<Release> releases = client.Repository.Release.GetAll("RLeventisB", "cubito").Result;
                cubeReleases = new CubeRelease[releases.Count];
                for (int i = 0; i < releases.Count; i++)
                {
                    IReadOnlyList<ReleaseAsset> assets = client.Repository.Release.GetAllAssets("RLeventisB", "cubito", releases[i].Id).Result;
                    cubeReleases[i] = new CubeRelease(releases[i], assets);
                }
            }
            catch (Exception e)
            {
                Write($"Error al obtener las versiones de cubito:\n {e}\nMandar a bloque de notasp rofavor :))");
                Console.ReadKey(false);
            }
            lastReload = DateTime.UtcNow;
        }
        public static void Download(ref CubeRelease release, bool sourceCode)
        {
            Span<ReleaseAsset> assets;
            if (sourceCode)
            {
                assets = release.assets.Skip(release.assets.Length - 2).ToArray();
            }
            else
            {
                assets = release.assets;
            }
            Console.Clear();
            string saveDir = AppDomain.CurrentDomain.BaseDirectory + "Descargas\\" + (release.isWeekly ? "Weekly\\" : "") + release.release.TagName + "\\";
            StringBuilder files = new StringBuilder();
            for (int i = 0; i < assets.Length; i++)
            {
                files.Append(assets[i].Name);
                files.Append(" (");
                files.Append(FormatMemory(assets[i].Size));
                files.Append(')');
                if (i < assets.Length - 1)
                    files.Append(", ");
            }
            Write($"Descargar {assets.Length} archivos? ({files})\nSe guardara en {saveDir}\nS: Descargar, N: Cancelar");
            bool notValidKey = true;
            while (notValidKey)
            {
                switch (Console.ReadKey(false).Key)
                {
                    case ConsoleKey.S:
                        Directory.CreateDirectory(saveDir);
                        foreach (var asset in assets)
                        {
                            currentDownload = new DownloadData(asset.Name, asset.Size, Console.CursorTop);
                            using (var httpClient = new HttpClient())
                            {
                                httpClient.Timeout = TimeSpan.FromMinutes(5);
                                httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla", "5.0"));
                                httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("AppleWebKit", "537.36"));
                                httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Chrome", "70.0.3538.77"));
                                httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Safari", "537.36"));
                                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", Token.secret);
                                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/octet-stream"));
                                using (FileStream stream = File.Create(saveDir + asset.Name))
                                {
                                    DownloadAsync(httpClient, $"https://api.github.com/repos/RLeventisB/cubito/releases/assets/{asset.Id}", stream).ConfigureAwait(false).GetAwaiter().GetResult();
                                }
                            }
                            while (!currentDownload.done)
                            {

                            }
                            Write($"Descargado {asset.Name}");

                        }
                        notValidKey = false;
                        break;
                    case ConsoleKey.N:
                        notValidKey = false;
                        break;
                }
            }
        }
        public static async Task DownloadAsync(HttpClient client, string requestUri, Stream destination)
        {
            using (var response = await client.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead))
            {
                using (var download = await response.Content.ReadAsStreamAsync())
                {
                    var buffer = new byte[81920];
                    long totalBytesRead = 0;
                    int bytesRead;
                    while ((bytesRead = await download.ReadAsync(buffer).ConfigureAwait(false)) != 0)
                    {
                        await destination.WriteAsync(buffer, 0, bytesRead).ConfigureAwait(false);
                        totalBytesRead += bytesRead;
                        downloadProgress.Report(totalBytesRead);
                    }
                }
            }
            currentDownload.done = true;
        }
    }
    public struct DownloadData
    {
        public bool done = false;
        public readonly string fileName;
        public readonly int size, top;
        public DownloadData(string fileName, int size, int top)
        {
            this.fileName = fileName;
            this.size = size;
            this.top = top;
        }
    }
    public readonly struct CubeRelease
    {
        public readonly bool isWeekly;
        public readonly Release release;
        public readonly ReleaseAsset[] assets;
        public CubeRelease(Release release, IReadOnlyList<ReleaseAsset> assets)
        {
            this.release = release;
            isWeekly = release.Name.StartsWith("Autobuild ");
            this.assets = assets.ToArray();
        }
    }
}