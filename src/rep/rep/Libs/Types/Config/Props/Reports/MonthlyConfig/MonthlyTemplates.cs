namespace rep.Libs.Types.Config.Props.Reports.MonthlyConfig
{
    /// <summary>
    /// monthly_templates
    /// </summary>
    internal sealed class MonthlyTemplates
    {
        #region props

        /// <summary>
        /// monthly_subject_template
        /// </summary>
        public string MonthlySubjectTemplate { get; set; }

        /// <summary>
        /// monthly_head_template
        /// </summary>
        public string MonthlyHeadTemplate { get; set; }

        /// <summary>
        /// monthly_foot_template
        /// </summary>
        public string MonthlyFootTemplate { get; set; }

        #endregion

        #region constructor

        /// <summary>
        /// constructor
        /// </summary>
        public MonthlyTemplates()
        {
            MonthlySubjectTemplate = "example";
            MonthlyHeadTemplate = "example";
            MonthlyFootTemplate = "example";
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="monthlySubjectTemplate">monthlySubjectTemplate</param>
        /// <param name="monthlyHeadTemplate">monthlyHeadTemplate</param>
        /// <param name="monthlyFootTemplate">monthlyFootTemplate</param>
        public MonthlyTemplates(string monthlySubjectTemplate, string monthlyHeadTemplate, string monthlyFootTemplate)
        {
            MonthlySubjectTemplate = monthlySubjectTemplate;
            MonthlyHeadTemplate = monthlyHeadTemplate;
            MonthlyFootTemplate = monthlyFootTemplate;
        }

        #endregion
    }
}
