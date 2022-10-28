namespace Octokit
{
    /// <summary>
    /// A client for GitHub's Releases API.
    /// </summary>
    /// <remarks>
    /// See the <a href="http://developer.github.com/v3/repos/releases/">Releases API documentation</a> for more information.
    /// </remarks>
    public class ReleasesClient : ApiClient, IReleasesClient
    {
        /// <summary>
        /// Initializes a new GitHub Releases API client.
        /// </summary>
        /// <param name="apiConnection">An API connection</param>
        public ReleasesClient(IApiConnection apiConnection) : base(apiConnection)
        {
        }

        /// <summary>
        /// Gets all <see cref="Release"/>s for the specified repository.
        /// </summary>
        /// <remarks>
        /// See the <a href="http://developer.github.com/v3/repos/releases/#list-releases-for-a-repository">API documentation</a> for more information.
        /// </remarks>
        /// <param name="owner">The repository's owner</param>
        /// <param name="name">The repository's name</param>
        /// <exception cref="ApiException">Thrown when a general API error occurs.</exception>
        [ManualRoute("GET", "/repos/{owner}/{repo}/releases")]
        public Task<IReadOnlyList<Release>> GetAll(string owner, string name)
        {
            Ensure.ArgumentNotNullOrEmptyString(owner, nameof(owner));
            Ensure.ArgumentNotNullOrEmptyString(name, nameof(name));

            return GetAll(owner, name, ApiOptions.None);
        }

        /// <summary>
        /// Gets all <see cref="Release"/>s for the specified repository.
        /// </summary>
        /// <remarks>
        /// See the <a href="http://developer.github.com/v3/repos/releases/#list-releases-for-a-repository">API documentation</a> for more information.
        /// </remarks>
        /// <param name="repositoryId">The Id of the repository</param>
        /// <exception cref="ApiException">Thrown when a general API error occurs.</exception>
        [ManualRoute("GET", "/repositories/{id}/releases")]
        public Task<IReadOnlyList<Release>> GetAll(long repositoryId)
        {
            return GetAll(repositoryId, ApiOptions.None);
        }

        /// <summary>
        /// Gets all <see cref="Release"/>s for the specified repository.
        /// </summary>
        /// <remarks>
        /// See the <a href="http://developer.github.com/v3/repos/releases/#list-releases-for-a-repository">API documentation</a> for more information.
        /// </remarks>
        /// <param name="owner">The repository's owner</param>
        /// <param name="name">The repository's name</param>
        /// <param name="options">Options for changing the API response</param>
        /// <exception cref="ApiException">Thrown when a general API error occurs.</exception>
        [ManualRoute("GET", "/repos/{owner}/{repo}/releases")]
        public Task<IReadOnlyList<Release>> GetAll(string owner, string name, ApiOptions options)
        {
            Ensure.ArgumentNotNullOrEmptyString(owner, nameof(owner));
            Ensure.ArgumentNotNullOrEmptyString(name, nameof(name));
            Ensure.ArgumentNotNull(options, nameof(options));

            var endpoint = ApiUrls.Releases(owner, name);
            return ApiConnection.GetAll<Release>(endpoint, null, AcceptHeaders.StableVersion, options);
        }

        /// <summary>
        /// Gets all <see cref="Release"/>s for the specified repository.
        /// </summary>
        /// <remarks>
        /// See the <a href="http://developer.github.com/v3/repos/releases/#list-releases-for-a-repository">API documentation</a> for more information.
        /// </remarks>
        /// <param name="repositoryId">The Id of the repository</param>
        /// <param name="options">Options for changing the API response</param>
        /// <exception cref="ApiException">Thrown when a general API error occurs.</exception>
        [ManualRoute("GET", "/repositories/{id}/releases")]
        public Task<IReadOnlyList<Release>> GetAll(long repositoryId, ApiOptions options)
        {
            Ensure.ArgumentNotNull(options, nameof(options));

            var endpoint = ApiUrls.Releases(repositoryId);
            return ApiConnection.GetAll<Release>(endpoint, null, AcceptHeaders.StableVersion, options);
        }

        /// <summary>
        /// Gets all <see cref="ReleaseAsset"/> for the specified release of the specified repository.
        /// </summary>
        /// <remarks>
        /// See the <a href="http://developer.github.com/v3/repos/releases/#list-assets-for-a-release">API documentation</a> for more information.
        /// </remarks>
        /// <param name="owner">The repository's owner</param>
        /// <param name="name">The repository's name</param>
        /// <param name="id">The id of the <see cref="Release"/>.</param>
        /// <exception cref="ApiException">Thrown when a general API error occurs.</exception>
        [ManualRoute("GET", "/repos/{owner}/{repo}/releases/{release_id}/assets")]
        public Task<IReadOnlyList<ReleaseAsset>> GetAllAssets(string owner, string name, int id)
        {
            Ensure.ArgumentNotNullOrEmptyString(owner, nameof(owner));
            Ensure.ArgumentNotNullOrEmptyString(name, nameof(name));

            return GetAllAssets(owner, name, id, ApiOptions.None);
        }

        /// <summary>
        /// Gets all <see cref="ReleaseAsset"/> for the specified release of the specified repository.
        /// </summary>
        /// <remarks>
        /// See the <a href="http://developer.github.com/v3/repos/releases/#list-assets-for-a-release">API documentation</a> for more information.
        /// </remarks>
        /// <param name="owner">The repository's owner</param>
        /// <param name="name">The repository's name</param>
        /// <param name="id">The id of the <see cref="Release"/>.</param>
        /// <param name="options">Options for changing the API response</param>
        /// <exception cref="ApiException">Thrown when a general API error occurs.</exception>
        [ManualRoute("GET", "/repos/{owner}/{repo}/releases/{release_id}/assets")]
        public Task<IReadOnlyList<ReleaseAsset>> GetAllAssets(string owner, string name, int id, ApiOptions options)
        {
            Ensure.ArgumentNotNullOrEmptyString(owner, nameof(owner));
            Ensure.ArgumentNotNullOrEmptyString(name, nameof(name));
            Ensure.ArgumentNotNull(options, nameof(options));

            var endpoint = ApiUrls.ReleaseAssets(owner, name, id);
            return ApiConnection.GetAll<ReleaseAsset>(endpoint, null, AcceptHeaders.StableVersion, options);
        }
    }
}
