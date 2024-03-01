using log4net.Config;
using rep.Libs.IO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace rep.Libs
{
    /// <summary>
    /// log4net wrapper class
    /// </summary>
    internal sealed class Logger
    {
        #region singleton

        /// <summary>
        /// static instance
        /// </summary>
        internal static readonly Logger Instance = new Logger();

        #endregion

        #region fields

        /// <summary>
        /// log4net
        /// </summary>
        private log4net.ILog _logger = log4net.LogManager.GetLogger("Default");

        #endregion

        #region methods

        /// <summary>
        /// Initialize
        /// </summary>
        internal void Initialize()
        {
            // get app config
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            // get REP_DIR
            if (!System.IO.Directory.Exists(config.AppSettings.Settings["REP_DIR"].Value))
            {
                // if no REP_DIR, create
                System.IO.Directory.CreateDirectory($"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\{Resources.Text.Environments.DirNameRep}");
            }

            // get log config dir path
            var logConfigFileDirectory = $"{config.AppSettings.Settings["REP_DIR"].Value}\\{Resources.Text.Environments.DirNameConfig}";
            if (!System.IO.Directory.Exists(logConfigFileDirectory))
            {
                // if no config dir, create log config dir
                System.IO.Directory.CreateDirectory(logConfigFileDirectory);
            }

            // get log config file path
            var logConfigFilePath = $"{logConfigFileDirectory}\\{Resources.Text.Environments.FileNameLogConfig}";
            if (!System.IO.File.Exists(logConfigFilePath))
            {
                // if no log_conf.toml, create with default value
                var defaultLogConfig = new Types.Config.LogConfig();
                ConfigReadWriter.WriteLogConfig(defaultLogConfig);
            }

            // read log config file
            var logConfig = ConfigReadWriter.ReadLogConfig();
            var logPath = config.AppSettings.Settings["REP_DIR"].Value;
            var normalLogLevelMax = logConfig.NormalLog.LogLevelMax;
            var normalLogLevelMin = logConfig.NormalLog.LogLevelMin;
            var errorLogLevelMax = logConfig.ErrorLog.LogLevelMax;
            var errorLogLevelMin = logConfig.ErrorLog.LogLevelMin;

            // create log4net xml config
            var log4netConfigXml = string.Format(Resources.Text.Components.StringLog4NetConfigXml, logPath, normalLogLevelMax, normalLogLevelMin, errorLogLevelMax, errorLogLevelMin);

            // configure log4net
            using (var stream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(log4netConfigXml)))
            {
                XmlConfigurator.Configure(stream);
            }
        }

        #region log

        /// <summary>
        /// output debug log
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="methodName">method name called this method</param>
        /// <param name="sourceFilePath">source file path of method</param>
        /// <param name="sourceLineNumber">souce line of method</param>
        internal void TraceDebug(string message,
            [System.Runtime.CompilerServices.CallerMemberName] string methodName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            // get class name from file path
            string className = System.IO.Path.GetFileName(sourceFilePath).Replace(Resources.Text.Components.StringCSFileExtension, string.Empty);

            // create tag for log
            var logTag = string.Format(Resources.Text.Formats.LogTag, className, methodName, sourceLineNumber);

            // format and output debug log
            this._logger.Debug(string.Format(Resources.Text.Formats.Log, logTag, message));
        }

        /// <summary>
        /// output info log
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="methodName">method name called this method</param>
        /// <param name="sourceFilePath">source file path of method</param>
        /// <param name="sourceLineNumber">souce line of method</param>
        internal void TraceInformation(string message,
            [System.Runtime.CompilerServices.CallerMemberName] string methodName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            // get class name from file path
            string className = System.IO.Path.GetFileName(sourceFilePath).Replace(Resources.Text.Components.StringCSFileExtension, string.Empty);

            // create tag for log
            var logTag = string.Format(Resources.Text.Formats.LogTag, className, methodName, sourceLineNumber);

            // format and output info log
            this._logger.Info(string.Format(Resources.Text.Formats.Log, logTag, message));
        }

        /// <summary>
        /// output warn log
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="methodName">method name called this method</param>
        /// <param name="sourceFilePath">source file path of method</param>
        /// <param name="sourceLineNumber">souce line of method</param>
        internal void TraceWarning(string message,
            [System.Runtime.CompilerServices.CallerMemberName] string methodName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            // get class name from file path
            string className = System.IO.Path.GetFileName(sourceFilePath).Replace(Resources.Text.Components.StringCSFileExtension, string.Empty);

            // create tag for log
            var logTag = string.Format(Resources.Text.Formats.LogTag, className, methodName, sourceLineNumber);

            // format and output info log
            this._logger.Warn(string.Format(Resources.Text.Formats.Log, logTag, message));
        }

        /// <summary>
        /// output error log
        /// </summary>
        /// <param name="logTag">tag for log</param>
        /// <param name="message">message</param>
        /// <param name="exception">exception</param>
        internal void TraceError(string logTag, string message, Exception exception)
        {
            // object for creating error log message
            var errorMessage = new System.Text.StringBuilder();

            // format error log message
            errorMessage.Append(string.Format(Resources.Text.Formats.Log, logTag, message));

            if (exception == null)
            {
                // if exception is null, do not output stack trace, just output error log
                this._logger.Error(errorMessage.ToString());
            }
            else
            {
                // insert blank line between error log message and stacktrace
                errorMessage.AppendLine(String.Empty);

                // insert stacktrace
                errorMessage.AppendLine(exception.ToString());

                if (exception.InnerException == null)
                {
                    // if inner exception is null, do not output inner exception, output error log
                    this._logger.Error(errorMessage.ToString());
                }
                else
                {
                    // insert stacktrace of inner exception
                    errorMessage.AppendLine(GetInnnerExceptions(exception.InnerException));

                    // output error log
                    this._logger.Error(errorMessage.ToString());
                }
            }
        }

        /// <summary>
        /// output fatal log
        /// </summary>
        /// <param name="logTag">tag for log</param>
        /// <param name="message">message</param>
        /// <param name="exception">exception</param>
        internal void TraceFatal(string logTag, string message, Exception exception)
        {
            // object for creating fatal log message
            var fatalMessage = new System.Text.StringBuilder();

            // format fatal log message
            fatalMessage.Append(string.Format(Resources.Text.Formats.Log, logTag, message));

            if (exception == null)
            {
                // if exception is null, do not output stack trace, just output fatal log
                this._logger.Fatal(fatalMessage.ToString());
            }
            else
            {
                // insert blank line between fatal log message and stacktrace
                fatalMessage.AppendLine(String.Empty);

                // insert stacktrace
                fatalMessage.AppendLine(exception.ToString());

                if (exception.InnerException == null)
                {
                    // if inner exception is null, do not output inner exception, output fatal log
                    this._logger.Fatal(fatalMessage.ToString());
                }
                else
                {
                    // insert stacktrace of inner exception
                    fatalMessage.AppendLine(GetInnnerExceptions(exception.InnerException));

                    // output fatal log
                    this._logger.Fatal(fatalMessage.ToString());
                }
            }
        }

        #endregion

        /// <summary>
        /// remove all empty error log
        /// </summary>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal void DeleteAllEmptyErrorLogFiles()
        {
            // start log
            Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {

                // get app config
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                // get error log dir path
                var errorLogFilePath = $"{config.AppSettings.Settings["REP_DIR"].Value}\\{Resources.Text.Environments.DirNameLog}\\{Resources.Text.Environments.DirNameErrorLog}";

                // get list of error log files
                List<System.IO.FileInfo> errorLogFiles = new List<System.IO.FileInfo>();
                errorLogFiles.AddRange(new System.IO.DirectoryInfo(errorLogFilePath).GetFiles());

                // for each error log files
                foreach (var errorLogFile in errorLogFiles)
                {
                    if (IsLocked(errorLogFile))
                    {
                        // if log file is locked, skip
                        continue;
                    }

                    if (errorLogFile.Length <= 0)
                    {
                        // if log file is empty remove
                        errorLogFile.Delete();
                    }
                }
            }
            catch (Exception ex)
            {
                // if exception occured, throw unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Instance.TraceDebug(Resources.Text.Components.TextLogEnd);

            }
        }

        /// <summary>
        /// foldering old log files
        /// </summary>
        /// <exception cref="Exceptions.UnexpectedException">unexpected exception</exception>
        internal void FolderingOldLogFiles()
        {
            // start log
            Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // get app config
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                // get log dir path
                var logFilesPath = $"{config.AppSettings.Settings["REP_DIR"].Value}\\{Resources.Text.Environments.DirNameLog}";

                // get log files created before last month
                var oldLogFiles = new System.IO.DirectoryInfo(logFilesPath).GetFiles()
                    .Where(oldLogFile => Int32.Parse(oldLogFile.LastWriteTime.ToString("yyyyMM")) < Int32.Parse(DateTime.Now.ToString("yyyyMM"))).ToList();

                // for each log files
                oldLogFiles.ForEach(oldLogFile =>
                {
                    // get dir path of destination
                    var oldLogFileDestPath = $"{logFilesPath}\\{oldLogFile.LastWriteTime.ToString("yyyy")}\\{oldLogFile.LastWriteTime.ToString("MM")}";

                    if (!System.IO.Directory.Exists(oldLogFileDestPath))
                    {
                        // if no old log files dir, creat old log files dir
                        System.IO.Directory.CreateDirectory(oldLogFileDestPath);
                    }

                    // move old log files
                    oldLogFile.MoveTo($"{oldLogFileDestPath}\\{oldLogFile.Name}");
                });

                // get error log dir path
                var errorLogFilePath = $"{logFilesPath}\\{Resources.Text.Environments.DirNameErrorLog}";
                Instance.TraceInformation(errorLogFilePath);

                // get error log files created before last month
                var oldErrorLogFiles = new System.IO.DirectoryInfo(errorLogFilePath).GetFiles()
                    .Where(oldErrorLogFile => Int32.Parse(oldErrorLogFile.LastWriteTime.ToString("yyyyMM")) < Int32.Parse(DateTime.Now.ToString("yyyyMM"))).ToList();

                oldErrorLogFiles.ForEach(oldErrorLogFile =>
                {
                    // get dir path of destination
                    var oldErrorLogFilDestPath = $"{errorLogFilePath}\\{oldErrorLogFile.LastWriteTime.ToString("yyyy")}\\{oldErrorLogFile.LastWriteTime.ToString("MM")}";
                    Instance.TraceInformation(oldErrorLogFilDestPath);
                    if (!System.IO.Directory.Exists(oldErrorLogFilDestPath))
                    {
                        // if no old error log files dir, creat old error log files dir
                        System.IO.Directory.CreateDirectory(oldErrorLogFilDestPath);
                    }

                    // move old error log files
                    oldErrorLogFile.MoveTo($"{oldErrorLogFilDestPath}\\{oldErrorLogFile.Name}");
                });
            }
            catch (Exception ex)
            {
                // if exception occured, throw unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        #endregion

        #region private methods

        /// <summary>
        /// get inner exceptions
        /// </summary>
        /// <param name="exception">exception</param>
        /// <returns>InnerExeption</returns>
        private static string GetInnnerExceptions(Exception exception)
        {
            // start log
            Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            // object for creating inner exception log message 
            var innerLogMessage = new System.Text.StringBuilder();

            try
            {
                // insert header for inner exception
                innerLogMessage.AppendLine(Resources.Text.Components.TextLogInnerExeptionHeader);

                // get inner exception up to 10
                for (int depth = 0; depth < 10; depth++)
                {
                    if (exception.InnerException == null)
                    {
                        // if inner exception is null, end
                        break;
                    }
                    else
                    {
                        // insert stack trace of inner exception
                        innerLogMessage.AppendLine(GetInnnerExceptions(exception.InnerException));
                    }
                }
            }
            catch (Exception ex)
            {
                // if exception occured, throw unexpected exception
                throw new Exceptions.UnexpectedException(ex.Message, ex);
            }
            finally
            {
                // end log
                Instance.TraceDebug(Resources.Text.Components.TextLogEnd);

            }

            return innerLogMessage.ToString();
        }

        /// <summary>
        /// get status the file is lockd or not
        /// </summary>
        /// <param name="file">file</param>
        /// <returns>True : is locked / False : is not locked</returns>
        private static bool IsLocked(System.IO.FileInfo file)
        {
            // start log
            Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                using (var stream = new System.IO.FileStream(file.FullName, System.IO.FileMode.Open))
                {
                    // is not locked
                    return false;
                }
            }
            catch
            {
                // is locked
                return true;
            }
            finally
            {
                // end log
                Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        #endregion
    }
}