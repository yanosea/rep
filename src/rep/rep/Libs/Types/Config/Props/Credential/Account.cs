namespace rep.Libs.Types.Config.Props.Credential
{
    /// <summary>
    /// account
    /// </summary>
    internal sealed class Account
    {
        #region props

        /// <summary>
        /// mailaddress
        /// </summary>
        public string Mailaddress { get; set; }

        /// <summary>
        /// password
        /// </summary>
        public string Password { get; set; }

        #endregion

        #region constructor

        /// <summary>
        /// constructor
        /// </summary>
        public Account()
        {
            Mailaddress = "example@example.com";
            Password = "example";
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="mailaddress">mailaddress</param>
        /// <param name="password">password</param>
        public Account(string mailaddress, string password)
        {
            Mailaddress = mailaddress;
            Password = password;
        }

        #endregion
    }
}
