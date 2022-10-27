using System.Diagnostics;
using System.Globalization;

namespace Octokit
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class ProjectCardUpdate
    {
        public ProjectCardUpdate()
        {
        }

        /// <summary>
        /// The new note of the card.
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Archive/Unarchive the card.
        /// </summary>
        public bool? Archived { get; set; }

        internal string DebuggerDisplay
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, "Note: {0}, Archived: {1}", Note, Archived?.ToString() ?? "null");
            }
        }
    }
}
