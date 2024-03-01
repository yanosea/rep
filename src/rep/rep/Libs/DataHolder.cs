using rep.Libs.IO;
using System;
using System.Collections.Generic;

namespace rep.Libs
{
    /// <summary>
    /// DataHolder
    /// </summary>
    internal sealed class DataHolder
    {
        #region singleton

        /// <summary>
        /// static instance
        /// </summary>
        internal static readonly DataHolder Instance = new DataHolder();

        #endregion

        #region fields

        #region command line arguments

        private static List<bool> CommandLineArguments { get; set; }

        #endregion

        #region configs

        /// <summary>
        /// credential
        /// </summary>
        private static Types.Config.Credential Credential { get; set; }

        /// <summary>
        /// preferences
        /// </summary>
        private static Types.Config.Preferences Preferences { get; set; }

        /// <summary>
        /// daily_conf
        /// </summary>
        private static Types.Config.Reports.DailyConfig DailyConfig { get; set; }

        /// <summary>
        /// weekly_conf
        /// </summary>
        private static Types.Config.Reports.WeeklyConfig WeeklyConfig { get; set; }

        /// <summary>
        /// monthly_conf
        /// </summary>
        private static Types.Config.Reports.MonthlyConfig MonthlyConfig { get; set; }

        /// <summary>
        /// daily_dest_conf
        /// </summary>
        private static Types.Config.Dests.DailyDestConfig DailyDestConfig { get; set; }

        /// <summary>
        /// weekly_dest_conf
        /// </summary>
        private static Types.Config.Dests.WeeklyDestConfig WeeklyDestConfig { get; set; }

        /// <summary>
        /// monthly_dest_conf
        /// </summary>
        private static Types.Config.Dests.MonthlyDestConfig MonthlyDestConfig { get; set; }

        #endregion

        #region flags

        /// <summary>
        /// IsInitializedSuccessfully
        /// </summary>
        private static bool InitializedSuccessfully = true;

        /// <summary>
        /// StartUp
        /// </summary>
        private static bool StartUp = true;

        /// <summary>
        /// FirstInitializeWrite
        /// </summary>
        private static bool FirstInitializeWrite = true;

        /// <summary>
        /// FirstInitializeDaily
        /// </summary>
        private static bool FirstInitializeDaily = true;


        /// <summary>
        /// FirstInitializeWeekly
        /// </summary>
        private static bool FirstInitializeWeekly = true;

        /// <summary>
        /// FirstInitializeMonthly
        /// </summary>
        private static bool FirstInitializeMonthly = true;

        /// <summary>
        /// SelectedDaily
        /// </summary>
        private static bool SelectedDaily { get; set; }

        /// <summary>
        /// SelectedWeekly
        /// </summary>
        private static bool SelectedWeekly { get; set; }

        /// <summary>
        /// SelectedMonthly
        /// </summary>
        private static bool SelectedMonthly { get; set; }

        /// <summary>
        /// PreparedDaily
        /// </summary>
        private static bool PreparedDaily = false;

        #endregion

        #region unsend mails

        /// <summary>
        /// UnsendDaily
        /// </summary>
        private static Database.Models.DAILY UnsendDaily { get; set; }

        /// <summary>
        /// UnsendWeekly
        /// </summary>
        private static Database.Models.WEEKLY UnsendWeekly { get; set; }

        /// <summary>
        /// UnsendWeeklyFilePaths
        /// </summary>
        private static List<string> UnsendWeeklyFilePaths = new List<string>();

        /// <summary>
        /// UnsendMonthly
        /// </summary>
        private static Database.Models.MONTHLY UnsendMonthly { get; set; }

        /// <summary>
        /// UnsendMonthlyFilePaths
        /// </summary>
        private static List<string> UnsendMonthlyFilePaths = new List<string>();

        #endregion

        #endregion

        #region methods

        /// <summary>
        /// initialize
        /// </summary>
        /// <exception cref="Exceptions.BackendUtilException">backend util exception</exception>
        /// <exception cref="Exceptions.ReadConfigFileException">read config files exception</exception>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal void Initialize()
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // get command line args
                CommandLineArguments = BackendUtility.GetCommandLineArguments();

                // read .cred.toml
                Credential = ConfigReadWriter.ReadCredential();

