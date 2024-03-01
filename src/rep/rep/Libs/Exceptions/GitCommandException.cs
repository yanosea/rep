using System;

namespace rep.Libs.Exceptions
{
    /// <summary>
    /// GitCommandException
    /// </summary>
    internal sealed class GitCommandException : Exception
    {
        #region fields

        /// <summary>
        /// FailedCommandName
        /// </summary>
        internal string? FailedCommandName;

        /// <summary>
        /// FormattedMessage
        /// </summary>
        internal string FormattedMessage { get; set; }

        #endregion

        #region constructor

        /// <summary>
        /// constructor
        /// </summary>
        internal GitCommandException() : base()
        {

        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="failedCommandName">failedCommandName</param>
        /// <param name="ex">exception</param>
        /// <param name="methodName">method name called this method</param>
        /// <param name="sourceFilePath">source file path of method</param>
        /// <param name="sourceLineNumber">souce line of method</param>
        internal GitCommandException(string failedCommandName, Exception ex,
            [System.Runtime.CompilerServices.CallerMemberName] string methodName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0) : base(failedCommandName, ex)
        {
            // get class name from file path
            string className = System.IO.Path.GetFileName(sourceFilePath).Replace(Resources.Text.Components.StringCSFileExtension, string.Empty);

            // format messagge
            var message = string.Format(Resources.Text.LogMessages.FailureGitCommand, failedCommandName);
            FormattedMessage = message;

            // create log tag
            var logTag = string.Format(Resources.Text.Formats.LogTag, className, methodName, sourceLineNumber);

            // output fatal log
            Logger.Instance.TraceFatal(logTag, message, ex);
        }

        #endregion
    }
}