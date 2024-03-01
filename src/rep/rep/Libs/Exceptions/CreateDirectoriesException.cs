using System;

namespace rep.Libs.Exceptions
{
    /// <summary>
    /// CreateDirectoriesException
    /// </summary>
    internal sealed class CreateDirectoriesException : Exception
    {
        #region constructor

        /// <summary>
        /// constructor
        /// </summary>
        internal CreateDirectoriesException() : base()
        {

        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="message">message of exception</param>
        /// <param name="ex">exception</param>
        /// <param name="methodName">method name called this method</param>
        /// <param name="sourceFilePath">source file path of method</param>
        /// <param name="sourceLineNumber">souce line of method</param>
        internal CreateDirectoriesException(string message, Exception ex,
            [System.Runtime.CompilerServices.CallerMemberName] string methodName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0) : base(message, ex)
        {
            // get class name from file path
            string className = System.IO.Path.GetFileName(sourceFilePath).Replace(Resources.Text.Components.StringCSFileExtension, string.Empty);

            // create log tag
            var logTag = string.Format(Resources.Text.Formats.LogTag, className, methodName, sourceLineNumber);

            // output fatal log
            Logger.Instance.TraceFatal(logTag, message, ex);
        }

        #endregion
    }
}