using System;
using System.Diagnostics;
using System.Globalization;

namespace Octokit
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class NewCheckRunAnnotation
    {
        /// <summary>
        /// Constructs a CheckRunCreateAnnotation request object
        /// </summary>
        /// <param name="path">Required. The path of the file to add an annotation to. For example, assets/css/main.css</param>
        /// <param name="startLine">Required. The start line of the annotation</param>
        /// <param name="endLine">Required. The end line of the annotation</param>
        /// <param name="annotationLevel">Required. The level of the annotation. Can be one of notice, warning, or failure</param>
        /// <param name="message">Required. A short description of the feedback for these lines of code. The maximum size is 64 KB</param>
        public NewCheckRunAnnotation(string path, int startLine, int endLine, CheckAnnotationLevel annotationLevel, string message)
        {
            Path = path;
            StartLine = startLine;
            EndLine = endLine;
            AnnotationLevel = annotationLevel;
            Message = message;
        }

        /// <summary>
        /// Required. The path of the file to add an annotation to. For example, assets/css/main.css
        /// </summary>
        public string Path { get; protected set; }

        /// <summary>
        /// Required. The start line of the annotation
        /// </summary>
        public int StartLine { get; protected set; }

        /// <summary>
        /// Required. The end line of the annotation
        /// </summary>
        public int EndLine { get; protected set; }

        /// <summary>
        /// Required. The start line of the annotation
        /// </summary>
        public int? StartColumn { get; set; }

        /// <summary>
        /// Required. The end line of the annotation
        /// </summary>
        public int? EndColumn { get; set; }

        /// <summary>
        /// Required. The level of the annotation. Can be one of notice, warning, or failure
        /// </summary>
        public StringEnum<CheckAnnotationLevel>? AnnotationLevel { get; protected set; }

        /// <summary>
        /// Required. A short description of the feedback for these lines of code. The maximum size is 64 KB
        /// </summary>
        public string Message { get; protected set; }

        /// <summary>
        /// The title that represents the annotation. The maximum size is 255 characters
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Details about this annotation. The maximum size is 64 KB
        /// </summary>
        public string RawDetails { get; set; }
    }
}