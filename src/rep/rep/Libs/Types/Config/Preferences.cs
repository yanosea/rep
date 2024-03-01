namespace rep.Libs.Types.Config
{
    /// <summary>
    /// preferences
    /// </summary>
    internal sealed class Preferences
    {
        #region props

        /// <summary>
        /// user
        /// </summary>
        public Props.Preferences.User User { get; set; }

        /// <summary>
        /// preferences
        /// </summary>
        public Props.Preferences.Preference Preference { get; set; }

        #endregion

        #region constructor

        /// <summary>
        /// constructor
        /// </summary>
        public Preferences()
        {
            User = new Props.Preferences.User();
            Preference = new Props.Preferences.Preference();
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="user">user</param>>
        /// <param name="preference">preference</param>>
        public Preferences(Props.Preferences.User user, Props.Preferences.Preference preference)
        {
            User = user;
            Preference = preference;
        }

        #endregion
    }
}
