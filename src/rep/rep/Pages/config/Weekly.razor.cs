using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace rep.Pages.config
{
    /// <summary>
    /// Weekly
    /// </summary>
    public partial class Weekly
    {
        #region props

        /// <summary>
        /// weeklySubjectTemplate
        /// </summary>
        private string weeklySubjectTemplate { get; set; }

        /// <summary>
        /// weeklyHeadTemplate
        /// </summary>
        private string weeklyHeadTemplate { get; set; }

        /// <summary>
        /// weeklyFootTemplate
        /// </summary>
        private string weeklyFootTemplate { get; set; }

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
                // get weekly config
                var weeklyConfig = Libs.DataHolder.Instance.GetWeeklyConfig();
                weeklySubjectTemplate = weeklyConfig.WeeklyTemplates.WeeklySubjectTemplate;
                weeklyHeadTemplate = weeklyConfig.WeeklyTemplates.WeeklyHeadTemplate;
                weeklyFootTemplate = weeklyConfig.WeeklyTemplates.WeeklyFootTemplate;

                // get weekly dest config
                var weeklyDestConfig = Libs.DataHolder.Instance.GetWeeklyDestConfig();
                tos = weeklyDestConfig.To;
                ccs = weeklyDestConfig.Cc;
                bccs = weeklyDestConfig.Bcc;
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
        /// SaveWeekly
        /// </summary>
        private async Task SaveWeekly()
        {
            // start log
            Libs.Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // create new weekly config object
                var weeklyConfig = new Libs.Types.Config.Reports.WeeklyConfig()
                {
                    WeeklyTemplates = new Libs.Types.Config.Props.Reports.WeeklyConfig.WeeklyTemplates()
                    {
                        WeeklySubjectTemplate = weeklySubjectTemplate,
                        WeeklyHeadTemplate = weeklyHeadTemplate,
                        WeeklyFootTemplate = weeklyFootTemplate
                    }
                };

                // create new weekly dest config object
                var weeklyDestConfig = new Libs.Types.Config.Dests.WeeklyDestConfig()
                {
                    To = tos,
                    Cc = ccs,
                    Bcc = bccs
                };

                // write weekly_conf.toml with new weekly config object
                Libs.IO.ConfigReadWriter.WriteWeeklyConfig(weeklyConfig);

                // write weekly_dest_conf.toml with new weekly dest config object
                Libs.IO.ConfigReadWriter.WriteWeeklyDestConfig(weeklyDestConfig);

                // re-set new weekly config to dataholder
                Libs.DataHolder.Instance.SetWeeklyConfig(weeklyConfig);

                // re-set new weekly dest config to dataholder
                Libs.DataHolder.Instance.SetWeeklyDestConfig(weeklyDestConfig);

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
