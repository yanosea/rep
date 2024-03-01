using System;
using System.Configuration;
using System.IO;
using Tomlyn;

namespace rep.Libs.IO
{
    /// <summary>
    /// class for config read and write
    /// </summary>
    internal static class ConfigReadWriter
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

        #endregion

        #region methods

        #region preferences

        /// <summary>
        /// read preferences.toml
        /// </summary>
        /// <returns>preferences</returns>
        /// <exception cref="Exceptions.ReadConfigFileException">ReadConfigFileException</exception>
        internal static Types.Config.Preferences ReadPreferences()
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // get preferences.toml path
                var preferencesFilePath = $"{ConfigDirPath}\\{Resources.Text.Environments.FileNamePreferencesConfig}";

                // return preferences object
                return ReadConfigFile<Types.Config.Preferences>(preferencesFilePath);
            }
            catch (Exception ex)
            {
                // if exception occured, throw read config file exception
                throw new Exceptions.ReadConfigFileException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// write preferences.toml
        /// </summary>
        /// <param name="preferences">preferences</param>
        /// <exception cref="Exceptions.WriteConfigFileException">WriteConfigFileException</exception>
        internal static void WritePreferences(Types.Config.Preferences preferences)
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // get preferences.toml path
                var preferencesFilePath = $"{ConfigDirPath}\\{Resources.Text.Environments.FileNamePreferencesConfig}";

                // write preferences.toml
                WriteConfigFile<Types.Config.Preferences>(preferencesFilePath, preferences);
            }
            catch (Exception ex)
            {
                // if exception occured, throw write config file exception
                throw new Exceptions.WriteConfigFileException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        #endregion

        #region credential

        /// <summary>
        /// read .cred.toml
        /// </summary>
        /// <returns>credential</returns>
        /// <exception cref="Exceptions.ReadConfigFileException">ReadConfigFileException</exception>
        internal static Types.Config.Credential ReadCredential()
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // get .cred.toml path
                var credentialFilePath = $"{ConfigDirPath}\\{Resources.Text.Environments.FileNameCredentialConfig}";

                // return credential object
                return ReadConfigFile<Types.Config.Credential>(credentialFilePath);
            }
            catch (Exception ex)
            {
                // if exception occured, throw read config file exception
                throw new Exceptions.ReadConfigFileException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// write .cred.toml
        /// </summary>
        /// <param name="credential">credential</param>
        /// <exception cref="Exceptions.WriteConfigFileException">WriteConfigFileException</exception>
        internal static void WriteCredential(Types.Config.Credential credential)
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // get .cred.toml path
                var credentialFilePath = $"{ConfigDirPath}\\{Resources.Text.Environments.FileNameCredentialConfig}";

                // write .cred.toml
                WriteConfigFile<Types.Config.Credential>(credentialFilePath, credential);
            }
            catch (Exception ex)
            {
                // if exception occured, throw write config file exception{
                throw new Exceptions.WriteConfigFileException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        #endregion

        #region log

        /// <summary>
        /// read log_conf.toml
        /// </summary>
        /// <returns>log_conf</returns>
        /// <exception cref="Exceptions.ReadConfigFileException">ReadConfigFileException</exception>
        internal static Types.Config.LogConfig ReadLogConfig()
        {
            try
            {
                // get log_conf.toml path
                var logConfigFilePath = $"{ConfigDirPath}\\{Resources.Text.Environments.FileNameLogConfig}";

                // return log_conf object
                return ReadConfigFile<Types.Config.LogConfig>(logConfigFilePath);
            }
            catch (Exception ex)
            {
                // if exception occured, throw read config file exception
                throw new Exceptions.ReadConfigFileException(ex.Message, ex);
            }
        }

        /// <summary>
        /// write log_conf.toml
        /// </summary>
        /// <param name="logConfig">log_conf</param>
        /// <exception cref="Exceptions.WriteConfigFileException">WriteConfigFileException</exception>
        internal static void WriteLogConfig(Types.Config.LogConfig logConfig)
        {
            try
            {
                // get log_conf.toml path
                var logConfigFilePath = $"{ConfigDirPath}\\{Resources.Text.Environments.FileNameLogConfig}";

                // write log_conf.toml
                WriteConfigFile<Types.Config.LogConfig>(logConfigFilePath, logConfig);
            }
            catch (Exception ex)
            {
                // if exception occured, throw write config file exception{
                throw new Exceptions.WriteConfigFileException(ex.Message, ex);
            }
        }

        #endregion

        #region reports

        #region daily_config 

        /// <summary>
        /// read daily_conf.toml
        /// </summary>
        /// <returns>daily_config</returns>
        /// <exception cref="Exceptions.ReadConfigFileException">ReadConfigFileException</exception>
        internal static Types.Config.Reports.DailyConfig ReadDailyConfig()
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // get daily_conf.toml path
                var dailyConfigFilePath = $"{ReportConfigDirPath}\\{Resources.Text.Environments.FileNameDailyConfig}";

                // return daily config object
                return ReadConfigFile<Types.Config.Reports.DailyConfig>(dailyConfigFilePath);
            }
            catch (Exception ex)
            {
                // if exception occured, throw read config file exception
                throw new Exceptions.ReadConfigFileException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// write daily_conf.toml
        /// </summary>
        /// <param name="dailyConfig">daily_config</param>
        /// <exception cref="Exceptions.WriteConfigFileException">WriteConfigFileException</exception>
        internal static void WriteDailyConfig(Types.Config.Reports.DailyConfig dailyConfig)
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // get daily_conf.toml path
                var dailyConfigFilePath = $"{ReportConfigDirPath}\\{Resources.Text.Environments.FileNameDailyConfig}";

                // write daily_conf.toml
                WriteConfigFile<Types.Config.Reports.DailyConfig>(dailyConfigFilePath, dailyConfig);
            }
            catch (Exception ex)
            {
                // if exception occured, throw write config file exception{
                throw new Exceptions.WriteConfigFileException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        #endregion

        #region weekly_config 

        /// <summary>
        /// read weekly_conf.toml
        /// </summary>
        /// <returns>weekly_config</returns>
        /// <exception cref="Exceptions.ReadConfigFileException">ReadConfigFileException</exception>
        internal static Types.Config.Reports.WeeklyConfig ReadWeeklyConfig()
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // get weekly_conf.toml path
                var weeklyConfigFilePath = $"{ReportConfigDirPath}\\{Resources.Text.Environments.FileNameWeeklyConfig}";

                // return weekly config object
                return ReadConfigFile<Types.Config.Reports.WeeklyConfig>(weeklyConfigFilePath);
            }
            catch (Exception ex)
            {
                // if exception occured, throw read config file exception
                throw new Exceptions.ReadConfigFileException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// write weekly_conf.toml
        /// </summary>
        /// <param name="weeklyConfig">weekly_config</param>
        /// <exception cref="Exceptions.WriteConfigFileException">WriteConfigFileException</exception>
        internal static void WriteWeeklyConfig(Types.Config.Reports.WeeklyConfig weeklyConfig)
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // get weekly_conf.toml path
                var weeklyConfigFilePath = $"{ReportConfigDirPath}\\{Resources.Text.Environments.FileNameWeeklyConfig}";

                // write weekly_conf.toml
                WriteConfigFile<Types.Config.Reports.WeeklyConfig>(weeklyConfigFilePath, weeklyConfig);
            }
            catch (Exception ex)
            {
                // if exception occured, throw write config file exception{
                throw new Exceptions.WriteConfigFileException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        #endregion

        #region monthly_config 

        /// <summary>
        /// read monthly_conf.toml
        /// </summary>
        /// <returns>monthly_config</returns>
        /// <exception cref="Exceptions.ReadConfigFileException">ReadConfigFileException</exception>
        internal static Types.Config.Reports.MonthlyConfig ReadMonthlyConfig()
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // get monthly_conf.toml path
                var monthlyConfigFilePath = $"{ReportConfigDirPath}\\{Resources.Text.Environments.FileNameMonthlyConfig}";

                // return monthly config object
                return ReadConfigFile<Types.Config.Reports.MonthlyConfig>(monthlyConfigFilePath);
            }
            catch (Exception ex)
            {
                // if exception occured, throw read config file exception
                throw new Exceptions.ReadConfigFileException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// write monthly_conf.toml
        /// </summary>
        /// <param name="monthlyConfig">monthly_config</param>
        /// <exception cref="Exceptions.WriteConfigFileException">WriteConfigFileException</exception>
        internal static void WriteMonthlyConfig(Types.Config.Reports.MonthlyConfig monthlyConfig)
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // get monthly_conf.toml path
                var monthlyConfigFilePath = $"{ReportConfigDirPath}\\{Resources.Text.Environments.FileNameMonthlyConfig}";

                // write monthly_conf.toml
                WriteConfigFile<Types.Config.Reports.MonthlyConfig>(monthlyConfigFilePath, monthlyConfig);
            }
            catch (Exception ex)
            {
                // if exception occured, throw write config file exception{
                throw new Exceptions.WriteConfigFileException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        #endregion

        #endregion

        #region dests

        #region daily_dest_config 

        /// <summary>
        /// read daily_dest_conf.toml
        /// </summary>
        /// <returns>daily_dest_config</returns>
        /// <exception cref="Exceptions.ReadConfigFileException">ReadConfigFileException</exception>
        internal static Types.Config.Dests.DailyDestConfig ReadDailyDestConfig()
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // get daily_dest_conf.toml path
                var dailyDestConfigFilePath = $"{DestConfigDirPath}\\{Resources.Text.Environments.FileNameDailyDestConfig}";

                // return daily dest config object
                return ReadConfigFile<Types.Config.Dests.DailyDestConfig>(dailyDestConfigFilePath);
            }
            catch (Exception ex)
            {
                // if exception occured, throw read config file exception
                throw new Exceptions.ReadConfigFileException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// write daily_dest_conf.toml
        /// </summary>
        /// <param name="dailyDestConfig">daily_dest_config</param>
        /// <exception cref="Exceptions.WriteConfigFileException">WriteConfigFileException</exception>
        internal static void WriteDailyDestConfig(Types.Config.Dests.DailyDestConfig dailyDestConfig)
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // get daily_dest_conf.toml path
                var dailyDestConfigFilePath = $"{DestConfigDirPath}\\{Resources.Text.Environments.FileNameDailyDestConfig}";

                // write daily_dest_conf.toml
                WriteConfigFile<Types.Config.Dests.DailyDestConfig>(dailyDestConfigFilePath, dailyDestConfig);
            }
            catch (Exception ex)
            {
                // if exception occured, throw write config file exception{
                throw new Exceptions.WriteConfigFileException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        #endregion

        #region weekly_dest_config 

        /// <summary>
        /// read weekly_dest_conf.toml
        /// </summary>
        /// <returns>weekly_dest_config</returns>
        /// <exception cref="Exceptions.ReadConfigFileException">ReadConfigFileException</exception>
        internal static Types.Config.Dests.WeeklyDestConfig ReadWeeklyDestConfig()
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // get weekly_dest_conf.toml path
                var weeklyDestConfigFilePath = $"{DestConfigDirPath}\\{Resources.Text.Environments.FileNameWeeklyDestConfig}";

                // return weekly dest config object
                return ReadConfigFile<Types.Config.Dests.WeeklyDestConfig>(weeklyDestConfigFilePath);
            }
            catch (Exception ex)
            {
                // if exception occured, throw read config file exception
                throw new Exceptions.ReadConfigFileException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// write weekly_dest_conf.toml
        /// </summary>
        /// <param name="weeklyDestConfig">weekly_dest_config</param>
        /// <exception cref="Exceptions.WriteConfigFileException">WriteConfigFileException</exception>
        internal static void WriteWeeklyDestConfig(Types.Config.Dests.WeeklyDestConfig weeklyDestConfig)
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // get weekly_dest_conf.toml path
                var weeklyDestConfigFilePath = $"{DestConfigDirPath}\\{Resources.Text.Environments.FileNameWeeklyDestConfig}";

                // write weekly_dest_conf.toml
                WriteConfigFile<Types.Config.Dests.WeeklyDestConfig>(weeklyDestConfigFilePath, weeklyDestConfig);
            }
            catch (Exception ex)
            {
                // if exception occured, throw write config file exception{
                throw new Exceptions.WriteConfigFileException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        #endregion

        #region monthly_dest_config 

        /// <summary>
        /// read monthly_dest_conf.toml
        /// </summary>
        /// <returns>monthly_dest_config</returns>
        /// <exception cref="Exceptions.ReadConfigFileException">ReadConfigFileException</exception>
        internal static Types.Config.Dests.MonthlyDestConfig ReadMonthlyDestConfig()
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // get monthly_dest_conf.toml path
                var monthlyDestConfigFilePath = $"{DestConfigDirPath}\\{Resources.Text.Environments.FileNameMonthlyDestConfig}";

                // return monthly dest config object
                return ReadConfigFile<Types.Config.Dests.MonthlyDestConfig>(monthlyDestConfigFilePath);
            }
            catch (Exception ex)
            {
                // if exception occured, throw read config file exception
                throw new Exceptions.ReadConfigFileException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// write monthly_dest_conf.toml
        /// </summary>
        /// <param name="monthlyDestConfig">monthly_dest_config</param>
        /// <exception cref="Exceptions.WriteConfigFileException">WriteConfigFileException</exception>
        internal static void WriteMonthlyDestConfig(Types.Config.Dests.MonthlyDestConfig monthlyDestConfig)
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // get monthly_dest_conf.toml path
                var monthlyDestConfigFilePath = $"{DestConfigDirPath}\\{Resources.Text.Environments.FileNameMonthlyDestConfig}";

                // write monthly_dest_conf.toml
                WriteConfigFile<Types.Config.Dests.MonthlyDestConfig>(monthlyDestConfigFilePath, monthlyDestConfig);
            }
            catch (Exception ex)
            {
                // if exception occured, throw write config file exception{
                throw new Exceptions.WriteConfigFileException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        #endregion

        #endregion

        #endregion

        #region private methods

        /// <summary>
        /// ReadConfigFile
        /// </summary>
        /// <param name="configFilePath">configFilePath</param>
        /// <returns>config object</returns>
        private static Type ReadConfigFile<Type>(string configFilePath) where Type : class, new()
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                using (var streamReader = new StreamReader(configFilePath))
                {
                    // read config file and return config object
                    return Toml.ToModel<Type>(streamReader.ReadToEnd());
                }
            }
            catch (Exception)
            {
                // if exception occured, throw
                throw;
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// WriteConfigFile
        /// </summary>
        /// <typeparam name="Type">type of config</typeparam>
        /// <param name="configFilePath">confitFilePath</param>
        /// <param name="config">config object</param>
        private static void WriteConfigFile<Type>(string configFilePath, Type config) where Type : class
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                using (var streamWriter = new StreamWriter(configFilePath))
                {
                    // write config object to config File
                    streamWriter.Write(Toml.FromModel(Toml.ToModel(Toml.FromModel(config))));
                }
            }
            catch (Exception)
            {
                // if exception occured, throw
                throw;
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