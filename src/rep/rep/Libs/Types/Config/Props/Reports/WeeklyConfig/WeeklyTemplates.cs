namespace rep.Libs.Types.Config.Props.Reports.WeeklyConfig
{
    /// <summary>
    /// weekly_templates
    /// </summary>
    internal sealed class WeeklyTemplates
    {
        #region props

        /// <summary>
        /// weekly_subject_template
        /// </summary>
        public string WeeklySubjectTemplate { get; set; }

        /// <summary>
        /// weekly_head_template
        /// </summary>
        public string WeeklyHeadTemplate { get; set; }

        /// <summary>
        /// weekly_foot_template
        /// </summary>
        public string WeeklyFootTemplate { get; set; }

        #endregion

        #region constructor

        /// <summary>
        /// constructor
        /// </summary>
        public WeeklyTemplates()
        {
            WeeklySubjectTemplate = "example";
            WeeklyHeadTemplate = "example";
            WeeklyFootTemplate = "example";
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="weeklySubjectTemplate">weeklySubjectTemplate</param>
        /// <param name="weeklyHeadTemplate">weeklyHeadTemplate</param>
        /// <param name="weeklyFootTemplate">weeklyFootTemplate</param>
        public WeeklyTemplates(string weeklySubjectTemplate, string weeklyHeadTemplate, string weeklyFootTemplate)
        {
            WeeklySubjectTemplate = weeklySubjectTemplate;
            WeeklyHeadTemplate = weeklyHeadTemplate;
            WeeklyFootTemplate = weeklyFootTemplate;
        }

        #endregion
    }
}
