namespace rep.Libs.Types.Config.Reports
{
    /// <summary>
    /// weekly_conf
    /// </summary>
    internal sealed class WeeklyConfig
    {
        #region props

        /// <summary>
        /// weekly_templates
        /// </summary>
        public Props.Reports.WeeklyConfig.WeeklyTemplates WeeklyTemplates { get; set; }

        #endregion

        #region constructor

        /// <summary>
        /// constructor
        /// </summary>
        public WeeklyConfig()
        {
            WeeklyTemplates = new Props.Reports.WeeklyConfig.WeeklyTemplates();
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="weeklyTemplates">weeklyTemplates</param>>
        public WeeklyConfig(Props.Reports.WeeklyConfig.WeeklyTemplates weeklyTemplates)
        {
            WeeklyTemplates = weeklyTemplates;
        }

        #endregion
    }
}
