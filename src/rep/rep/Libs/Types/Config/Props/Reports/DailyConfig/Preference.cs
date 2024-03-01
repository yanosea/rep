namespace rep.Libs.Types.Config.Props.Reports.DailyConfig
{
    /// <summary>
    /// preference
    /// </summary>
    internal sealed class Preference
    {
        #region props

        /// <summary>
        /// threshold_time
        /// </summary>
        public string ThresholdTime { get; set; }

        /// <summary>
        /// interval_minutes
        /// </summary>
        public int IntervalMinutes { get; set; }

        /// <summary>
        /// time_format
        /// </summary>
        public string TimeFormat { get; set; }

        #endregion

        #region constructor

        /// <summary>
        /// constructor
        /// </summary>
        public Preference()
        {
            ThresholdTime = "14:00";
            IntervalMinutes = 15;
            TimeFormat = "■作業時間\r\n  %from% - %to%    - %worktime% H";
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="thresholdTime">thresholdTime</param>
        /// <param name="intervalMinutes">intervalMinutes</param>
        /// <param name="timeFormat">timeFormat</param>
        public Preference(string thresholdTime, int intervalMinutes, string timeFormat)
        {
            ThresholdTime = thresholdTime;
            IntervalMinutes = intervalMinutes;
            TimeFormat = timeFormat;
        }

        #endregion
    }
}
