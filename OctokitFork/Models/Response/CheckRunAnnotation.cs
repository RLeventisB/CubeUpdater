using System.Diagnostics;

namespace Octokit
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class CheckRunAnnotation
    {
        public CheckRunAnnotation()
        {
        }

        public CheckRunAnnotation(string path, string blobHref, int startLine, int endLine, int? startColumn, int? endColumn, CheckAnnotationLevel? annotationLevel, string message, string title, string rawDetails)
        {
            Path = path;
            BlobHref = blobHref;
            StartLine = startLine;
            EndLine = endLine;
            StartColumn = startColumn;
            EndColumn = endColumn;
            AnnotationLevel = annotationLevel;
            Message = message;
            Title = title;
            RawDetails = rawDetails;
        }

        /// <summary>
        /// The path of the file the annotation refers to
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// The file's full blob URL
        /// </summary>
        public string BlobHref { get; private set; }

        /// <summary>
        /// The start line of the annotation
        /// </summary>
        public int StartLine { get; private set; }

        /// <summary>
        /// The end line of the annotation
        /// </summary>
        public int EndLine { get; private set; }

        /// <summary>
        /// The start line of the annotation
        /// </summary>
        public int? StartColumn { get; private set; }

        /// <summary>
        /// The end line of the annotation
        /// </summary>
        public int? EndColumn { get; private set; }

        /// <summary>
        /// The level of the annotation. Can be one of notice, warning, or failure
        /// </summary>
        public StringEnum<CheckAnnotationLevel>? AnnotationLevel { get; private set; }

        /// <summary>
        /// A short description of the feedback for these lines of code
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// The title that represents the annotation
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Details about this annotation
        /// </summary>
        public string RawDetails { get; private set; }
    }
}
