using System.Net;
using System.Runtime.CompilerServices;
using Octokit;

namespace CubeUpdater
{
    public class Program
    {
        public static GitHubClient client;
        public static CubeRelease[] cubeReleases;
        public static DateTime lastReload;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(object text)
        {
            Console.WriteLine(text);
        }
        public static void Main()
        {
            Write("Hola bienvenido a cubitos descargador!!!!\nCargando...");
            try
            {
                client = new GitHubClient(new ProductHeaderValue("CubeUpdater"))
                {
                    Credentials = new Credentials("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")
                };
            }
            catch (Exception e)
            {
                Write($"Error al crear sesion del usuario:\n {e}\nMandar a bloque de notasp rofavor :))");
                Console.ReadKey();
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
                    Release release = sortedReleases[selectedIndex].release;
                    Write($"Nombre de la version: {release.Name}\n{release.Body}\nSubida el: {release.CreatedAt.ToLocalTime()}");
                    Write("D: Descargar version");
                    Write("C: Descargar codigo fuente de la version (para que necesitas esto)");
                    Write("S: Volver al selector de menus");
                    var key = Console.ReadKey().Key;
                    switch (key)
                    {
                        case ConsoleKey.D:
                            break;
                        case ConsoleKey.C:
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
            Console.ReadKey();
        }
        public static void GetAllReleases()
        {
            try
            {
                IReadOnlyList<Release> releases = client.Repository.Release.GetAll("RLeventisB", "cubito").Result;
                cubeReleases = new CubeRelease[releases.Count];
                for (int i = 0; i < releases.Count; i++)
                {
                    cubeReleases[i] = new CubeRelease(releases[i]);
                }
            }
            catch (Exception e)
            {
                Write($"Error al obtener las versiones de cubito:\n {e}\nMandar a bloque de notasp rofavor :))");
                Console.ReadKey();
            }
            lastReload = DateTime.UtcNow;
        }
        public static void Download(ref Release release, bool sourceCode)
        {
        }
    }
    public readonly struct CubeRelease
    {
        public readonly bool isWeekly;
        public readonly Release release;
        public CubeRelease(Release release)
        {
            this.release = release;
            isWeekly = release.Name.StartsWith("Autobuild ");
        }
    }
}