namespace rep.Libs.Types.Config.Reports
{
    /// <summary>
    /// daily_conf
    /// </summary>
    internal sealed class DailyConfig
    {
        #region props

        /// <summary>
        /// preferences
        /// </summary>
        public Props.Reports.DailyConfig.Preference Preferences { get; set; }

        /// <summary>
        /// daily_templates
        /// </summary>
        public Props.Reports.DailyConfig.DailyTemplates DailyTemplates { get; set; }

        #endregion

        #region constructor

        /// <summary>
        /// constructor
        /// </summary>
        public DailyConfig()
        {
            Preferences = new Props.Reports.DailyConfig.Preference();
            DailyTemplates = new Props.Reports.DailyConfig.DailyTemplates();
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="preferences">preferences</param>>
        /// <param name="dailyTemplates">dailyTemplates</param>>
        public DailyConfig(Props.Reports.DailyConfig.Preference preferences, Props.Reports.DailyConfig.DailyTemplates dailyTemplates)
        {
            Preferences = preferences;
            DailyTemplates = dailyTemplates;
        }

        #endregion
    }
}
