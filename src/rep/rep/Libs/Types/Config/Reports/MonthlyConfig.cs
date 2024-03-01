namespace rep.Libs.Types.Config.Reports
{
    /// <summary>
    /// monthly_conf
    /// </summary>
    internal sealed class MonthlyConfig
    {
        #region props

        /// <summary>
        /// monthly_templates
        /// </summary>
        public Props.Reports.MonthlyConfig.MonthlyTemplates MonthlyTemplates { get; set; }

        #endregion

        #region constructor

        /// <summary>
        /// constructor
        /// </summary>
        public MonthlyConfig()
        {
            MonthlyTemplates = new Props.Reports.MonthlyConfig.MonthlyTemplates();
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="monthlyTemplates">monthlyTemplates</param>>
        public MonthlyConfig(Props.Reports.MonthlyConfig.MonthlyTemplates monthlyTemplates)
        {
            MonthlyTemplates = monthlyTemplates;
        }

        #endregion
    }
}
