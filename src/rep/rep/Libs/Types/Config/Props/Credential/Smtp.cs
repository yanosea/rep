namespace rep.Libs.Types.Config.Props.Credential
{
    /// <summary>
    /// smtp
    /// </summary>
    internal sealed class Smtp
    {
        #region props

        /// <summary>
        /// smtp_server
        /// </summary>
        public string SmtpServer { get; set; }

        /// <summary>
        /// smtp_port
        /// </summary>
        public int SmtpPort { get; set; }

        #endregion

        #region constructor

        /// <summary>
        /// constructor
        /// </summary>
        public Smtp()
        {
            SmtpServer = "example.com";
            SmtpPort = 9999;
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="smtpServer">smtpServer</param>
        /// <param name="smtpPort">smtpPort</param>
        public Smtp(string smtpServer, int smtpPort)
        {
            SmtpServer = smtpServer;
            SmtpPort = SmtpPort;
        }

        #endregion
    }
}
