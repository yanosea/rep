namespace rep.Libs.Types.Config.Props.Reports.DailyConfig
{
    /// <summary>
    /// daily_templates
    /// </summary>
    internal sealed class DailyTemplates
    {
        #region props

        /// <summary>
        /// opening_subject_template
        /// </summary>
        public string OpeningSubjectTemplate { get; set; }

        /// <summary>
        /// opening_head_template
        /// </summary>
        public string OpeningHeadTemplate { get; set; }


        /// <summary>
        /// opening_foot_template
        /// </summary>
        public string OpeningFootTemplate { get; set; }

        /// <summary>
        /// closing_subject_template
        /// </summary>
        public string ClosingSubjectTemplate { get; set; }

        /// <summary>
        /// closing_head_template
        /// </summary>
        public string ClosingHeadTemplate { get; set; }


        /// <summary>
        /// closing_foot_template
        /// </summary>
        public string ClosingFootTemplate { get; set; }

        #endregion

        #region constructor

        /// <summary>
        /// constructor
        /// </summary>
        public DailyTemplates()
        {
            OpeningSubjectTemplate = "example";
            OpeningHeadTemplate = "example";
            OpeningFootTemplate = "example";
            ClosingSubjectTemplate = "example";
            ClosingHeadTemplate = "example";
            ClosingFootTemplate = "example";
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="openingSubjectTemplate">openingSubjectTemplate</param>
        /// <param name="openingHeadTemplate">openingHeadTemplate</param>
        /// <param name="openingFootTemplate">openingFootTemplate</param>
        /// <param name="closingSubjectTemplate">closingSubjectTemplate</param>
        /// <param name="closingHeadTemplate">closingHeadTemplate</param>
        /// <param name="closingFootTemplate">closingFootTemplate</param>
        public DailyTemplates(string openingSubjectTemplate, string openingHeadTemplate, string openingFootTemplate,
            string closingSubjectTemplate, string closingHeadTemplate, string closingFootTemplate)
        {
            OpeningSubjectTemplate = openingSubjectTemplate;
            OpeningHeadTemplate = openingHeadTemplate;
            OpeningFootTemplate = openingFootTemplate;
            ClosingSubjectTemplate = closingSubjectTemplate;
            ClosingHeadTemplate = closingHeadTemplate;
            ClosingFootTemplate = closingFootTemplate;
        }

        #endregion
    }
}
