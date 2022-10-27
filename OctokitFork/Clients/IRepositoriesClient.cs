using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Octokit
{
    /// <summary>
    /// A client for GitHub's Repositories API.
    /// </summary>
    /// <remarks>
    /// See the <https://docs.github.com/en/rest/reference/repos">Repositories API documentation</a> for more details.
    /// </remarks>
    public interface IRepositoriesClient
    {
        /// <summary>
        /// Access GitHub's Releases API.
        /// </summary>
        /// <remarks>
        /// Refer to the API documentation for more information: https://developer.github.com/v3/repos/releases/
        /// </remarks>
        IReleasesClient Release { get; }
    }
}
