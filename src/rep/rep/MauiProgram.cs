using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using System;
using System.Configuration;

namespace rep
{
    /// <summary>
    /// MauiProgram
    /// </summary>
    internal static class MauiProgram
    {
        /// <summary>
        /// common end point of rep
        /// </summary>
        /// <returns>maui app</returns>
        public static MauiApp CreateMauiApp()
        {
            // generage maui app
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>();
            builder.Services.AddMauiBlazorWebView();

            // add services
            // add dialog
            builder.Services.AddSingleton<Libs.Services.DialogService>();

#if DEBUG
            // for debug, enable devtool
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            // get app config
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            // set REP_DIR
            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["REP_DIR"]) || string.IsNullOrEmpty(config.AppSettings.Settings["REP_DIR"].Value))
            {
                // if no config, set user profile to REP_DIR
                config.AppSettings.Settings["REP_DIR"].Value = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\{Resources.Text.Environments.DirNameRep}";
                config.Save();
            }

            // initialize logger
            Libs.Logger.Instance.Initialize();

            // output version log
            Libs.Logger.Instance.TraceInformation(
                String.Format(Resources.Text.Formats.LogCurrentVersion, $"v{Libs.BackendUtility.GetCurrentVersion()}"));

            // start log
            Libs.Logger.Instance.TraceInformation(Resources.Text.Components.TextLogStartRep);

            // initialize rep
            Libs.Initializer.Initialize();

            // return maui app
            return builder.Build();
        }
    }
}