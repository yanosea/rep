namespace rep.Libs.Types.Config.Props.Preferences
{
    /// <summary>
    /// preference
    /// </summary>
    internal sealed class Preference
    {
        #region props

        /// <summary>
        /// dark_mode
        /// </summary>
        public bool DarkMode { get; set; }

        /// <summary>
        /// check_updates
        /// </summary>
        public bool CheckUpdates { get; set; }

        /// <summary>
        /// check_updates_on_opening
        /// </summary>
        public bool CheckUpdatesOnOpening { get; set; }

        /// <summary>
        /// skip_top_page
        /// </summary>
        public bool SkipTopPage { get; set; }

        /// <summary>
        /// confirm_before_send
        /// </summary>
        public bool ConfirmBeforeSend { get; set; }

        /// <summary>
        /// save_text_file
        /// </summary>
        public bool SaveTextFile { get; set; }

        /// <summary>
        /// SaveDestination
        /// </summary>
        public bool SaveDestination { get; set; }

        /// <summary>
        /// AddDateToAttachedFile 
        /// </summary>
        public bool AddDateToAttachedFile { get; set; }

        /// <summary>
        /// exit_after_send
        /// </summary>
        public bool ExitAfterSend { get; set; }

        #endregion

        #region constructor

        /// <summary>
        /// constructor
        /// </summary>
        public Preference()
        {
            DarkMode = false;
            CheckUpdates = true;
            CheckUpdatesOnOpening = false;
            SkipTopPage = false;
            ConfirmBeforeSend = true;
            SaveTextFile = true;
            SaveDestination = false;
            AddDateToAttachedFile = true;
            ExitAfterSend = true;
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="darkMode">darkMode</param>
        /// <param name="checkUpdates">checkUpdates</param>
        /// <param name="checkUpdatesOnOpening">checkUpdatesOnOpening</param>
        /// <param name="skipTopPage">skipTopPage</param>
        /// <param name="confirmBeforeSend">confirmBeforeSend</param>
        /// <param name="saveTextFile">saveTextFile</param>
        /// <param name="saveDestination">saveDestination</param>
        /// <param name="addDateToAttachedFile">addDateToAttachedFile</param>
        /// <param name="exitAfterSend">exitAfterSend</param>
        public Preference(bool darkMode, bool checkUpdates, bool checkUpdatesOnOpening, bool skipTopPage, bool confirmBeforeSend, bool saveTextFile, bool saveDestination, bool addDateToAttachedFile, bool exitAfterSend)
        {
            DarkMode = darkMode;
            CheckUpdates = checkUpdates;
            CheckUpdatesOnOpening = checkUpdatesOnOpening;
            SkipTopPage = skipTopPage;
            ConfirmBeforeSend = confirmBeforeSend;
            SaveTextFile = saveTextFile;
            SaveDestination = saveDestination;
            AddDateToAttachedFile = addDateToAttachedFile;
            ExitAfterSend = exitAfterSend;
        }

        #endregion
    }
}
