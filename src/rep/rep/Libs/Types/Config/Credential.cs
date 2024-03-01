namespace rep.Libs.Types.Config
{
    /// <summary>
    /// credential
    /// </summary>
    internal sealed class Credential
    {
        #region props

        /// <summary>
        /// account
        /// </summary>
        public Props.Credential.Account Account { get; set; }

        /// <summary>
        /// smtp
        /// </summary>
        public Props.Credential.Smtp Smtp { get; set; }

        #endregion

        #region constructor

        /// <summary>
        /// constructor
        /// </summary>
        public Credential()
        {
            Account = new Props.Credential.Account();
            Smtp = new Props.Credential.Smtp();
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="account">account</param>>
        /// <param name="smtp">smtp</param>>
        public Credential(Props.Credential.Account account, Props.Credential.Smtp smtp)
        {
            Account = account;
            Smtp = smtp;
        }

        #endregion
    }
}
