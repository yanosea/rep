namespace rep.Libs.Types.Config
{
    /// <summary>
    /// log
    /// </summary>
    internal sealed class LogConfig
    {
        #region props

        /// <summary>
        /// normal_log
        /// </summary>
        public Props.Log.LogAppender NormalLog { get; set; }

        /// <summary>
        /// error_log
        /// </summary>
        public Props.Log.LogAppender ErrorLog { get; set; }

        #endregion

        #region constructor

        /// <summary>
        /// constructor
        /// </summary>
        public LogConfig()
        {
            NormalLog = new Props.Log.LogAppender()
            {
                LogLevelMax = "WARN",
                LogLevelMin = "INFO"
            };
            ErrorLog = new Props.Log.LogAppender()
            {
                LogLevelMax = "FATAL",
                LogLevelMin = "ERROR"
            };
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="normalLog">normalLog</param>>
        /// <param name="errorLog">errorLog</param>>
        public LogConfig(Props.Log.LogAppender normalLog, Props.Log.LogAppender errorLog)
        {
            NormalLog = normalLog;
            ErrorLog = errorLog;
        }

        #endregion
    }
}
