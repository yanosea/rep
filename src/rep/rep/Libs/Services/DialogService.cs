using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;

namespace rep.Libs.Services
{
    /// <summary>
    /// DialogService
    /// </summary>
    internal sealed class DialogService
    {
        #region singleton

        /// <summary>
        /// static instance
        /// </summary>
        internal static readonly DialogService Instance = new DialogService();

        #endregion

        #region methods
        /// <summary>
        /// ShowAlertAsync
        /// </summary>
        /// <param name="title">title</param>
        /// <param name="message">message</param>
        /// <param name="cancel">cancel</param>
        /// <returns>dialog</returns>
        internal Task ShowAlertAsync(string title, string message, string cancel = "ok")
        {
            return Application.Current.MainPage.DisplayAlert(title, message, cancel);
        }

        /// <summary>
        /// ShowConfirmationAsync
        /// </summary>
        /// <param name="title">title</param>
        /// <param name="message">message</param>
        /// <param name="accept">accept</param>
        /// <param name="cancel">cancel</param>
        /// <returns>dialog</returns>
        internal Task<bool> ShowConfirmationAsync(string title, string message, string accept = "yes", string cancel = "no")
        {
            return Application.Current.MainPage.DisplayAlert(title, message, accept, cancel);
        }

        /// <summary>
        /// ShowPromptAsync
        /// </summary>
        /// <param name="title">title</param>
        /// <param name="message">message</param>
        /// <param name="accept">accept</param>
        /// <param name="cancel">cancel</param>
        /// <returns>dialog</returns>
        internal Task<string> ShowPromptAsync(string title, string message, string accept = "yes", string cancel = "no")
        {
            return Application.Current.MainPage.DisplayPromptAsync(title, message, accept, cancel, maxLength: 100);
        }

        /// <summary>
        /// ShowAlert
        /// </summary>
        /// <param name="title">title</param>
        /// <param name="message">message</param>
        /// <param name="cancel">cancel</param>
        /// <returns>dialog</returns>
        internal void ShowAlert(string title, string message, string cancel = "ok")
        {
            Application.Current.MainPage.Dispatcher.Dispatch(async () =>
                await ShowAlertAsync(title, message, cancel)
            );
        }

        /// <summary>
        /// ShowConfirmation
        /// </summary>
        /// <param name="title">title</param>
        /// <param name="message">message</param>
        /// <param name="callback">Action to perform afterwards.</param>
        /// <param name="accept">accept</param>
        /// <param name="cancel">cancel</param>
        /// <returns>dialog</returns>
        internal void ShowConfirmation(string title, string message, Action<bool> callback,
                                     string accept = "yes", string cancel = "no")
        {
            Application.Current.MainPage.Dispatcher.Dispatch(async () =>
            {
                bool answer = await ShowConfirmationAsync(title, message, accept, cancel);
                callback(answer);
            });
        }

        #endregion
    }
}
