namespace rep.Libs.Types.Config.Props.Log
{
    /// <summary>
    /// log_appender
    /// </summary>
    internal sealed class LogAppender
    {
        #region props

        /// <summary>
        /// log_level_max
        /// </summary>
        public string LogLevelMax { get; set; }

        /// <summary>
        /// log_level_min
        /// </summary>
        public string LogLevelMin { get; set; }

        #endregion

        #region constructor

        /// <summary>
        /// constructor
        /// </summary>
        public LogAppender()
        {
            LogLevelMax = "WARN";
            LogLevelMin = "INFO";
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="logLevelMax">logLevelMax</param>
        /// <param name="logLevelMin">logLevelMin</param>
        public LogAppender(string logLevelMax, string logLevelMin)
        {
            LogLevelMax = logLevelMax;
            LogLevelMin = logLevelMin;
        }

        #endregion
    }
}
