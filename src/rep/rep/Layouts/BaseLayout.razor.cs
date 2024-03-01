using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace rep.Layouts
{
    /// <summary>
    /// BaseLayout
    /// </summary>
    public partial class BaseLayout
    {
        #region injects

        /// <summary>
        /// NavigationManager
        /// </summary>
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// JSRuntime
        /// </summary>
        [Inject]
        private IJSRuntime JSRuntime { get; set; }

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

                if (preferences.Preference.DarkMode)
                {
                    // if dark mode is true, enable dark mode
                    await JSRuntime.InvokeVoidAsync("enableDarkClass");
                }
                else
                {
                    // if darkmode is false, disable dark mode
                    await JSRuntime.InvokeVoidAsync("disableDarkClass");
                }
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

        #region methods

        /// <summary>
        /// Redirect
        /// </summary>
        /// <param name="href">href</param>
        internal async Task Redirect(string href)
        {
            // start log
            Libs.Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // navigate page
                NavigationManager.NavigateTo(href, true);
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
    }
}
