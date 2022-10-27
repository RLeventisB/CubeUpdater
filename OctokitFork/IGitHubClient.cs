using System;

namespace Octokit
{
    /// <summary>
    /// A Client for the GitHub API v3. You can read more about the API here: http://developer.github.com.
    /// </summary>
    public interface IGitHubClient
    {
        /// <summary>
        /// Provides a client connection to make rest requests to HTTP endpoints.
        /// </summary>
        IConnection Connection { get; }

        /// <summary>
        /// Access GitHub's Repositories API.
        /// </summary>
        /// <remarks>
        /// Refer to the API documentation for more information: https://developer.github.com/v3/repos/
        /// </remarks>
        IRepositoriesClient Repository { get; }
    }
}
