using System;
using System.Threading.Tasks;

namespace rep.Pages.config
{
    /// <summary>
    /// Credential
    /// </summary>
    public partial class Credential
    {
        #region props

        /// <summary>
        /// mailaddress
        /// </summary>
        private string mailaddress { get; set; }

        /// <summary>
        /// password
        /// </summary>
        private string password { get; set; }

        /// <summary>
        /// smtpServer
        /// </summary>
        private string smtpServer { get; set; }

        /// <summary>
        /// smtpPort
        /// </summary>
        private int smtpPort { get; set; }

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
                // get credential config
                var credential = Libs.DataHolder.Instance.GetCredential();
                mailaddress = credential.Account.Mailaddress;
                password = credential.Account.Password;
                smtpServer = credential.Smtp.SmtpServer;
                smtpPort = credential.Smtp.SmtpPort;
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

        #region private methods

        /// <summary>
        /// SaveCredential
        /// </summary>
        private async Task SaveCredential()
        {
            // start log
            Libs.Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // create new credential object
                var credential = new Libs.Types.Config.Credential()
                {
                    Account = new Libs.Types.Config.Props.Credential.Account()
                    {
                        Mailaddress = mailaddress,
                        Password = password
                    },
                    Smtp = new Libs.Types.Config.Props.Credential.Smtp()
                    {
                        SmtpServer = smtpServer,
                        SmtpPort = smtpPort
                    }
                };

                // write .cred.toml with new credential object
                Libs.IO.ConfigReadWriter.WriteCredential(credential);

                // re-set new credential to dataholder
                Libs.DataHolder.Instance.SetCredential(credential);

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
