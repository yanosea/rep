using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace rep.Libs
{
    /// <summary>
    /// Mailer
    /// </summary>
    internal static class Mailer
    {
        #region methods

        /// <summary>
        /// Send
        /// </summary>
        /// <param name="messages">messages</param>
        /// <param name="credential">credential</param>
        /// <param name="preferences">preferences</param>
        /// <returns>result list(model,  send date, is succeeded)</returns>
        /// <exception cref="Exceptions.MailerException">MailerException</exception>
        internal static async Task<Dictionary<Database.Models.Interfaces.IReport, Tuple<DateTime, int>>> Send(Dictionary<Database.Models.Interfaces.IReport, List<string>> messages,
            Types.Config.Credential credential, Types.Config.Preferences preferences)
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            // declare var for attached files streams
            var attachedFilesStreams = new List<System.IO.FileStream>();

            try
            {
                // declare var for result list
                var resultList = new Dictionary<Database.Models.Interfaces.IReport, Tuple<DateTime, int>>();

                // on a-per-messages basis
                foreach (var message in messages)
                {
                    // declare var for mime message
                    var mimeMessage = new MimeKit.MimeMessage();

                    // set subject
                    mimeMessage.Subject = message.Key.SUBJECT;

                    // set from
                    mimeMessage.From.Add(new MimeKit.MailboxAddress($"{preferences.User.LastName} {preferences.User.FirstName}", credential.Account.Mailaddress));

                    // set tos
                    foreach (var to in FrontendUtility.DestsStringToDests<Types.Config.Props.Dests.To>(message.Key.TO))
                    {
                        mimeMessage.To.Add(new MimeKit.MailboxAddress(to.Name, to.Mailaddress));
                    }

                    // set ccs
                    foreach (var cc in FrontendUtility.DestsStringToDests<Types.Config.Props.Dests.Cc>(message.Key.CC))
                    {
                        mimeMessage.Cc.Add(new MimeKit.MailboxAddress(cc.Name, cc.Mailaddress));
                    }

                    // set bccs
                    foreach (var bcc in FrontendUtility.DestsStringToDests<Types.Config.Props.Dests.Bcc>(message.Key.BCC))
                    {
                        mimeMessage.Bcc.Add(new MimeKit.MailboxAddress(bcc.Name, bcc.Mailaddress));
                    }

                    // declare var for body of mime message
                    var bodyBuilder = new MimeKit.BodyBuilder();

                    // build messager
                    bodyBuilder.TextBody = BuildMessageBody(message.Key);

                    if (message.Value != null && message.Value.Count != 0)
                    {
                        // if attached files exist, on a per-attached-file basis 
                        foreach (var attachedFilePath in message.Value)
                        {
                            // set attached files stream
                            var attachedFilesStream = System.IO.File.OpenRead(attachedFilePath);
                            attachedFilesStreams.Add(attachedFilesStream);

                            // create mimepart
                            var mimePart = new MimeKit.MimePart(MimeKit.MimeTypes.GetMimeType(attachedFilePath))
                            {
                                Content = new MimeKit.MimeContent(attachedFilesStream),
                                ContentDisposition = new MimeKit.ContentDisposition(),
                                ContentTransferEncoding = MimeKit.ContentEncoding.Base64,
                                FileName = System.IO.Path.GetFileName(attachedFilePath)
                            };

                            // Add attachments
                            bodyBuilder.Attachments.Add(mimePart);
                        }
                    }

                    // body build
                    mimeMessage.Body = bodyBuilder.ToMessageBody();

                    using (var client = new MailKit.Net.Smtp.SmtpClient())
                    {
                        try
                        {
                            try
                            {
                                // connect
                                await client.ConnectAsync(credential.Smtp.SmtpServer, credential.Smtp.SmtpPort, MailKit.Security.SecureSocketOptions.SslOnConnect);

                                // connect log
                                Logger.Instance.TraceInformation(string.Format(Resources.Text.Formats.LogSuccessConnectingFormat, credential.Smtp.SmtpServer, credential.Smtp.SmtpPort));
                            }
                            catch (Exception ex)
                            {
                                // if exception occured on connecting, throw connect exception
                                throw new Exceptions.ConnectException(ex.Message, ex);
                            }

                            try
                            {
                                // authenticate
                                await client.AuthenticateAsync(credential.Account.Mailaddress, credential.Account.Password);

                                // authenticate log
                                Logger.Instance.TraceInformation(string.Format(Resources.Text.Formats.LogSuccessAuthenticatingFormat, credential.Account.Mailaddress));
                            }
                            catch (Exception ex)
                            {
                                // if exception occured on authenticating, throw authenticate exception
                                throw new Exceptions.AuthenticateException(ex.Message, ex);
                            }

                            try
                            {
                                // send
                                await client.SendAsync(mimeMessage);

                                // send log
                                Logger.Instance.TraceInformation(string.Format(Resources.Text.Formats.LogSuccessSendingFormat, mimeMessage.Subject));

                                // on a per-to basis
                                foreach (var to in mimeMessage.To.Mailboxes)
                                {
                                    // to log
                                    Logger.Instance.TraceInformation(string.Format(Resources.Text.Formats.LogSentToFormat, to.Name, to.Address));
                                }

                                // on a per-cc basis
                                foreach (var cc in mimeMessage.Cc.Mailboxes)
                                {
                                    // cc log
                                    Logger.Instance.TraceInformation(string.Format(Resources.Text.Formats.LogSentCcFormat, cc.Name, cc.Address));
                                }

                                // on a per-bcc basis
                                foreach (var bcc in mimeMessage.Bcc.Mailboxes)
                                {
                                    // bcc log
                                    Logger.Instance.TraceInformation(string.Format(Resources.Text.Formats.LogSentBccFormat, bcc.Name, bcc.Address));
                                }

                                // on a per-attachment basis
                                foreach (MimeKit.MimePart attachment in mimeMessage.Attachments)
                                {
                                    // attachment log
                                    Logger.Instance.TraceInformation(string.Format(Resources.Text.Formats.LogSentAttachmentsFormat, attachment.FileName));
                                }
                            }
                            catch (Exception ex)
                            {
                                // if exception occured on sending, throw send exception
                                throw new Exceptions.SendException(ex.Message, ex);
                            }

                            try
                            {
                                // disconnect
                                await client.DisconnectAsync(true);

                                // disconnect log
                                Logger.Instance.TraceInformation(string.Format(Resources.Text.Formats.LogSuccessDisconnectingFormat, credential.Smtp.SmtpServer, credential.Smtp.SmtpPort));
                            }
                            catch (Exception ex)
                            {
                                // if exception occured on disconnecting, throw disconnect exception
                                throw new Exceptions.DisconnectException(ex.Message, ex);
                            }

                            // add success result to list
                            resultList.Add(message.Key, new Tuple<DateTime, int>(DateTime.Now, 1));
                        }
                        catch (Exceptions.ConnectException ex)
                        {
                            // fatal log
                            Logger.Instance.TraceFatal(Resources.Text.Components.TextLogTagMailerFatal, Resources.Text.LogMessages.FailureConnecting, ex);

                            // add failure result to list
                            resultList.Add(message.Key, new Tuple<DateTime, int>(DateTime.Now, 0));
                        }
                        catch (Exceptions.AuthenticateException ex)
                        {
                            // fatal log
                            Logger.Instance.TraceFatal(Resources.Text.Components.TextLogTagMailerFatal, Resources.Text.LogMessages.FailureAuthenticating, ex);

                            // add failure result to list
                            resultList.Add(message.Key, new Tuple<DateTime, int>(DateTime.Now, 0));
                        }
                        catch (Exceptions.SendException ex)
                        {
                            // fatal log
                            Logger.Instance.TraceFatal(Resources.Text.Components.TextLogTagMailerFatal, Resources.Text.LogMessages.FailureSending, ex);

                            // add failure result to list
                            resultList.Add(message.Key, new Tuple<DateTime, int>(DateTime.Now, 0));
                        }
                        catch (Exceptions.DisconnectException ex)
                        {
                            // fatal log
                            Logger.Instance.TraceFatal(Resources.Text.Components.TextLogTagMailerFatal, Resources.Text.LogMessages.FailureDisconnecting, ex);

                            // add failure result to list
                            resultList.Add(message.Key, new Tuple<DateTime, int>(DateTime.Now, 0));
                        }
                    }

                    if (attachedFilesStreams != null && attachedFilesStreams.Count != 0)
                    {
                        // if file attached, close all streams
                        foreach (var attachedFileStream in attachedFilesStreams)
                        {
                            attachedFileStream.Close();
                        }
                    }
                }

                // return result list
                return resultList;
            }
            catch (Exception ex)
            {
                // fatal log
                Logger.Instance.TraceFatal(Resources.Text.Components.TextLogTagMailerFatal, Resources.Text.LogMessages.FailureSomethingWrongOccured, ex);

                if (attachedFilesStreams != null && attachedFilesStreams.Count != 0)
                {
                    // if file attached, close all streams
                    foreach (var attachedFileStream in attachedFilesStreams)
                    {
                        attachedFileStream.Close();
                    }
                }

                // if exception occured, throw mailer exception
                throw new Exceptions.MailerException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        #endregion

        #region private methods

        /// <summary>
        /// BuildMessageBody
        /// </summary>
        /// <param name="report">report</param>
        /// <returns>message body</returns>
        /// <exception cref="Exceptions.MailerException">MailerException</exception>
        private static string BuildMessageBody(Database.Models.Interfaces.IReport report)
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // declare var for message body
                var messageBody = new StringBuilder();

                // add head
                messageBody.AppendLine(report.HEAD);

                if (report is Database.Models.DAILY daily && daily.DAILY_TYPE == 1)
                {
                    // if report type is daily, add blank line between head and time
                    messageBody.AppendLine(string.Empty);

                    // add time
                    messageBody.AppendLine(daily.TIME);
                }

                // add blank line between head (or time) and body
                messageBody.AppendLine(string.Empty);

                // add body
                messageBody.AppendLine(report.BODY);

                // add blank line between body and foot
                messageBody.AppendLine(string.Empty);

                // add foot
                messageBody.AppendLine(report.FOOT);

                // return message body
                return messageBody.ToString();
            }
            catch (Exception ex)
            {
                // fatal log
                Logger.Instance.TraceFatal(Resources.Text.Components.TextLogTagMailerFatal, Resources.Text.LogMessages.FailureSomethingWrongOccured, ex);

                // if exception occured, throw mailer exception
                throw new Exceptions.MailerException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }
    }

    #endregion
}
