using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace rep.Pages.config
{
    /// <summary>
    /// Preferences
    /// </summary>
    public partial class Preferences
    {
        #region injects

        /// <summary>
        /// JSRuntime
        /// </summary>
        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        #endregion

        #region props

        /// <summary>
        /// firstName
        /// </summary>
        private string firstName { get; set; }

        /// <summary>
        /// lastName
        /// </summary>
        private string lastName { get; set; }

        /// <summary>
        /// company
        /// </summary>
        private string company { get; set; }

        /// <summary>
        /// projectName
        /// </summary>
        private string projectName { get; set; }

        /// <summary>
        /// darkMode
        /// </summary>
        private bool darkMode { get; set; }

        /// <summary>
        /// checkUpdates
        /// </summary>
        private bool checkUpdates { get; set; }

        /// <summary>
        /// checkUpdatesOnOpening
        /// </summary>
        private bool checkUpdatesOnOpening { get; set; }

        /// <summary>
        /// skipTopPage
        /// </summary>
        private bool skipTopPage { get; set; }

        /// <summary>
        /// confirmBeforeSend
        /// </summary>
        private bool confirmBeforeSend { get; set; }

        /// <summary>
        /// saveTextFile
        /// </summary>
        private bool saveTextFile { get; set; }

        /// <summary>
        /// saveDestination
        /// </summary>
        private bool saveDestination { get; set; }

        /// <summary>
        /// addDateToAttachedFile
        /// </summary>
        private bool addDateToAttachedFile { get; set; }

        /// <summary>
        /// exitAfterSend
        /// </summary>
        private bool exitAfterSend { get; set; }

        #endregion

        #region events

        /// <summary>
        /// OnInitialized
        /// </summary>
        protected override async void OnInitialized()
        {
            // start log
            Libs.Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // get preferences config
                var preferences = Libs.DataHolder.Instance.GetPreferences();
                firstName = preferences.User.FirstName;
                lastName = preferences.User.LastName;
                company = preferences.User.Company;
                projectName = preferences.User.ProjectName;
                darkMode = preferences.Preference.DarkMode;
                checkUpdates = preferences.Preference.CheckUpdates;
                checkUpdatesOnOpening = preferences.Preference.CheckUpdatesOnOpening;
                skipTopPage = preferences.Preference.SkipTopPage;
                confirmBeforeSend = preferences.Preference.ConfirmBeforeSend;
                saveTextFile = preferences.Preference.SaveTextFile;
                saveDestination = preferences.Preference.SaveDestination;
                addDateToAttachedFile = preferences.Preference.AddDateToAttachedFile;
                exitAfterSend = preferences.Preference.ExitAfterSend;
            }
            catch (Exception ex)
            {
                // if exception occured, show dialog
                await Libs.Services.DialogService.Instance.ShowAlertAsync(Resources.Text.Components.DialogCaptionFailure, Resources.Text.Messages.FailureSomethingWrongOccured);

                // fatal log
                Libs.Logger.Instance.TraceFatal(Resources.Text.Components.TextLogTagFrontendFatal, Resources.Text.LogMessages.FailureSomethingWrongOccured, ex);

                // teminate rep
                Environment.Exit(1);
            }
            finally
            {
                // end log
                Libs.Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        #endregion

        #region prevate methods

        /// <summary>
        /// SavePreferences
        /// </summary>
        private async Task SavePreferences()
        {
            // start log
            Libs.Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // create new preferences object
                var preferences = new Libs.Types.Config.Preferences()
                {
                    User = new Libs.Types.Config.Props.Preferences.User()
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Company = company,
                        ProjectName = projectName
                    },
                    Preference = new Libs.Types.Config.Props.Preferences.Preference()
                    {
                        DarkMode = darkMode,
                        CheckUpdates = checkUpdates,
                        CheckUpdatesOnOpening = checkUpdatesOnOpening,
                        SkipTopPage = skipTopPage,
                        ConfirmBeforeSend = confirmBeforeSend,
                        AddDateToAttachedFile = addDateToAttachedFile,
                        SaveTextFile = saveTextFile,
                        SaveDestination = saveDestination,
                        ExitAfterSend = exitAfterSend,
                    }
                };

                // write preference.toml with new credential object
                Libs.IO.ConfigReadWriter.WritePreferences(preferences);

                // re-set new preference to dataholder
                Libs.DataHolder.Instance.SetPreferences(preferences);

                if (darkMode)
                {
                    // if dark mode enable, enable dark mode
                    await JSRuntime.InvokeVoidAsync("enableDarkClass");
                }
                else
                {
                    // if dark mode disable, disable dark mode
                    await JSRuntime.InvokeVoidAsync("disableDarkClass");
                }

                // show success dialog
                await Libs.Services.DialogService.Instance.ShowAlertAsync(Resources.Text.Components.DialogCaptionSucceeded, Resources.Text.Messages.SucceedSavingConfig);
            }
            catch (Exception ex)
            {
                // if exception occured, show dialog
                await Libs.Services.DialogService.Instance.ShowAlertAsync(Resources.Text.Components.DialogCaptionFailure, Resources.Text.Messages.FailureSavingConfig);

                // fatal log
                Libs.Logger.Instance.TraceFatal(Resources.Text.Components.TextLogTagFrontendFatal, Resources.Text.LogMessages.FailureSavingConfig, ex);
            }
            finally
            {
                // end log
                Libs.Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        #endregion
    }
}
