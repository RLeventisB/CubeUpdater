using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace Octokit
{
    /// <summary>
    /// A client for GitHub's Repositories API.
    /// </summary>
    /// <remarks>
    /// See the <a href="https://docs.github.com/rest/repos/repos">Repositories API documentation</a> for more details.
    /// </remarks>
    public class RepositoriesClient : ApiClient, IRepositoriesClient
    {
        /// <summary>
        /// Initializes a new GitHub Repos API client.
        /// </summary>
        /// <param name="apiConnection">An API connection</param>
        public RepositoriesClient(IApiConnection apiConnection) : base(apiConnection)
        {
            Release = new ReleasesClient(apiConnection);
        }
        public IReleasesClient Release { get; private set; }
    }
}
