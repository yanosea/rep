using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace rep.Libs
{
    /// <summary>
    /// utility class for backend
    /// </summary>
    internal static class BackendUtility
    {
        #region methods

        /// <summary>
        /// GetCurrentVersion
        /// </summary>
        /// <returns>current version of rep</returns>
        /// <exception cref="Exceptions.BackendUtilException">backend util exception</exception>
        internal static string GetCurrentVersion()
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                return Assembly.GetEntryAssembly().GetCustomAttributes<AssemblyInformationalVersionAttribute>().FirstOrDefault().InformationalVersion.Split('+')[0];
            }
            catch (Exception ex)
            {
                // fatal log
                Logger.Instance.TraceFatal(Resources.Text.Components.TextLogTagBackendFatal, Resources.Text.LogMessages.FailureSomethingWrongOccured, ex);

                // if exception occured, throw backend util exception
                throw new Exceptions.BackendUtilException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// GetCommandLineArguments
        /// </summary>
        /// <returns>command line arguments as bool list</returns>
        /// <exception cref="Exceptions.BackendUtilException">backend util exception</exception>
        internal static List<bool> GetCommandLineArguments()
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // get command line arguments
                var args = Environment.GetCommandLineArgs().ToList();

                if (args.Count() != 1)
                {
                    // if command line args exist, remove first arg (that is dll path defalut)
                    args.RemoveAt(0);

                    if (args.Count() != 3 ||
                        args.Where(arg => !int.TryParse(arg, out _)).Count() != 0 ||
                        args.Where(arg => int.Parse(arg) == 0 || int.Parse(arg) == 1).Count() != 3)
                    {
                        // if non-3 command line arguments, or non-numeric arguments, or arguments that are not 0 or 1
                        // warn log
                        Logger.Instance.TraceWarning(Resources.Text.LogMessages.FailureGettingCommandLineArguments);

                        // return default command line arguments as bool list
                        return new List<bool>() { true, false, false };
                    }

                    // info log
                    Logger.Instance.TraceInformation(
                        string.Format(Resources.Text.Formats.LogCommandLineArguments, BuildCommandLineArgumentsStringForLog(args)));

                    // convert command line arguments to a list of type bool
                    var commandLineArgumentsList = new List<bool>();
                    args.ToList().ForEach(arg =>
                    {
                        commandLineArgumentsList.Add(Convert.ToBoolean(int.Parse(arg)));
                    });

                    // return command line arguments as bool list
                    return commandLineArgumentsList;
                }
                else
                {
                    // return default command line arguments as bool list
                    return new List<bool>() { true, false, false };
                }
            }
            catch (Exception ex)
            {
                // fatal log
                Logger.Instance.TraceFatal(Resources.Text.Components.TextLogTagBackendFatal, Resources.Text.LogMessages.FailureSomethingWrongOccured, ex);

                // if exception occured, throw backend util exception
                throw new Exceptions.BackendUtilException(ex.Message, ex);
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
        /// BuildCommandLineArgumentsStringForLog
        /// </summary>
        /// <param name="arguments">arguments</param>
        /// <returns>command line arguments string for log</returns>
        internal static string BuildCommandLineArgumentsStringForLog(List<string> arguments)
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // declare var for arguments string
                var commandLineArgumentsString = string.Empty;

                // on a per-command-line-argument basis
                foreach (var argument in arguments.Select((value, index) => new { value, index }))
                {
                    // add argument to var
                    commandLineArgumentsString += argument.value;

                    if (argument.index != arguments.Count - 1)
                    {
                        // if not last, add comma
                        commandLineArgumentsString += ", ";
                    }
                }

                // return command line arguments string
                return commandLineArgumentsString;
            }
            catch (Exception ex)
            {
                // fatal log
                Logger.Instance.TraceFatal(Resources.Text.Components.TextLogTagBackendFatal, Resources.Text.LogMessages.FailureSomethingWrongOccured, ex);

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

