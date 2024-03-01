using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace rep.Pages.read
{
    /// <summary>
    /// Daily
    /// </summary>
    public partial class Daily : IDisposable
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
        /// isOpening
        /// </summary>
        private bool isOpening { get; set; }

        /// <summary>
        /// subject
        /// </summary>
        private string subject { get; set; }

        /// <summary>
        /// head
        /// </summary>
        private string head { get; set; }

        /// <summary>
        /// time
        /// </summary>
        private string time { get; set; }

        /// <summary>
        /// body
        /// </summary>
        private string body { get; set; }

        /// <summary>
        /// foot
        /// </summary>
        private string foot { get; set; }

        /// <summary>
        /// from
        /// </summary>
        private DateTime? from { get; set; }

        /// <summary>
        /// to
        /// </summary>
        private DateTime? to { get; set; }

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

        /// <summary>
        /// isPreparedeDaily
        /// </summary>
        private bool isPreparedeDaily = false;

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
                // get daily config
                var dailyConfig = Libs.DataHolder.Instance.GetDailyConfig();

                // set is opening
                if (string.IsNullOrEmpty(dailyConfig.Preferences.ThresholdTime))
                {
                    // if threshold time is unset, force opening
                    isOpening = true;

                    // set prepared daily flag true
                    isPreparedeDaily = true;

                    // show dialog
                    await Libs.Services.DialogService.Instance.ShowAlertAsync(Resources.Text.Components.DialogCaptionWarning, Resources.Text.Messages.FailureGettingThresholdTime);

                    // warn log
                    Libs.Logger.Instance.TraceWarning(Resources.Text.LogMessages.FailureGettingThresholdTime);
                }
                else
                {
                    // declaring vars for parsed time
                    DateTime thresholdTime;
                    if (!DateTime.TryParse(dailyConfig.Preferences.ThresholdTime, out thresholdTime))
                    {
                        // if failed to parse,  force opening
                        isOpening = true;

                        // set prepared daily flag true
                        isPreparedeDaily = true;

                        // throw abnormal input exception
                        throw new Libs.Exceptions.AbnormalInputException(nameof(thresholdTime), new Libs.Exceptions.UnexpectedException());
                    }

                    // set is opening flag
                    isOpening = Int32.Parse(DateTime.Now.ToString("HHmm")) <= Int32.Parse(DateTime.Parse(dailyConfig.Preferences.ThresholdTime).ToString("HHmm"));

                    // set isPrepared daily flag
                    isPreparedeDaily = isOpening ? true : false;
                }

                if (Libs.DataHolder.Instance.IsFirstInitializeDaily())
                {
                    // if first initialize daily, get daily dest config
                    var dailyDestConfig = Libs.DataHolder.Instance.GetDailyDestConfig();
                    tos = dailyDestConfig.To;
                    ccs = dailyDestConfig.Cc;
                    bccs = dailyDestConfig.Bcc;

                    // binding params
                    subject = Libs.FrontendUtility.BindParams(isOpening ? dailyConfig.DailyTemplates.OpeningSubjectTemplate : dailyConfig.DailyTemplates.ClosingSubjectTemplate);
                    head = Libs.FrontendUtility.BindParams(isOpening ? dailyConfig.DailyTemplates.OpeningHeadTemplate : dailyConfig.DailyTemplates.ClosingHeadTemplate);
                    body = isOpening ? Libs.Database.IO.DBReadWriter.GetLastOpening().BODY : Libs.Database.IO.DBReadWriter.GetLastClosing().BODY;
                    foot = Libs.FrontendUtility.BindParams(isOpening ? dailyConfig.DailyTemplates.OpeningFootTemplate : dailyConfig.DailyTemplates.ClosingFootTemplate);

                    if (Libs.DataHolder.Instance.IsFirstInitializeWrite())
                    {
                        // if first initialize write , set selected reports with command line arguments
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

                    if (!isOpening)
                    {
                        // get last opening time
                        var lastOpeningTime = Libs.Database.IO.DBReadWriter.GetLastOpening().SEND_TIME;

                        // get interval minutes
                        var intervalMinute = TimeSpan.FromMinutes(Libs.DataHolder.Instance.GetDailyConfig().Preferences.IntervalMinutes);

                        // get current time
                        var currentTime = DateTime.Now;

                        if (lastOpeningTime.CompareTo(DateTime.Now.AddDays(-1)) == -1)
                        {
                            // if the last opening was sent before previous day, set from 00:00
                            from = DateTime.Parse("00:00");

                            // set to
                            to = new DateTime((((currentTime.Ticks + intervalMinute.Ticks) / intervalMinute.Ticks) - 1) * intervalMinute.Ticks, currentTime.Kind);

                            // show dialog
                            await Libs.Services.DialogService.Instance.ShowAlertAsync(Resources.Text.Components.DialogCaptionWarning, Resources.Text.Messages.FailureGettingTodaysOpeningTime);

                            // warn log
                            Libs.Logger.Instance.TraceWarning(Resources.Text.LogMessages.FailureGettingTodaysOpeningTime);
                        }
                        else
                        {
                            // set from
                            from = new DateTime(((lastOpeningTime.Ticks + intervalMinute.Ticks - 1) / intervalMinute.Ticks) * intervalMinute.Ticks, lastOpeningTime.Kind);

                            // set to
                            to = new DateTime((((currentTime.Ticks + intervalMinute.Ticks) / intervalMinute.Ticks) - 1) * intervalMinute.Ticks, currentTime.Kind);
                        }

                        // set prepared daily flag true
                        isPreparedeDaily = isOpening ? true : false;
                    }
                }
                else
                {
                    // if not first initialize daily, get unsend daily and set input form
                    from = Libs.DataHolder.Instance.GetUnsendDaily().OPEN_TIME;
                    to = Libs.DataHolder.Instance.GetUnsendDaily().CLOSE_TIME;
                    tos = Libs.FrontendUtility.DestsStringToDests<Libs.Types.Config.Props.Dests.To>(Libs.DataHolder.Instance.GetUnsendDaily().TO);
                    ccs = Libs.FrontendUtility.DestsStringToDests<Libs.Types.Config.Props.Dests.Cc>(Libs.DataHolder.Instance.GetUnsendDaily().CC);
                    bccs = Libs.FrontendUtility.DestsStringToDests<Libs.Types.Config.Props.Dests.Bcc>(Libs.DataHolder.Instance.GetUnsendDaily().BCC);
                    subject = Libs.DataHolder.Instance.GetUnsendDaily().SUBJECT.ToString();
                    head = Libs.DataHolder.Instance.GetUnsendDaily().HEAD?.ToString();
                    time = Libs.DataHolder.Instance.GetUnsendDaily().TIME?.ToString();
                    body = Libs.DataHolder.Instance.GetUnsendDaily().BODY?.ToString();
                    foot = Libs.DataHolder.Instance.GetUnsendDaily().FOOT?.ToString();

                    // set selected reports with data holder
                    isSelectedDaily = Libs.DataHolder.Instance.IsSelectedDaily();
                    isSelectedWeekly = Libs.DataHolder.Instance.IsSelectedWeekly();
                    isSelectedMonthly = Libs.DataHolder.Instance.IsSelectedMonthly();

                    // set prepared daily flag with data holder
                    isPreparedeDaily = Libs.DataHolder.Instance.IsPreparedDaily();
                }

                // set first initialize daily flag false
                Libs.DataHolder.Instance.SetFirstInitializeDaily(false);
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

        /// <summary>
        /// Dispose
        /// </summary>
        public async void Dispose()
        {
            // start log
            Libs.Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                //  set unsend daily
                Libs.DataHolder.Instance.SetUnsendDaily(new Libs.Database.Models.DAILY()
                {
                    DAILY_TYPE = isOpening ? (int)Libs.Types.Enums.DailyType.Opening : (int)Libs.Types.Enums.DailyType.Closing,
                    OPEN_TIME = isOpening ? null : from,
                    CLOSE_TIME = isOpening ? null : to,
                    FROM = $"{Libs.DataHolder.Instance.GetPreferences().User.LastName} {Libs.DataHolder.Instance.GetPreferences().User.FirstName}/{Libs.DataHolder.Instance.GetCredential().Account.Mailaddress}",
                    TO = Libs.FrontendUtility.DestsToDestsString(tos.Select(to => (Libs.Types.Config.Props.Dests.Interfaces.IDest)to).ToList()),
                    CC = Libs.FrontendUtility.DestsToDestsString(ccs.Select(cc => (Libs.Types.Config.Props.Dests.Interfaces.IDest)cc).ToList()),
                    BCC = Libs.FrontendUtility.DestsToDestsString(bccs.Select(bcc => (Libs.Types.Config.Props.Dests.Interfaces.IDest)bcc).ToList()),
                    SUBJECT = subject,
                    HEAD = head,
                    TIME = time,
                    BODY = body,
                    FOOT = foot
                });

                // set selected reports
                Libs.DataHolder.Instance.SetSelectedDaily(isSelectedDaily);
                Libs.DataHolder.Instance.SetSelectedWeekly(isSelectedWeekly);
                Libs.DataHolder.Instance.SetSelectedMonthly(isSelectedMonthly);

                // set prepared daily flag
                Libs.DataHolder.Instance.SetPreparedDaily(isPreparedeDaily);
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
        /// CalcWorkTime
        /// </summary>
        private async Task CalcWorkTime()
        {
            // start log
            Libs.Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // set time
                time = Libs.FrontendUtility.GetWorkTime(from, to, Libs.DataHolder.Instance.GetDailyConfig().Preferences.TimeFormat);

                // set prepared daily flag true
                isPreparedeDaily = true;
            }
            catch (Libs.Exceptions.AbnormalInputException ex)
            {
                // if abnormal input exception occured, show dialog
                await Libs.Services.DialogService.Instance.ShowAlertAsync(Resources.Text.Components.DialogCaptionWarning, ex.FormattedMessage);

                // warn log
                Libs.Logger.Instance.TraceWarning(ex.FormattedMessage);
            }
            catch (OverflowException)
            {
                // if overflow exception occured, show dialog
                await Libs.Services.DialogService.Instance.ShowAlertAsync(Resources.Text.Components.DialogCaptionWarning, string.Format(Resources.Text.Messages.FailureInputtingAbnormalTime));

                // warn log
                Libs.Logger.Instance.TraceWarning(Resources.Text.Messages.FailureInputtingAbnormalTime);
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

                // if selected daily, is not opening, check is prepared daily
                if (isSelectedDaily && !isOpening && !isPreparedeDaily)
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
                if (isSelectedMonthly && (Libs.DataHolder.Instance.GetUnsendWeeklyFilePaths() == null || Libs.DataHolder.Instance.GetUnsendWeeklyFilePaths().Count == 0))
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
                            isSelectedDaily ? subject : null,
                            isSelectedWeekly ? Libs.DataHolder.Instance.GetUnsendWeekly().SUBJECT : null,
                            isSelectedMonthly ? Libs.DataHolder.Instance.GetUnsendMonthly().SUBJECT : null
                        );

                    // declare var for confirm dialog message
                    string confirmMessage;

                    if (isSelectedDaily && !isOpening)
                    {
                        // if selected daily and is closing, format with time
                        confirmMessage = string.Format(Resources.Text.Messages.ConfirmSendingMessageWithClosing, sendingReportSubjects, time);
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
                Libs.Database.Models.DAILY daily = null;
                var weekly = Libs.DataHolder.Instance.GetUnsendWeekly();
                var weeklyFilePaths = Libs.DataHolder.Instance.GetUnsendWeeklyFilePaths();
                var monthly = Libs.DataHolder.Instance.GetUnsendMonthly();
                var monthlyFilePaths = Libs.DataHolder.Instance.GetUnsendMonthlyFilePaths();

                // declare var for messages
                var messages = new Dictionary<Libs.Database.Models.Interfaces.IReport, List<string>>();

                if (isSelectedDaily)
                {
                    // if daily is selected, re-create daily and add list
                    daily = new Libs.Database.Models.DAILY()
                    {
                        DAILY_TYPE = isOpening ? (int)Libs.Types.Enums.DailyType.Opening : (int)Libs.Types.Enums.DailyType.Closing,
                        OPEN_TIME = isOpening ? null : from,
                        CLOSE_TIME = isOpening ? null : to,
                        FROM = $"{Libs.DataHolder.Instance.GetPreferences().User.LastName} {Libs.DataHolder.Instance.GetPreferences().User.FirstName}/{Libs.DataHolder.Instance.GetCredential().Account.Mailaddress}",
                        TO = Libs.FrontendUtility.DestsToDestsString(tos.Select(to => (Libs.Types.Config.Props.Dests.Interfaces.IDest)to).ToList()),
                        CC = Libs.FrontendUtility.DestsToDestsString(ccs.Select(cc => (Libs.Types.Config.Props.Dests.Interfaces.IDest)cc).ToList()),
                        BCC = Libs.FrontendUtility.DestsToDestsString(bccs.Select(bcc => (Libs.Types.Config.Props.Dests.Interfaces.IDest)bcc).ToList()),
                        SUBJECT = string.IsNullOrEmpty(subject) ? null : Regex.Replace(subject, @"\r\n?|\n", "\r\n"),
                        HEAD = string.IsNullOrEmpty(head) ? null : Regex.Replace(head, @"\r\n?|\n", "\r\n"),
                        TIME = string.IsNullOrEmpty(time) ? null : Regex.Replace(time, @"\r\n?|\n", "\r\n"),
                        BODY = string.IsNullOrEmpty(body) ? null : Regex.Replace(body, @"\r\n?|\n", "\r\n"),
                        FOOT = string.IsNullOrEmpty(foot) ? null : Regex.Replace(foot, @"\r\n?|\n", "\r\n")
                    };

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
                    // if monthly is selected, replace newline character
                    monthly.SUBJECT = string.IsNullOrEmpty(monthly.SUBJECT) ? null : Regex.Replace(monthly.SUBJECT, @"\r\n?|\n", "\r\n");
                    monthly.HEAD = string.IsNullOrEmpty(monthly.HEAD) ? null : Regex.Replace(monthly.HEAD, @"\r\n?|\n", "\r\n");
                    monthly.BODY = string.IsNullOrEmpty(monthly.BODY) ? null : Regex.Replace(monthly.BODY, @"\r\n?|\n", "\r\n");
                    monthly.FOOT = string.IsNullOrEmpty(monthly.FOOT) ? null : Regex.Replace(monthly.FOOT, @"\r\n?|\n", "\r\n");

                    // add list
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
