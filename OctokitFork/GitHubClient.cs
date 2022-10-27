using Octokit.Internal;

namespace Octokit
{
    /// <summary>
    /// A Client for the GitHub API v3. You can read more about the api here: http://developer.github.com.
    /// </summary>
    public class GitHubClient : IGitHubClient
    {
        /// <summary>
        /// The base address for the GitHub API
        /// </summary>
        public static readonly Uri GitHubApiUrl = new Uri("https://api.github.com/");
        internal static readonly Uri GitHubDotComUrl = new Uri("https://github.com/");

        /// <summary>
        /// Create a new instance of the GitHub API v3 client pointing to
        /// https://api.github.com/
        /// </summary>
        /// <remarks>
        /// See more information regarding User-Agent requirements here: https://developer.github.com/v3/#user-agent-required
        /// </remarks>
        /// <param name="productInformation">
        /// The name (and optionally version) of the product using this library, the name of your GitHub organization, or your GitHub username (in that order of preference). This is sent to the server as part of
        /// the user agent for analytics purposes, and used by GitHub to contact you if there are problems.
        /// </param>
        public GitHubClient(ProductHeaderValue productInformation)
            : this(new Connection(productInformation, GitHubApiUrl))
        {
        }

        /// <summary>
        /// Create a new instance of the GitHub API v3 client pointing to the specified baseAddress.
        /// </summary>
        /// <remarks>
        /// See more information regarding User-Agent requirements here: https://developer.github.com/v3/#user-agent-required
        /// </remarks>
        /// <param name="productInformation">
        /// The name (and optionally version) of the product using this library, the name of your GitHub organization, or your GitHub username (in that order of preference). This is sent to the server as part of
        /// the user agent for analytics purposes, and used by GitHub to contact you if there are problems.
        /// </param>
        /// <param name="credentialStore">Provides credentials to the client when making requests</param>
        /// <param name="baseAddress">
        /// The address to point this client to. Typically used for GitHub Enterprise
        /// instances</param>
        public GitHubClient(ProductHeaderValue productInformation, ICredentialStore credentialStore, Uri baseAddress)
            : this(new Connection(productInformation, FixUpBaseUri(baseAddress), credentialStore))
        {
        }

        /// <summary>
        /// Create a new instance of the GitHub API v3 client using the specified connection.
        /// </summary>
        /// <param name="connection">The underlying <seealso cref="IConnection"/> used to make requests</param>
        public GitHubClient(IConnection connection)
        {
            Ensure.ArgumentNotNull(connection, nameof(connection));

            Connection = connection;
            var apiConnection = new ApiConnection(connection);
            Repository = new RepositoriesClient(apiConnection);
        }

        /// <summary>
        /// Convenience property for getting and setting credentials.
        /// </summary>
        /// <remarks>
        /// You can use this property if you only have a single hard-coded credential. Otherwise, pass in an
        /// <see cref="ICredentialStore"/> to the constructor.
        /// Setting this property will change the <see cref="ICredentialStore"/> to use
        /// the default <see cref="InMemoryCredentialStore"/> with just these credentials.
        /// </remarks>
        public Credentials Credentials
        {
            get { return Connection.Credentials; }
            // Note this is for convenience. We probably shouldn't allow this to be mutable.
            set
            {
                Ensure.ArgumentNotNull(value, nameof(value));
                Connection.Credentials = value;
            }
        }

        /// <summary>
        /// The base address of the GitHub API. This defaults to https://api.github.com,
        /// but you can change it if needed (to talk to a GitHub:Enterprise server for instance).
        /// </summary>
        public Uri BaseAddress
        {
            get { return Connection.BaseAddress; }
        }

        /// <summary>
        /// Provides a client connection to make rest requests to HTTP endpoints.
        /// </summary>
        public IConnection Connection { get; private set; }

        /// <summary>
        /// Access GitHub's Repositories API.
        /// </summary>
        /// <remarks>
        /// Refer to the API documentation for more information: https://developer.github.com/v3/repos/
        /// </remarks>
        public IRepositoriesClient Repository { get; private set; }

        static Uri FixUpBaseUri(Uri uri)
        {
            Ensure.ArgumentNotNull(uri, nameof(uri));

            if (uri.Host.Equals("github.com") || uri.Host.Equals("api.github.com"))
            {
                return GitHubApiUrl;
            }

            return new Uri(uri, new Uri("/api/v3/", UriKind.Relative));
        }
    }
}
