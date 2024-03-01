using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Configuration;

namespace rep.Libs.IO
{
    /// <summary>
    /// IO helper class
    /// </summary>
    internal static class IOHelper
    {
        #region readonlys

        /// <summary>
        /// Config
        /// </summary>
        private static readonly Configuration Config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        /// <summary>
        /// ConfigDirPath
        /// </summary>
        private static readonly string ConfigDirPath = $"{Config.AppSettings.Settings["REP_DIR"].Value}\\{Resources.Text.Environments.DirNameConfig}";

        /// <summary>
        /// ReportConfigDirPath
        /// </summary>
        private static readonly string ReportConfigDirPath = $"{ConfigDirPath}\\{Resources.Text.Environments.DirNameReportConfig}";

        /// <summary>
        /// DestConfigDirPath
        /// </summary>
        private static readonly string DestConfigDirPath = $"{ConfigDirPath}\\{Resources.Text.Environments.DirNameDestConfig}";

        /// <summary>
        /// ReportPath
        /// </summary>
        private static readonly string ReportDirPath = $"{Config.AppSettings.Settings["REP_DIR"].Value}\\{Resources.Text.Environments.DirNameReport}";

        #endregion

        #region methods

        /// <summary>
        /// create necesarry directories for rep
        /// </summary>
        /// <exception cref="Exceptions.CreateDirectoriesException">create directories exception</exception>
        internal static void CreateRepDirectories()
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // REP_DIR
                if (!System.IO.Directory.Exists(Config.AppSettings.Settings["REP_DIR"].Value))
                {
                    // if no rep dir, create
                    System.IO.Directory.CreateDirectory($"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\{Resources.Text.Environments.DirNameRep}");
                }

                // config dir
                if (!System.IO.Directory.Exists(ConfigDirPath))
                {
                    // if no config dir, create
                    System.IO.Directory.CreateDirectory(ConfigDirPath);
                }

                // report config dir
                if (!System.IO.Directory.Exists(ReportConfigDirPath))
                {
                    // if no report config dir, create
                    System.IO.Directory.CreateDirectory(ReportConfigDirPath);
                }

                // dest config dir
                if (!System.IO.Directory.Exists(DestConfigDirPath))
                {
                    // if no dest config dir, create
                    System.IO.Directory.CreateDirectory(DestConfigDirPath);
                }

                // reports dir
                if (!System.IO.Directory.Exists(ReportDirPath))
                {
                    // if no reports dir, create
                    System.IO.Directory.CreateDirectory(ReportDirPath);
                }

                // daily dir
                var dailyReportsDirectory = $"{ReportDirPath}\\{Resources.Text.Environments.DirNameDaily}";
                if (!System.IO.Directory.Exists(dailyReportsDirectory))
                {
                    // if no daily reports dir, create
                    System.IO.Directory.CreateDirectory(dailyReportsDirectory);
                }

                // opening dir
                var openingDirectory = $"{dailyReportsDirectory}\\{Resources.Text.Environments.DirNameOpening}";
                if (!System.IO.Directory.Exists(openingDirectory))
                {
                    // if no opening reports dir, create
                    System.IO.Directory.CreateDirectory(openingDirectory);
                }

                // closing dir
                var closingReportsDirectory = $"{dailyReportsDirectory}\\{Resources.Text.Environments.DirNameClosing}";
                if (!System.IO.Directory.Exists(closingReportsDirectory))
                {
                    // if no closing reports dir, create
                    System.IO.Directory.CreateDirectory(closingReportsDirectory);
                }

                // weekly dir
                var weeklyReportsDirectory = $"{ReportDirPath}\\{Resources.Text.Environments.DirNameWeekly}";
                if (!System.IO.Directory.Exists(weeklyReportsDirectory))
                {
                    // if no weekly reports dir, create
                    System.IO.Directory.CreateDirectory(weeklyReportsDirectory);
                }

                // monthly dir
                var monthlyReportsDirectory = $"{ReportDirPath}\\{Resources.Text.Environments.DirNameMonthly}";
                if (!System.IO.Directory.Exists(monthlyReportsDirectory))
                {
                    // if no monthly reports dir, create
                    System.IO.Directory.CreateDirectory(monthlyReportsDirectory);
                }

                // cache files dir
                var cacheFilesDirectory = $"{Config.AppSettings.Settings["REP_DIR"].Value}\\{Resources.Text.Environments.DirNameCachedFiles}";
                if (!System.IO.Directory.Exists(cacheFilesDirectory))
                {
                    // if no cache  dir, create
                    System.IO.Directory.CreateDirectory(cacheFilesDirectory);
                }

                // clear cached files
                var cachedFiles = System.IO.Directory.GetFiles(cacheFilesDirectory);
                foreach (var cachedFile in cachedFiles)
                {
                    System.IO.File.Delete(cachedFile);
                }

