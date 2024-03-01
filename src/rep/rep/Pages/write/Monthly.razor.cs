﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace rep.Pages.write
{
    /// <summary>
    /// Monthly
    /// </summary>
    public partial class Monthly : IDisposable
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
        /// subject
        /// </summary>
        private string subject { get; set; }

        /// <summary>
        /// head
        /// </summary>
        private string head { get; set; }

        /// <summary>
        /// body
        /// </summary>
        private string body { get; set; }

        /// <summary>
        /// foot
        /// </summary>
        private string foot { get; set; }

        /// <summary>
        /// tos
        /// </summary>
        private List<Libs.Types.Config.Props.Dests.To> tos = new List<Libs.Types.Config.Props.Dests.To>();

        /// <summary>
        /// ccs
        /// </summary>
        private List<Libs.Types.Config.Props.Dests.Cc> ccs = new List<Libs.Types.Config.Props.Dests.Cc>();

        /// <summary>
        /// bccs
        /// </summary>
        private List<Libs.Types.Config.Props.Dests.Bcc> bccs = new List<Libs.Types.Config.Props.Dests.Bcc>();

        /// <summary>
        /// monthlyFiles
        /// </summary>
        private List<IBrowserFile> monthlyFiles = new List<IBrowserFile>();

        /// <summary>
        /// isSelectedDaily 
        /// </summary>
        private bool isSelectedDaily { get; set; }

        /// <summary>
        /// isSelectedWeekly
        /// </summary>
        private bool isSelectedWeekly { get; set; }

        /// <summary>
        /// isSelectedMonthly 
        /// </summary>
        private bool isSelectedMonthly { get; set; }

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
                // get monthly config
                var monthlyConfig = Libs.DataHolder.Instance.GetMonthlyConfig();

                if (Libs.DataHolder.Instance.IsFirstInitializeMonthly())
                {
                    // if first initialize, get monthly dest config
                    var monthlyDestConfig = Libs.DataHolder.Instance.GetMonthlyDestConfig();
                    tos = monthlyDestConfig.To;
                    ccs = monthlyDestConfig.Cc;
                    bccs = monthlyDestConfig.Bcc;

                    // binding params
                    subject = Libs.FrontendUtility.BindParams(monthlyConfig.MonthlyTemplates.MonthlySubjectTemplate);
                    head = Libs.FrontendUtility.BindParams(monthlyConfig.MonthlyTemplates.MonthlyHeadTemplate);
                    body = Libs.Database.IO.DBReadWriter.GetLastMonthly().BODY;
                    foot = Libs.FrontendUtility.BindParams(monthlyConfig.MonthlyTemplates.MonthlyFootTemplate);

                    if (Libs.DataHolder.Instance.IsFirstInitializeWrite())
                    {
                        // if first initialize write, set selected reports with command line arguments
                        isSelectedDaily = Libs.DataHolder.Instance.GetCommandLineArguments()[0];
                        isSelectedWeekly = Libs.DataHolder.Instance.GetCommandLineArguments()[1];
                        isSelectedMonthly = Libs.DataHolder.Instance.GetCommandLineArguments()[2];

                        // set first initialize write flag, false
                        Libs.DataHolder.Instance.SetFirstInitializeWrite(false);
                    }
                    else
                    {
                        // set selected reports with data holder
                        isSelectedDaily = Libs.DataHolder.Instance.IsSelectedDaily();
                        isSelectedWeekly = Libs.DataHolder.Instance.IsSelectedWeekly();
                        isSelectedMonthly = Libs.DataHolder.Instance.IsSelectedMonthly();
                    }
                }
                else
                {
                    // if not first initialize monthly, get unsend monthly and set input form
                    tos = Libs.FrontendUtility.DestsStringToDests<Libs.Types.Config.Props.Dests.To>(Libs.DataHolder.Instance.GetUnsendMonthly().TO);
                    ccs = Libs.FrontendUtility.DestsStringToDests<Libs.Types.Config.Props.Dests.Cc>(Libs.DataHolder.Instance.GetUnsendMonthly().CC);
                    bccs = Libs.FrontendUtility.DestsStringToDests<Libs.Types.Config.Props.Dests.Bcc>(Libs.DataHolder.Instance.GetUnsendMonthly().BCC);
                    subject = Libs.DataHolder.Instance.GetUnsendMonthly().SUBJECT.ToString();
                    head = Libs.DataHolder.Instance.GetUnsendMonthly().HEAD?.ToString();
                    body = Libs.DataHolder.Instance.GetUnsendMonthly().BODY?.ToString();
                    foot = Libs.DataHolder.Instance.GetUnsendMonthly().FOOT?.ToString();

                    // set selected reports with data holder
                    isSelectedDaily = Libs.DataHolder.Instance.IsSelectedDaily();
                    isSelectedWeekly = Libs.DataHolder.Instance.IsSelectedWeekly();
                    isSelectedMonthly = Libs.DataHolder.Instance.IsSelectedMonthly();
                }

                // set first initialize monthly flag false
                Libs.DataHolder.Instance.SetFirstInitializeMonthly(false);
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

        /// <summary>
        /// OnAfterRenderAsync
        /// </summary>
        protected override async Task OnAfterRenderAsync(bool _)
        {
            // start log
            Libs.Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                if (Libs.DataHolder.Instance.GetUnsendMonthlyFilePaths() != null && Libs.DataHolder.Instance.GetUnsendMonthlyFilePaths().Count != 0)
                {
                    // if attached file selected, decrare var for monthly file names
                    var monthlyFileNames = new List<string>();

                    foreach (var monthlyFilePath in Libs.DataHolder.Instance.GetUnsendMonthlyFilePaths())
                    {
                        // add monthly file name to var
                        monthlyFileNames.Add(System.IO.Path.GetFileName(System.IO.Path.GetFileName(monthlyFilePath)));
                    }

                    // change file drop zone texts
                    await JSRuntime.InvokeVoidAsync("changeFileDropZoneText", monthlyFileNames);
                }
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

        /// <summary>
        /// FileChanged
        /// </summary>
        /// <param name="e">event</param>
        private async Task FileChanged(InputFileChangeEventArgs e)
        {
            // start log
            Libs.Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // set monthly files
                monthlyFiles = e.GetMultipleFiles().ToList();

                // cache attached monthly files
                var cachedMonthlyFilePaths = await Libs.FrontendUtility.CacheAttachedFiles(monthlyFiles, Libs.DataHolder.Instance.GetPreferences().Preference.AddDateToAttachedFile);

                // delete cached monthly files
                Libs.FrontendUtility.DeleteCachedFiles(Libs.DataHolder.Instance.GetUnsendMonthlyFilePaths());

                // set cached monthly file paths
                Libs.DataHolder.Instance.SetUnsendMonthlyFilePaths(cachedMonthlyFilePaths);
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

        /// <summary>
        /// Dispose
        /// </summary>
        public async void Dispose()
        {
            // start log
            Libs.Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // set unsend monthly
                Libs.DataHolder.Instance.SetUnsendMonthly(new Libs.Database.Models.MONTHLY()
                {
                    FROM = $"{Libs.DataHolder.Instance.GetPreferences().User.LastName} {Libs.DataHolder.Instance.GetPreferences().User.FirstName}/{Libs.DataHolder.Instance.GetCredential().Account.Mailaddress}",
                    TO = Libs.FrontendUtility.DestsToDestsString(tos.Select(to => (Libs.Types.Config.Props.Dests.Interfaces.IDest)to).ToList()),
                    CC = Libs.FrontendUtility.DestsToDestsString(ccs.Select(cc => (Libs.Types.Config.Props.Dests.Interfaces.IDest)cc).ToList()),
                    BCC = Libs.FrontendUtility.DestsToDestsString(bccs.Select(bcc => (Libs.Types.Config.Props.Dests.Interfaces.IDest)bcc).ToList()),
                    SUBJECT = subject,
                    HEAD = head,
                    BODY = body,
                    FOOT = foot
                });

                // set selected reports
                Libs.DataHolder.Instance.SetSelectedDaily(isSelectedDaily);
                Libs.DataHolder.Instance.SetSelectedWeekly(isSelectedWeekly);
                Libs.DataHolder.Instance.SetSelectedMonthly(isSelectedMonthly);
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

        #region private methods

        /// <summary>
        /// ClearFiles
        /// </summary>
        private async Task ClearFiles()
        {
            // start log
            Libs.Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // clear monthly files
                monthlyFiles.Clear();

                if (Libs.DataHolder.Instance.GetUnsendMonthlyFilePaths() != null || Libs.DataHolder.Instance.GetUnsendMonthlyFilePaths().Count != 0)
                {
                    // if attached file selected, delete cached monthly files
                    Libs.FrontendUtility.DeleteCachedFiles(Libs.DataHolder.Instance.GetUnsendMonthlyFilePaths());

                    // clear monthly files path
                    Libs.DataHolder.Instance.GetUnsendMonthlyFilePaths().Clear();
                }

                // clear file drop zone texts
                await JSRuntime.InvokeVoidAsync("clearFileDropZoneText");
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

        /// <summary>
        /// Send
        /// </summary>
        private async Task Send()
        {
            // start log
            Libs.Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // check selected report toggles
                if (!isSelectedDaily && !isSelectedWeekly && !isSelectedMonthly)
                {
                    // if not selected, show dialog
                    await Libs.Services.DialogService.Instance.ShowAlertAsync(Resources.Text.Components.DialogCaptionWarning, Resources.Text.Messages.WarningNoneOfReportSelected);

                    // warn log
                    Libs.Logger.Instance.TraceWarning(Resources.Text.LogMessages.FailureNoneOfReportSelected);

                    // end method
                    return;
                }

                // if selected daily, check is prepared daily
                if (isSelectedDaily && !Libs.DataHolder.Instance.IsPreparedDaily())
                {
                    // if not prepared, show dialog
                    await Libs.Services.DialogService.Instance.ShowAlertAsync(Resources.Text.Components.DialogCaptionWarning, Resources.Text.Messages.WarningDailyNotPrepared);

                    // warn log
                    Libs.Logger.Instance.TraceWarning(Resources.Text.LogMessages.FailureDailyNotPrepared);

                    // end method
                    return;
                }

                // if selected weekly, check weekly files selected
                if (isSelectedWeekly && (Libs.DataHolder.Instance.GetUnsendWeeklyFilePaths() == null || Libs.DataHolder.Instance.GetUnsendWeeklyFilePaths().Count == 0))
                {
                    // if not selected, show dialog
                    await Libs.Services.DialogService.Instance.ShowAlertAsync(Resources.Text.Components.DialogCaptionWarning, Resources.Text.Messages.WarningNoneOfWeeklyFilesSelected);

                    // warn log
                    Libs.Logger.Instance.TraceWarning(Resources.Text.LogMessages.FailureNoneOfWeeklyFilesSelected);

                    // end method
                    return;
                }

                // if selected monthly, check monthly files selected
                if (isSelectedMonthly && (Libs.DataHolder.Instance.GetUnsendMonthlyFilePaths() == null || Libs.DataHolder.Instance.GetUnsendMonthlyFilePaths().Count == 0))
                {
                    // if not selected, show dialog
                    await Libs.Services.DialogService.Instance.ShowAlertAsync(Resources.Text.Components.DialogCaptionWarning, Resources.Text.Messages.WarningNoneOfMonthlyFilesSelected);

                    // warn log
                    Libs.Logger.Instance.TraceWarning(Resources.Text.LogMessages.FailureNoneOfMonthlyFilesSelected);

                    // end method
                    return;
                }

                // if config enabled, show confirm dialog
                if (Libs.DataHolder.Instance.GetPreferences().Preference.ConfirmBeforeSend)
                {
                    // build message for confirm dialog
                    var sendingReportSubjects = Libs.FrontendUtility.BuildConfirmDialogMessage
                        (
                            isSelectedDaily ? Libs.DataHolder.Instance.GetUnsendDaily().SUBJECT : null,
                            isSelectedWeekly ? Libs.DataHolder.Instance.GetUnsendWeekly().SUBJECT : null,
                            isSelectedMonthly ? subject : null
                        );

                    // declare var for confirm dialog message
                    string confirmMessage;

                    if (isSelectedDaily && Libs.DataHolder.Instance.GetUnsendDaily().DAILY_TYPE == (int)Libs.Types.Enums.DailyType.Closing)
                    {
                        // if selected daily and is closing, format with time
                        confirmMessage = string.Format(Resources.Text.Messages.ConfirmSendingMessageWithClosing, sendingReportSubjects, Libs.DataHolder.Instance.GetUnsendDaily().TIME);
                    }
                    else
                    {
                        // format without time
                        confirmMessage = string.Format(Resources.Text.Messages.ConfirmSendingMessage, sendingReportSubjects);
                    }

                    // show dialog
                    var confirmResult = await Libs.Services.DialogService.Instance.ShowConfirmationAsync(Resources.Text.Components.DialogCaptionConfirm, confirmMessage);

                    if (!confirmResult)
                    {
                        // if no selected in dialog, info log
                        Libs.Logger.Instance.TraceInformation(Resources.Text.LogMessages.CancelSending);

                        // end method
                        return;
                    }
                }

                // show modal
                await JSRuntime.InvokeVoidAsync("showModal");

                // re-get reports
                var daily = Libs.DataHolder.Instance.GetUnsendDaily();
                var weekly = Libs.DataHolder.Instance.GetUnsendWeekly();
                var weeklyFilePaths = Libs.DataHolder.Instance.GetUnsendWeeklyFilePaths();
                Libs.Database.Models.MONTHLY monthly = null;
                var monthlyFilePaths = Libs.DataHolder.Instance.GetUnsendMonthlyFilePaths();

                // declare var for messages
                var messages = new Dictionary<Libs.Database.Models.Interfaces.IReport, List<string>>();

                if (isSelectedDaily)
                {
                    // if daily is selected, replace newline character
                    daily.SUBJECT = string.IsNullOrEmpty(daily.SUBJECT) ? null : Regex.Replace(daily.SUBJECT, @"\r\n?|\n", "\r\n");
                    daily.HEAD = string.IsNullOrEmpty(daily.HEAD) ? null : Regex.Replace(daily.HEAD, @"\r\n?|\n", "\r\n");
                    daily.TIME = string.IsNullOrEmpty(daily.TIME) ? null : Regex.Replace(daily.TIME, @"\r\n?|\n", "\r\n");
                    daily.BODY = string.IsNullOrEmpty(daily.BODY) ? null : Regex.Replace(daily.BODY, @"\r\n?|\n", "\r\n");
                    daily.FOOT = string.IsNullOrEmpty(daily.FOOT) ? null : Regex.Replace(daily.FOOT, @"\r\n?|\n", "\r\n");

                    // add list
                    messages.Add(daily, null);
                }

                if (isSelectedWeekly)
                {
                    // if weekly is selected, replace newline character
                    weekly.SUBJECT = string.IsNullOrEmpty(weekly.SUBJECT) ? null : Regex.Replace(weekly.SUBJECT, @"\r\n?|\n", "\r\n");
                    weekly.HEAD = string.IsNullOrEmpty(weekly.HEAD) ? null : Regex.Replace(weekly.HEAD, @"\r\n?|\n", "\r\n");
                    weekly.BODY = string.IsNullOrEmpty(weekly.BODY) ? null : Regex.Replace(weekly.BODY, @"\r\n?|\n", "\r\n");
                    weekly.FOOT = string.IsNullOrEmpty(weekly.FOOT) ? null : Regex.Replace(weekly.FOOT, @"\r\n?|\n", "\r\n");

                    // add list
                    messages.Add(weekly, weeklyFilePaths);
                }

                if (isSelectedMonthly)
                {
                    // if monthly is selected, re-create monthly add list
                    monthly = new Libs.Database.Models.MONTHLY()
                    {
                        FROM = $"{Libs.DataHolder.Instance.GetPreferences().User.LastName} {Libs.DataHolder.Instance.GetPreferences().User.FirstName}/{Libs.DataHolder.Instance.GetCredential().Account.Mailaddress}",
                        TO = Libs.FrontendUtility.DestsToDestsString(tos.Select(to => (Libs.Types.Config.Props.Dests.Interfaces.IDest)to).ToList()),
                        CC = Libs.FrontendUtility.DestsToDestsString(ccs.Select(cc => (Libs.Types.Config.Props.Dests.Interfaces.IDest)cc).ToList()),
                        BCC = Libs.FrontendUtility.DestsToDestsString(bccs.Select(bcc => (Libs.Types.Config.Props.Dests.Interfaces.IDest)bcc).ToList()),
                        SUBJECT = string.IsNullOrEmpty(subject) ? null : Regex.Replace(subject, @"\r\n?|\n", "\r\n"),
                        HEAD = string.IsNullOrEmpty(head) ? null : Regex.Replace(head, @"\r\n?|\n", "\r\n"),
                        BODY = string.IsNullOrEmpty(body) ? null : Regex.Replace(body, @"\r\n?|\n", "\r\n"),
                        FOOT = string.IsNullOrEmpty(foot) ? null : Regex.Replace(foot, @"\r\n?|\n", "\r\n")
                    };

                    messages.Add(monthly, monthlyFilePaths);
                }

                // send mime messages
                var resultList = Task.Run(async () => await Libs.Mailer.Send(messages, Libs.DataHolder.Instance.GetCredential(), Libs.DataHolder.Instance.GetPreferences())).Result;

                // on a per-mime-message basis 
                // set result(send_time and is_succeeded)
                foreach (var result in resultList)
                {
                    if (result.Key is Libs.Database.Models.DAILY)
                    {
                        // if daily result, set daily props
                        daily.SEND_TIME = result.Value.Item1;
                        daily.IS_SUCCEEDED = result.Value.Item2;
                    }

                    if (result.Key is Libs.Database.Models.WEEKLY)
                    {
                        // if weekly result, set weekly props
                        weekly.SEND_TIME = result.Value.Item1;
                        weekly.IS_SUCCEEDED = result.Value.Item2;
                    }

                    if (result.Key is Libs.Database.Models.MONTHLY)
                    {
                        // if monthly result, set monthly props
                        monthly.SEND_TIME = result.Value.Item1;
                        monthly.IS_SUCCEEDED = result.Value.Item2;
                    }
                }

                // declare var for saving reports
                var reports = new Dictionary<Libs.Database.Models.Interfaces.IReport,
                    Tuple<List<Libs.Types.Config.Props.Dests.To>, List<Libs.Types.Config.Props.Dests.Cc>, List<Libs.Types.Config.Props.Dests.Bcc>, List<string>>>();

                // add reports
                foreach (var result in resultList)
                {
                    if (result.Key is Libs.Database.Models.DAILY)
                    {
                        reports.Add
                            (
                                daily,
                                new Tuple<List<Libs.Types.Config.Props.Dests.To>, List<Libs.Types.Config.Props.Dests.Cc>, List<Libs.Types.Config.Props.Dests.Bcc>, List<string>>
                                (
                                    Libs.FrontendUtility.DestsStringToDests<Libs.Types.Config.Props.Dests.To>(daily.TO),
                                    Libs.FrontendUtility.DestsStringToDests<Libs.Types.Config.Props.Dests.Cc>(daily.CC),
                                    Libs.FrontendUtility.DestsStringToDests<Libs.Types.Config.Props.Dests.Bcc>(daily.BCC),
                                    null
                                )
                            );
                    }

                    if (result.Key is Libs.Database.Models.WEEKLY)
                    {
                        reports.Add
                            (
                                weekly,
                                new Tuple<List<Libs.Types.Config.Props.Dests.To>, List<Libs.Types.Config.Props.Dests.Cc>, List<Libs.Types.Config.Props.Dests.Bcc>, List<string>>
                                (
                                    Libs.FrontendUtility.DestsStringToDests<Libs.Types.Config.Props.Dests.To>(weekly.TO),
                                    Libs.FrontendUtility.DestsStringToDests<Libs.Types.Config.Props.Dests.Cc>(weekly.CC),
                                    Libs.FrontendUtility.DestsStringToDests<Libs.Types.Config.Props.Dests.Bcc>(weekly.BCC),
                                    weeklyFilePaths
                                )
                            );
                    }

                    if (result.Key is Libs.Database.Models.MONTHLY)
                    {
                        reports.Add
                            (
                                monthly,
                                new Tuple<List<Libs.Types.Config.Props.Dests.To>, List<Libs.Types.Config.Props.Dests.Cc>, List<Libs.Types.Config.Props.Dests.Bcc>, List<string>>
                                (
                                    Libs.FrontendUtility.DestsStringToDests<Libs.Types.Config.Props.Dests.To>(monthly.TO),
                                    Libs.FrontendUtility.DestsStringToDests<Libs.Types.Config.Props.Dests.Cc>(monthly.CC),
                                    Libs.FrontendUtility.DestsStringToDests<Libs.Types.Config.Props.Dests.Bcc>(monthly.BCC),
                                    monthlyFilePaths
                                )
                            );
                    }
                }

                // save files
                Libs.FrontendUtility.SaveFiles(reports, Libs.DataHolder.Instance.GetPreferences().Preference.SaveTextFile, Libs.DataHolder.Instance.GetPreferences().Preference.SaveDestination);

                // build message for result dialog
                var resultDialogMessage = Libs.FrontendUtility.BuildResultDialogMessage(resultList);

                // hide modal
                await JSRuntime.InvokeVoidAsync("hideModal");

                // show result dialog
                await Libs.Services.DialogService.Instance.ShowAlertAsync(Resources.Text.Components.DialogCaptionResult, resultDialogMessage);

                // end log
                Libs.Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);

                if (!resultList.Any(result => result.Value.Item2 == 0) || Libs.DataHolder.Instance.GetPreferences().Preference.ExitAfterSend)
                {
                    // if not any failure and config enabled, rep end log
                    Libs.Logger.Instance.TraceInformation(Resources.Text.Components.TextLogEndRep);

                    // end rep
                    Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                // show error dialog
                await Libs.Services.DialogService.Instance.ShowAlertAsync(Resources.Text.Components.DialogCaptionFailure, Resources.Text.Messages.FailureSomethingWrongOccured);

                // fatal log
                Libs.Logger.Instance.TraceFatal(Resources.Text.Components.TextLogTagFrontendFatal, Resources.Text.LogMessages.FailureSendingReport, ex);
            }
            finally
            {
                // hide modal (for the case exception occured before result dialog showed)
                await JSRuntime.InvokeVoidAsync("hideModal");
            }
        }

        /// <summary>
        /// remove from tos
        /// </summary>
        /// <param name="to">to</param>
        private async Task RemoveFromTos(Libs.Types.Config.Props.Dests.To to)
        {
            // start log
            Libs.Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                tos.Remove(to);
            }
            catch (Exception ex)
            {
                // if exception occured, show dialog
                await Libs.Services.DialogService.Instance.ShowAlertAsync(Resources.Text.Components.DialogCaptionFailure, Resources.Text.Messages.FailureSomethingWrongOccured);

                // fatal log
                Libs.Logger.Instance.TraceFatal(Resources.Text.Components.TextLogTagFrontendFatal, Resources.Text.LogMessages.FailureSomethingWrongOccured, ex);
            }
            finally
            {
                // end log
                Libs.Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// add to
        /// </summary>
        private async Task AddTo()
        {
            // start log
            Libs.Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                tos.Add(new Libs.Types.Config.Props.Dests.To());
            }
            catch (Exception ex)
            {
                // if exception occured, show dialog
                await Libs.Services.DialogService.Instance.ShowAlertAsync(Resources.Text.Components.DialogCaptionFailure, Resources.Text.Messages.FailureSomethingWrongOccured);

                // fatal log
                Libs.Logger.Instance.TraceFatal(Resources.Text.Components.TextLogTagFrontendFatal, Resources.Text.LogMessages.FailureSomethingWrongOccured, ex);
            }
            finally
            {
                // end log
                Libs.Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// remove from ccs
        /// </summary>
        /// <param name="cc">cc</param>
        private async Task RemoveFromCcs(Libs.Types.Config.Props.Dests.Cc cc)
        {
            // start log
            Libs.Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                ccs.Remove(cc);
            }
            catch (Exception ex)
            {
                // if exception occured, show dialog
                await Libs.Services.DialogService.Instance.ShowAlertAsync(Resources.Text.Components.DialogCaptionFailure, Resources.Text.Messages.FailureSomethingWrongOccured);

                // fatal log
                Libs.Logger.Instance.TraceFatal(Resources.Text.Components.TextLogTagFrontendFatal, Resources.Text.LogMessages.FailureSomethingWrongOccured, ex);
            }
            finally
            {
                // end log
                Libs.Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// add cc
        /// </summary>
        private async Task AddCc()
        {
            // start log
            Libs.Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                ccs.Add(new Libs.Types.Config.Props.Dests.Cc());
            }
            catch (Exception ex)
            {
                // if exception occured, show dialog
                await Libs.Services.DialogService.Instance.ShowAlertAsync(Resources.Text.Components.DialogCaptionFailure, Resources.Text.Messages.FailureSomethingWrongOccured);

                // fatal log
                Libs.Logger.Instance.TraceFatal(Resources.Text.Components.TextLogTagFrontendFatal, Resources.Text.LogMessages.FailureSomethingWrongOccured, ex);
            }
            finally
            {
                // end log
                Libs.Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// remove from bccs
        /// </summary>
        /// <param name="bcc">bcc</param>
        private async Task RemoveFromBccs(Libs.Types.Config.Props.Dests.Bcc bcc)
        {
            // start log
            Libs.Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                bccs.Remove(bcc);
            }
            catch (Exception ex)
            {
                // if exception occured, show dialog
                await Libs.Services.DialogService.Instance.ShowAlertAsync(Resources.Text.Components.DialogCaptionFailure, Resources.Text.Messages.FailureSomethingWrongOccured);

                // fatal log
                Libs.Logger.Instance.TraceFatal(Resources.Text.Components.TextLogTagFrontendFatal, Resources.Text.LogMessages.FailureSomethingWrongOccured, ex);
            }
            finally
            {
                // end log
                Libs.Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// add bcc
        /// </summary>
        private async Task AddBcc()
        {
            // start log
            Libs.Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                bccs.Add(new Libs.Types.Config.Props.Dests.Bcc());
            }
            catch (Exception ex)
            {
                // if exception occured, show dialog
                await Libs.Services.DialogService.Instance.ShowAlertAsync(Resources.Text.Components.DialogCaptionFailure, Resources.Text.Messages.FailureSomethingWrongOccured);

                // fatal log
                Libs.Logger.Instance.TraceFatal(Resources.Text.Components.TextLogTagFrontendFatal, Resources.Text.LogMessages.FailureSomethingWrongOccured, ex);
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
