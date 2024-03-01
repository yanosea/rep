using System;

namespace rep.Libs.Exceptions
{
    /// <summary>
    /// AbnormalInputException 
    /// </summary>
    internal sealed class AbnormalInputException : Exception
    {

        #region fields

        /// <summary>
        /// AbnormalParamName
        /// </summary>
        internal string AbnormalParamName { get; set; }

        /// <summary>
        /// FormattedMessage
        /// </summary>
        internal string FormattedMessage { get; set; }

        #endregion

        #region constructor

        /// <summary>
        /// constructor
        /// </summary>
        internal AbnormalInputException() : base()
        {

        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="abnormalParamName">abnormalParamName</param>
        /// <param name="ex">exception</param>
        /// <param name="methodName">method name called this method</param>
        /// <param name="sourceFilePath">source file path of method</param>
        /// <param name="sourceLineNumber">souce line of method</param>
        internal AbnormalInputException(string abnormalParamName, Exception ex,
            [System.Runtime.CompilerServices.CallerMemberName] string methodName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0) : base(abnormalParamName, ex)
        {
            // set abnormal param name
            AbnormalParamName = abnormalParamName;

            // get class name from file path
            string className = System.IO.Path.GetFileName(sourceFilePath).Replace(Resources.Text.Components.StringCSFileExtension, string.Empty);

            // format message
            var message = abnormalParamName.Equals(Resources.Text.Messages.FailureInputtingAbnormalTime) ?
                            abnormalParamName : string.Format(Resources.Text.Messages.FailureInputtingAbnormalParam, this.AbnormalParamName);
            FormattedMessage = message;

            // format log message
            var logMessage = abnormalParamName.Equals(Resources.Text.Messages.FailureInputtingAbnormalTime) ?
                                Resources.Text.LogMessages.FailureInputtingAbnormalTime : string.Format(Resources.Text.LogMessages.FailureInputtingAbnormalParam, this.AbnormalParamName);

            // create log tag
            var logTag = string.Format(Resources.Text.Formats.LogTag, className, methodName, sourceLineNumber);

            // output fatal log
            Logger.Instance.TraceFatal(logTag, logMessage, ex);
        }

        #endregion
    }
}