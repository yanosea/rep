using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace rep.Pages.config
{
    /// <summary>
    /// Monthly
    /// </summary>
    public partial class Monthly
    {
        #region props

        /// <summary>
        /// monthlySubjectTemplate
        /// </summary>
        private string monthlySubjectTemplate { get; set; }

        /// <summary>
        /// monthlyHeadTemplate
        /// </summary>
        private string monthlyHeadTemplate { get; set; }

        /// <summary>
        /// monthlyFootTemplate
        /// </summary>
        private string monthlyFootTemplate { get; set; }

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
                // get monthly config
                var monthlyConfig = Libs.DataHolder.Instance.GetMonthlyConfig();
                monthlySubjectTemplate = monthlyConfig.MonthlyTemplates.MonthlySubjectTemplate;
                monthlyHeadTemplate = monthlyConfig.MonthlyTemplates.MonthlyHeadTemplate;
                monthlyFootTemplate = monthlyConfig.MonthlyTemplates.MonthlyFootTemplate;

                // get monthly dest config
                var monthlyDestConfig = Libs.DataHolder.Instance.GetMonthlyDestConfig();
                tos = monthlyDestConfig.To;
                ccs = monthlyDestConfig.Cc;
                bccs = monthlyDestConfig.Bcc;
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
        /// SaveMonthly
        /// </summary>
        private async Task SaveMonthly()
        {
            // start log
            Libs.Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // create new monthly config object
                var monthlyConfig = new Libs.Types.Config.Reports.MonthlyConfig()
                {
                    MonthlyTemplates = new Libs.Types.Config.Props.Reports.MonthlyConfig.MonthlyTemplates()
                    {
                        MonthlySubjectTemplate = monthlySubjectTemplate,
                        MonthlyHeadTemplate = monthlyHeadTemplate,
                        MonthlyFootTemplate = monthlyFootTemplate
                    }
                };

                // create new monthly dest config object
                var monthlyDestConfig = new Libs.Types.Config.Dests.MonthlyDestConfig()
                {
                    To = tos,
                    Cc = ccs,
                    Bcc = bccs
                };

                // write monthly_conf.toml with new monthly config object
                Libs.IO.ConfigReadWriter.WriteMonthlyConfig(monthlyConfig);

                // write monthly_dest_conf.toml with new monthly dest config object
                Libs.IO.ConfigReadWriter.WriteMonthlyDestConfig(monthlyDestConfig);

                // re-set new monthly config to dataholder
                Libs.DataHolder.Instance.SetMonthlyConfig(monthlyConfig);

                // re-set new monthly dest config to dataholder
                Libs.DataHolder.Instance.SetMonthlyDestConfig(monthlyDestConfig);

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