                // git dir 
                var clonePath = $"{System.IO.Path.GetTempPath()}\\{Resources.Text.Environments.DirNameGit}";
                if (!System.IO.Directory.Exists(clonePath))
                {
                    // if no git clone dir, create
                    System.IO.Directory.CreateDirectory(clonePath);
                }
            }
            catch (Exception ex)
            {
                // if exception occured, throw create directories exception
                throw new Exceptions.CreateDirectoriesException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// create necesarry files for rep
        /// </summary>
        /// <exception cref="Exceptions.CreateFilesException">CreateFilesException</exception>
        internal static void CreateRepFiles()
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // preferences.toml
                var preferencesFilePath = $"{ConfigDirPath}\\{Resources.Text.Environments.FileNamePreferencesConfig}";
                if (!System.IO.File.Exists(preferencesFilePath))
                {
                    // if no preferences.toml, create with default value
                    var defaultPreferences = new Types.Config.Preferences();
                    ConfigReadWriter.WritePreferences(defaultPreferences);
                }

                // .cred.toml
                var credentialFilePath = $"{ConfigDirPath}\\{Resources.Text.Environments.FileNameCredentialConfig}";
                if (!System.IO.File.Exists(credentialFilePath))
                {
                    // if no .cred.toml, create with default value
                    var defaultCredential = new Types.Config.Credential();
                    ConfigReadWriter.WriteCredential(defaultCredential);
                }

                // log_conf.toml
                var logConfigFilePath = $"{ConfigDirPath}\\{Resources.Text.Environments.FileNameLogConfig}";
                if (!System.IO.File.Exists(logConfigFilePath))
                {
                    // if no log_conf.toml, create with default value
                    var defaultLogConfig = new Types.Config.LogConfig();
                    ConfigReadWriter.WriteLogConfig(defaultLogConfig);
                }

                // daily_conf.toml
                var dailyConfigFilePath = $"{ReportConfigDirPath}\\{Resources.Text.Environments.FileNameDailyConfig}";
                if (!System.IO.File.Exists(dailyConfigFilePath))
                {
                    // if no daily_conf.toml, create with default value
                    var defaultDailyConfig = new Types.Config.Reports.DailyConfig();
                    ConfigReadWriter.WriteDailyConfig(defaultDailyConfig);
                }

                // weekly_conf.toml
                var weeklyConfigFilePath = $"{ReportConfigDirPath}\\{Resources.Text.Environments.FileNameWeeklyConfig}";
                if (!System.IO.File.Exists(weeklyConfigFilePath))
                {
                    // if no weekly_conf.toml, create with default value
                    var defaultWeeklyConfig = new Types.Config.Reports.WeeklyConfig();
                    ConfigReadWriter.WriteWeeklyConfig(defaultWeeklyConfig);
                }

                // monthly_conf.toml
                var monthlyConfigFilePath = $"{ReportConfigDirPath}\\{Resources.Text.Environments.FileNameMonthlyConfig}";
                if (!System.IO.File.Exists(monthlyConfigFilePath))
                {
                    // if no monthly_conf.toml, create with default value
                    var defaultMonthlyConfig = new Types.Config.Reports.MonthlyConfig();
                    ConfigReadWriter.WriteMonthlyConfig(defaultMonthlyConfig);
                }

                // daily_dest_conf.toml
                var dailyDestConfigFilePath = $"{DestConfigDirPath}\\{Resources.Text.Environments.FileNameDailyDestConfig}";
                if (!System.IO.File.Exists(dailyDestConfigFilePath))
                {
                    // if no daily_conf.toml, create with default value
                    var defaultDailyDestConfig = new Types.Config.Dests.DailyDestConfig();
                    ConfigReadWriter.WriteDailyDestConfig(defaultDailyDestConfig);
                }

                // weekly_dest_conf.toml
                var weeklyDestConfigFilePath = $"{DestConfigDirPath}\\{Resources.Text.Environments.FileNameWeeklyDestConfig}";
                if (!System.IO.File.Exists(weeklyDestConfigFilePath))
                {
                    // if no weekly_conf.toml, create with default value
                    var defaultWeeklyDestConfig = new Types.Config.Dests.WeeklyDestConfig();
                    ConfigReadWriter.WriteWeeklyDestConfig(defaultWeeklyDestConfig);
                }

                // monthly_dest_conf.toml
                var monthlyDestConfigFilePath = $"{DestConfigDirPath}\\{Resources.Text.Environments.FileNameMonthlyDestConfig}";
                if (!System.IO.File.Exists(monthlyDestConfigFilePath))
                {
                    // if no monthly_conf.toml, create with default value
                    var defaultMonthlyDestConfig = new Types.Config.Dests.MonthlyDestConfig();
                    ConfigReadWriter.WriteMonthlyDestConfig(defaultMonthlyDestConfig);
                }

                // report.db
                var sqliteDBFilePath = $"{ReportDirPath}\\{Resources.Text.Environments.FileNameSQLiteDB}";
                var context = new Database.RepContext();
                if (!System.IO.File.Exists(sqliteDBFilePath))
                {
                    // if no reports.db, create
                    var rdc = (RelationalDatabaseCreator)context.Database.GetService<IDatabaseCreator>();
                    rdc.CreateTables();
                }
                else
                {
                    // if report.db exists、migrate
                    context.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                // if exception occured, throw create files exception
                throw new Exceptions.CreateFilesException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        #endregion
    }
}
