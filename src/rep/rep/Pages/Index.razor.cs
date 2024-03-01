using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace rep.Pages
{
    /// <summary>
    /// Index
    /// </summary>
    public partial class Index
    {

        #region injects

        /// <summary>
        /// JSRuntime
        /// </summary>
        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        #endregion

        #region readonlys

        /// <summary>
        /// prodname
        /// </summary>
        private static readonly string prodname = Assembly.GetEntryAssembly().GetCustomAttributes<AssemblyTitleAttribute>().FirstOrDefault().Title;

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
                if (!Libs.DataHolder.Instance.IsInitializedSuccessfully())
                {
                    // if failure with initializing, show dialog
                    await Libs.Services.DialogService.Instance.ShowAlertAsync(Resources.Text.Components.DialogCaptionFailure, Resources.Text.Messages.FailureInitializing);

                    // fatal log
                    Libs.Logger.Instance.TraceFatal(Resources.Text.Components.TextLogTagFrontendFatal, Resources.Text.LogMessages.FailureInitializing, new Libs.Exceptions.UnexpectedException());

                    // teminate rep
                    Environment.Exit(1);
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

        /// <summary>
        /// OnAfterRenderAsync
        /// </summary>
        protected override async Task OnAfterRenderAsync(bool _)
        {
            // start log
            Libs.Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                if (Libs.DataHolder.Instance.IsStartUp())
                {
                    if (Libs.DataHolder.Instance.GetPreferences().Preference.CheckUpdates)
                    {
                        // if start up and config enabled, declare vars for parsed time
                        DateTime thresholdTime;

                        if (!string.IsNullOrEmpty(Libs.DataHolder.Instance.GetDailyConfig().Preferences.ThresholdTime)
                            && DateTime.TryParse(Libs.DataHolder.Instance.GetDailyConfig().Preferences.ThresholdTime, out thresholdTime))
                        {
                            // if threshold time correct
                            if (Int32.Parse(DateTime.Now.ToString("HHmm")) <= Int32.Parse(DateTime.Parse(Libs.DataHolder.Instance.GetDailyConfig().Preferences.ThresholdTime).ToString("HHmm")))
                            {
                                // if is opening
                                if (Libs.DataHolder.Instance.GetPreferences().Preference.CheckUpdatesOnOpening)
                                {
                                    // if config enabled, show modal
                                    await JSRuntime.InvokeVoidAsync("showModal");

                                    // info log
                                    Libs.Logger.Instance.TraceInformation(Resources.Text.LogMessages.CheckUpdates);

                                    // if config enabled, check updates
                                    var checkUpdatesResult = Libs.FrontendUtility.CheckUpdates(Libs.BackendUtility.GetCurrentVersion());

                                    // hide modal
                                    await JSRuntime.InvokeVoidAsync("hideModal");

                                    if (checkUpdatesResult.Item1)
                                    {
                                        // if has updates, show dialog
                                        var confirmResult = await Libs.Services.DialogService.Instance.ShowConfirmationAsync(Resources.Text.Components.DialogCaptionConfirm,
                                            string.Format(Resources.Text.Messages.ConfirmUpdatingRep, checkUpdatesResult.Item2, checkUpdatesResult.Item3));

                                        if (confirmResult)
                                        {
                                            // info log
                                            Libs.Logger.Instance.TraceInformation(Resources.Text.LogMessages.ExecuteUpdate);

                                            // if yes selected in dialog, execute update
                                            Libs.FrontendUtility.ExecuteUpdate();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // if is closing, if config enabled, show modal
                                await JSRuntime.InvokeVoidAsync("showModal");

                                // info log
                                Libs.Logger.Instance.TraceInformation(Resources.Text.LogMessages.CheckUpdates);

                                // if config enabled, check updates
                                var checkUpdatesResult = Libs.FrontendUtility.CheckUpdates(Libs.BackendUtility.GetCurrentVersion());

                                // hide modal
                                await JSRuntime.InvokeVoidAsync("hideModal");

                                if (checkUpdatesResult.Item1)
                                {
                                    // if has updates, show dialog
                                    var confirmResult = await Libs.Services.DialogService.Instance.ShowConfirmationAsync(Resources.Text.Components.DialogCaptionConfirm,
                                        string.Format(Resources.Text.Messages.ConfirmUpdatingRep, checkUpdatesResult.Item2, checkUpdatesResult.Item3));

                                    if (confirmResult)
                                    {
                                        // info log
                                        Libs.Logger.Instance.TraceInformation(Resources.Text.LogMessages.ExecuteUpdate);

                                        // if yes selected in dialog, execute update
                                        Libs.FrontendUtility.ExecuteUpdate();
                                    }
                                }

                            }
                        }
                        else
                        {
                            // if threshold time is abnormal value, throw abnormal input exception
                            throw new Libs.Exceptions.AbnormalInputException(nameof(thresholdTime), new Libs.Exceptions.UnexpectedException());
                        }
                    }

                    if (Libs.DataHolder.Instance.GetPreferences().Preference.SkipTopPage && Libs.DataHolder.Instance.IsStartUp())
                    {
                        // if config enable and start up, redirect to write/daily
                        await Redirect(Resources.Text.Pages.PathWriteDaily);

                        // info log
                        Libs.Logger.Instance.TraceInformation(Resources.Text.LogMessages.SkipTopPageEnable);
                    }
                }

                // set startup flag false
                Libs.DataHolder.Instance.SetStartUp(false);
            }
            catch (Libs.Exceptions.AbnormalInputException ex)
            {
                // if abnormal input exception occured, show dialog
                await Libs.Services.DialogService.Instance.ShowAlertAsync(Resources.Text.Components.DialogCaptionWarning, ex.FormattedMessage);

                // warn log
                Libs.Logger.Instance.TraceWarning(ex.FormattedMessage);
            }
            catch (Exception ex)
            {
                // if exception occured, show dialog
                await Libs.Services.DialogService.Instance.ShowAlertAsync(Resources.Text.Components.DialogCaptionFailure, Resources.Text.Messages.FailureSomethingWrongOccured);

                // fatal log
                Libs.Logger.Instance.TraceFatal(Resources.Text.Components.TextLogTagFrontendFatal, Resources.Text.Messages.FailureSomethingWrongOccured, new Libs.Exceptions.UnexpectedException(ex.Message, ex));

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