                // read pref.toml
                Preferences = ConfigReadWriter.ReadPreferences();

                // read daily_conf.toml
                DailyConfig = ConfigReadWriter.ReadDailyConfig();

                // read weekly_conf.toml
                WeeklyConfig = ConfigReadWriter.ReadWeeklyConfig();

                // read monthly_conf.toml
                MonthlyConfig = ConfigReadWriter.ReadMonthlyConfig();

                // read daily_dest_conf.toml
                DailyDestConfig = ConfigReadWriter.ReadDailyDestConfig();

                // read weekly_dest_conf.toml
                WeeklyDestConfig = ConfigReadWriter.ReadWeeklyDestConfig();

                // read weekly_dest_conf.toml
                MonthlyDestConfig = ConfigReadWriter.ReadMonthlyDestConfig();
            }
            catch (Exceptions.BackendUtilException ex)
            {
                // fatal log
                Logger.Instance.TraceFatal(Resources.Text.Components.TextLogTagBackendFatal, Resources.Text.LogMessages.FailureGettingCommandLineArguments, ex);

                // if backend util exception occured, throw
                throw;
            }
            catch (Exceptions.ReadConfigFileException ex)
            {
                // fatal log
                Logger.Instance.TraceFatal(Resources.Text.Components.TextLogTagBackendFatal, Resources.Text.LogMessages.FailureReadingConfigFiles, ex);

                // if read config files exception occured, throw
                throw;
            }
            catch (Exception ex)
            {
                // fatal log
                Logger.Instance.TraceFatal(Resources.Text.Components.TextLogTagBackendFatal, Resources.Text.LogMessages.FailureSomethingWrongOccured, ex);

                // if something wrong occured, throw new unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        #region getter

        #region command line arguments

        /// <summary>
        /// GetCommandLineArguments
        /// </summary>
        /// <returns>command line arguments</returns>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal List<bool> GetCommandLineArguments()
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                return CommandLineArguments;
            }
            catch (Exception ex)
            {
                // if something wrong occured, throw new unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        #endregion

        #region config

        /// <summary>
        /// GetCredential
        /// </summary>
        /// <returns>credential</returns>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal Types.Config.Credential GetCredential()
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                return Credential;
            }
            catch (Exception ex)
            {
                // if something wrong occured, throw new unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// GetPreferences
        /// </summary>
        /// <returns>preferences</returns>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal Types.Config.Preferences GetPreferences()
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                return Preferences;
            }
            catch (Exception ex)
            {
                // if something wrong occured, throw new unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// GetDailyConfig
        /// </summary>
        /// <returns>daily_conf</returns>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal Types.Config.Reports.DailyConfig GetDailyConfig()
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                return DailyConfig;
            }
            catch (Exception ex)
            {
                // if something wrong occured, throw new unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// GetWeeklyConfig
        /// </summary>
        /// <returns>weekly_conf</returns>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal Types.Config.Reports.WeeklyConfig GetWeeklyConfig()
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                return WeeklyConfig;
            }
            catch (Exception ex)
            {
                // if something wrong occured, throw new unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// GetMonthlyConfig
        /// </summary>
        /// <returns>monthly_conf</returns>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal Types.Config.Reports.MonthlyConfig GetMonthlyConfig()
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                return MonthlyConfig;
            }
            catch (Exception ex)
            {
                // if something wrong occured, throw new unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// GetDailyDestConfig
        /// </summary>
        /// <returns>daily_dest_conf</returns>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal Types.Config.Dests.DailyDestConfig GetDailyDestConfig()
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                return DailyDestConfig;
            }
            catch (Exception ex)
            {
                // if something wrong occured, throw new unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// GetWeeklyDestConfig
        /// </summary>
        /// <returns>weekly_dest_conf</returns>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal Types.Config.Dests.WeeklyDestConfig GetWeeklyDestConfig()
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                return WeeklyDestConfig;
            }
            catch (Exception ex)
            {
                // if something wrong occured, throw new unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// GetMonthlyDestConfig
        /// </summary>
        /// <returns>monthly_dest_conf</returns>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal Types.Config.Dests.MonthlyDestConfig GetMonthlyDestConfig()
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                return MonthlyDestConfig;
            }
            catch (Exception ex)
            {
                // if something wrong occured, throw new unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        #endregion

        #region flags

        /// <summary>
        /// IsInitializedSuccessfully
        /// </summary>
        /// <returns>IsInitializedSuccessfully</returns>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal bool IsInitializedSuccessfully()
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                return InitializedSuccessfully;
            }
            catch (Exception ex)
            {
                // if something wrong occured, throw new unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// IsStartUp
        /// </summary>
        /// <returns>IsStartUp</returns>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal bool IsStartUp()
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                return StartUp;
            }
            catch (Exception ex)
            {
                // if something wrong occured, throw new unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// IsFirstInitializeWrite
        /// </summary>
        /// <returns>IsFirstInitializeWrite</returns>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal bool IsFirstInitializeWrite()
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                return FirstInitializeWrite;
            }
            catch (Exception ex)
            {
                // if something wrong occured, throw new unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// IsFirstInitializeDaily
        /// </summary>
        /// <returns>IsFirstInitializeDaily</returns>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal bool IsFirstInitializeDaily()
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                return FirstInitializeDaily;
            }
            catch (Exception ex)
            {
                // if something wrong occured, throw new unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// IsFirstInitializeWeekly
        /// </summary>
        /// <returns>IsFirstInitializeWeekly</returns>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal bool IsFirstInitializeWeekly()
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                return FirstInitializeWeekly;
            }
            catch (Exception ex)
            {
                // if something wrong occured, throw new unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// IsFirstInitializeMonthly
        /// </summary>
        /// <returns>IsFirstInitializeMonthly</returns>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal bool IsFirstInitializeMonthly()
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                return FirstInitializeMonthly;
            }
            catch (Exception ex)
            {
                // if something wrong occured, throw new unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// IsSelectedDaily
        /// </summary>
        /// <returns>IsSelectedDaily</returns>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal bool IsSelectedDaily()
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                return SelectedDaily;
            }
            catch (Exception ex)
            {
                // if something wrong occured, throw new unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// IsSelectedWeekly
        /// </summary>
        /// <returns>IsSelectedWeekly</returns>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal bool IsSelectedWeekly()
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                return SelectedWeekly;
            }
            catch (Exception ex)
            {
                // if something wrong occured, throw new unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// IsSelectedMonthly
        /// </summary>
        /// <returns>IsSelectedMonthly</returns>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal bool IsSelectedMonthly()
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                return SelectedMonthly;
            }
            catch (Exception ex)
            {
                // if something wrong occured, throw new unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// IsPreparedDaily
        /// </summary>
        /// <returns>IsPreparedDaily</returns>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal bool IsPreparedDaily()
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                return PreparedDaily;
            }
            catch (Exception ex)
            {
                // if something wrong occured, throw new unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        #endregion

        #region unsend mails

        /// <summary>
        /// GetUnsendDaily
        /// </summary>
        /// <returns>unsend daily</returns>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal Database.Models.DAILY GetUnsendDaily()
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                return UnsendDaily;
            }
            catch (Exception ex)
            {
                // if something wrong occured, throw new unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// GetUnsendWeekly
        /// </summary>
        /// <returns>unsend weekly</returns>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal Database.Models.WEEKLY GetUnsendWeekly()
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                return UnsendWeekly;
            }
            catch (Exception ex)
            {
                // if something wrong occured, throw new unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// GetUnsendWeeklyFilePaths
        /// </summary>
        /// <returns>unsend weekly file paths</returns>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal List<string> GetUnsendWeeklyFilePaths()
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                return UnsendWeeklyFilePaths;
            }
            catch (Exception ex)
            {
                // if something wrong occured, throw new unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// GetUnsendMonthly
        /// </summary>
        /// <returns>unsend monthly</returns>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal Database.Models.MONTHLY GetUnsendMonthly()
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                return UnsendMonthly;
            }
            catch (Exception ex)
            {
                // if something wrong occured, throw new unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// GetUnsendMonthlyFilePaths
        /// </summary>
        /// <returns>unsend monthly file paths</returns>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal List<string> GetUnsendMonthlyFilePaths()
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                return UnsendMonthlyFilePaths;
            }
            catch (Exception ex)
            {
                // if something wrong occured, throw new unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        #endregion

        #endregion

        #region setter

        #region command line arguments

        /// <summary>
        /// SetCommandLineArguments
        /// </summary>
        /// <param name="commandLineArguments">command line arguments</param>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal void SetCommandLineArguments(List<bool> commandLineArguments)
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                CommandLineArguments = commandLineArguments;
            }
            catch (Exception ex)
            {
                // if something wrong occured, throw new unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        #endregion

        #region config

        /// <summary>
        /// SetCredential
        /// </summary>
        /// <param name="credential">credential</param>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal void SetCredential(Types.Config.Credential credential)
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                Credential = credential;
            }
            catch (Exception ex)
            {
                // if something wrong occured, throw new unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// SetPreference
        /// </summary>
        /// <param name="preferences">preferences</param>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal void SetPreferences(Types.Config.Preferences preferences)
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                Preferences = preferences;
            }
            catch (Exception ex)
            {
                // if something wrong occured, throw new unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// SetDailyConfig
        /// </summary>
        /// <param name="dailyConfig">dailyConfig</param>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal void SetDailyConfig(Types.Config.Reports.DailyConfig dailyConfig)
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                DailyConfig = dailyConfig;
            }
            catch (Exception ex)
            {
                // if something wrong occured, throw new unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// SetWeeklyConfig
        /// </summary>
        /// <param name="weeklyConfig">weeklyConfig</param>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal void SetWeeklyConfig(Types.Config.Reports.WeeklyConfig weeklyConfig)
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                WeeklyConfig = weeklyConfig;
            }
            catch (Exception ex)
            {
                // if something wrong occured, throw new unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// SetMonthlyConfig
        /// </summary>
        /// <param name="monthlyConfig">monthlyConfig</param>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal void SetMonthlyConfig(Types.Config.Reports.MonthlyConfig monthlyConfig)
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                MonthlyConfig = monthlyConfig;
            }
            catch (Exception ex)
            {
                // if something wrong occured, throw new unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// SetDailyDestConfig
        /// </summary>
        /// <param name="dailyDestConfig">dailyDestConfig</param>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal void SetDailyDestConfig(Types.Config.Dests.DailyDestConfig dailyDestConfig)
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                DailyDestConfig = dailyDestConfig;
            }
            catch (Exception ex)
            {
                // if something wrong occured, throw new unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// SetWeeklyDestConfig
        /// </summary>
        /// <param name="weeklyDestConfig">weeklyDestConfig</param>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal void SetWeeklyDestConfig(Types.Config.Dests.WeeklyDestConfig weeklyDestConfig)
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                WeeklyDestConfig = weeklyDestConfig;
            }
            catch (Exception ex)
            {
                // if something wrong occured, throw new unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// SetMonthlyDestConfig
        /// </summary>
        /// <param name="monthlyDestConfig">monthlyDestConfig</param>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal void SetMonthlyDestConfig(Types.Config.Dests.MonthlyDestConfig monthlyDestConfig)
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                MonthlyDestConfig = monthlyDestConfig;
            }
            catch (Exception ex)
            {
                // if something wrong occured, throw new unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        #endregion

        #region flags

        /// <summary>
        /// SetInitializedSuccessfully
        /// </summary>
        /// <param name="isInitializedSuccessfully">isInitializedSuccessfully</param>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal void SetInitializedSuccessfully(bool isInitializedSuccessfully)
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                InitializedSuccessfully = isInitializedSuccessfully;
            }
            catch (Exception ex)
            {
                // if something wrong occured, throw new unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// SetStartUp
        /// </summary>
        /// <param name="isStartUp">isStartUp</param>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal void SetStartUp(bool isStartUp)
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                StartUp = isStartUp;
            }
            catch (Exception ex)
            {
                // if something wrong occured, throw new unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// SetFirstInitializeWrite
        /// </summary>
        /// <param name="isFirstInitializeWrite">isFirstInitializeWrite</param>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal void SetFirstInitializeWrite(bool isFirstInitializeWrite)
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                FirstInitializeWrite = isFirstInitializeWrite;
            }
            catch (Exception ex)
            {
                // if something wrong occured, throw new unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// SetFirstInitializeDaily
        /// </summary>
        /// <param name="isFirstInitializeDaily">isFirstInitializeDaily</param>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal void SetFirstInitializeDaily(bool isFirstInitializeDaily)
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                FirstInitializeDaily = isFirstInitializeDaily;
            }
            catch (Exception ex)
            {
                // if something wrong occured, throw new unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// SetFirstInitializeWeekly
        /// </summary>
        /// <param name="isFirstInitializeWeekly">isFirstInitializeWeekly</param>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal void SetFirstInitializeWeekly(bool isFirstInitializeWeekly)
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                FirstInitializeWeekly = isFirstInitializeWeekly;
            }
            catch (Exception ex)
            {
                // if something wrong occured, throw new unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// SetFirstInitializeMonthly
        /// </summary>
        /// <param name="isFirstInitializeMonthly">isFirstInitializeMonthly</param>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal void SetFirstInitializeMonthly(bool isFirstInitializeMonthly)
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                FirstInitializeMonthly = isFirstInitializeMonthly;
            }
            catch (Exception ex)
            {
                // if something wrong occured, throw new unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// SetSelectedDaily
        /// </summary>
        /// <param name="isSelectedDaily">isSelectedDaily</param>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal void SetSelectedDaily(bool isSelectedDaily)
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                SelectedDaily = isSelectedDaily;
            }
            catch (Exception ex)
            {
                // if something wrong occured, throw new unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// SetSelectedWeekly
        /// </summary>
        /// <param name="isSelectedWekly">isSelectedWekly</param>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal void SetSelectedWeekly(bool isSelectedWekly)
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                SelectedWeekly = isSelectedWekly;
            }
            catch (Exception ex)
            {
                // if something wrong occured, throw new unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// SetSelectedMonthly
        /// </summary>
        /// <param name="isSelectedMonthly">isSelectedMonthly</param>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal void SetSelectedMonthly(bool isSelectedMonthly)
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                SelectedMonthly = isSelectedMonthly;
            }
            catch (Exception ex)
            {
                // if something wrong occured, throw new unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }
        /// <summary>
        /// SetPreparedDaily
        /// </summary>
        /// <param name="isPreparedDaily">isStartUp</param>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal void SetPreparedDaily(bool isPreparedDaily)
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                PreparedDaily = isPreparedDaily;
            }
            catch (Exception ex)
            {
                // if something wrong occured, throw new unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        #endregion

        #region unsend mails

        /// <summary>
        /// SetUnsendDaily
        /// </summary>
        /// <param name="unsendMonthly">unsendDaily</param>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal void SetUnsendDaily(Database.Models.DAILY unsendMonthly)
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                UnsendDaily = unsendMonthly;
            }
            catch (Exception ex)
            {
                // if something wrong occured, throw new unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// SetUnsendWeekly
        /// </summary>
        /// <param name="unsendWeekly">unsendWeekly</param>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal void SetUnsendWeekly(Database.Models.WEEKLY unsendWeekly)
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                UnsendWeekly = unsendWeekly;
            }
            catch (Exception ex)
            {
                // if something wrong occured, throw new unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// SetUnsendWeeklyFilePaths
        /// </summary>
        /// <param name="unsendWeeklyFilePaths">unsendWeeklyFilePaths</param>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal void SetUnsendWeeklyFilePaths(List<string> unsendWeeklyFilePaths)
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                UnsendWeeklyFilePaths = unsendWeeklyFilePaths;
            }
            catch (Exception ex)
            {
                // if something wrong occured, throw new unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// SetUnsendMonthly
        /// </summary>
        /// <param name="unsendMonthly">unsendMonthly</param>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal void SetUnsendMonthly(Database.Models.MONTHLY unsendMonthly)
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                UnsendMonthly = unsendMonthly;
            }
            catch (Exception ex)
            {
                // if something wrong occured, throw new unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// SetUnsendMonthlyFilePaths
        /// </summary>
        /// <param name="unsendMonthlyFilePaths">unsendMonthlyFilePaths</param>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal void SetUnsendMonthlyFilePaths(List<string> unsendMonthlyFilePaths)
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                UnsendMonthlyFilePaths = unsendMonthlyFilePaths;
            }
            catch (Exception ex)
            {
                // if something wrong occured, throw new unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
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
    }
}