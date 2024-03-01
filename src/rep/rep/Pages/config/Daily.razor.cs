using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace rep.Pages.config
{
    /// <summary>
    /// Daily
    /// </summary>
    public partial class Daily
    {
        #region props

        /// <summary>
        /// thresholdTime
        /// </summary>
        private DateTime? thresholdTime { get; set; }

        /// <summary>
        /// intervalMinutes
        /// </summary>
        private int intervalMinutes { get; set; }

        /// <summary>
        /// timeFormat
        /// </summary>
        private string timeFormat { get; set; }

        /// <summary>
        /// openingSubjectTemplate
        /// </summary>
        private string openingSubjectTemplate { get; set; }

        /// <summary>
        /// openingHeadTemplate
        /// </summary>
        private string openingHeadTemplate { get; set; }

        /// <summary>
        /// openingFootTemplate
        /// </summary>
        private string openingFootTemplate { get; set; }

        /// <summary>
        /// closingSubjectTemplate
        /// </summary>
        private string closingSubjectTemplate { get; set; }

        /// <summary>
        /// closingSubjectTemplate
        /// </summary>
        private string closingHeadTemplate { get; set; }

        /// <summary>
        /// closingFootTemplate
        /// </summary>
        private string closingFootTemplate { get; set; }

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
                thresholdTime = DateTime.TryParse(dailyConfig.Preferences.ThresholdTime, out DateTime parsedTime) ? parsedTime : DateTime.Parse("14:00");
                intervalMinutes = dailyConfig.Preferences.IntervalMinutes;
                timeFormat = dailyConfig.Preferences.TimeFormat;
                openingSubjectTemplate = dailyConfig.DailyTemplates.OpeningSubjectTemplate;
                openingHeadTemplate = dailyConfig.DailyTemplates.OpeningHeadTemplate;
                openingFootTemplate = dailyConfig.DailyTemplates.OpeningFootTemplate;
                closingSubjectTemplate = dailyConfig.DailyTemplates.ClosingSubjectTemplate;
                closingHeadTemplate = dailyConfig.DailyTemplates.ClosingHeadTemplate;
                closingFootTemplate = dailyConfig.DailyTemplates.ClosingFootTemplate;

                // get daily dest config
                var dailyDestConfig = Libs.DataHolder.Instance.GetDailyDestConfig();
                tos = dailyDestConfig.To;
                ccs = dailyDestConfig.Cc;
                bccs = dailyDestConfig.Bcc;

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
        /// SaveDaily
        /// </summary>
        private async Task SaveDaily()
        {
            // start log
            Libs.Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // create new daily config object
                var dailyConfig = new Libs.Types.Config.Reports.DailyConfig()
                {
                    Preferences = new Libs.Types.Config.Props.Reports.DailyConfig.Preference()
                    {
                        ThresholdTime = thresholdTime.HasValue ? thresholdTime.Value.ToString("HH:mm") : "14:00",
                        IntervalMinutes = intervalMinutes,
                        TimeFormat = timeFormat
                    },
                    DailyTemplates = new Libs.Types.Config.Props.Reports.DailyConfig.DailyTemplates()
                    {
                        OpeningSubjectTemplate = openingSubjectTemplate,
                        OpeningHeadTemplate = openingHeadTemplate,
                        OpeningFootTemplate = openingFootTemplate,
                        ClosingSubjectTemplate = closingSubjectTemplate,
                        ClosingHeadTemplate = closingHeadTemplate,
                        ClosingFootTemplate = closingFootTemplate
                    }
                };

                // create new daily dest config object
                var dailyDestConfig = new Libs.Types.Config.Dests.DailyDestConfig()
                {
                    To = tos,
                    Cc = ccs,
                    Bcc = bccs
                };

                // write daily_conf.toml with new daily config object
                Libs.IO.ConfigReadWriter.WriteDailyConfig(dailyConfig);

                // write daily_dest_conf.toml with new daily dest config object
                Libs.IO.ConfigReadWriter.WriteDailyDestConfig(dailyDestConfig);

                // re-set new daily config to dataholder
                Libs.DataHolder.Instance.SetDailyConfig(dailyConfig);

                // re-set new daily dest config to dataholder
                Libs.DataHolder.Instance.SetDailyDestConfig(dailyDestConfig);

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
