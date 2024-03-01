using Microsoft.AspNetCore.Components.Forms;
using rep.Libs.IO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace rep.Libs
{
    /// <summary>
    /// utility class for frontend
    /// </summary>
    internal static class FrontendUtility
    {
        #region methods

        /// <summary>
        /// CheckUpdates
        /// </summary>
        /// <param name="currentVersion">currentVersion</param>
        /// <return>true / false (true : has update / false : has no update), currentVersion, latestVersion</return>
        /// <exception cref="Exceptions.FrontendUtilException">FrontendUtilException</exception>
        internal static Tuple<bool, string, string> CheckUpdates(string currentVersion)
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // get latest version
                var latestVersion = GetLatestVersion();

                // info log
                Logger.Instance.TraceInformation(string.Format(Resources.Text.Formats.LogLatestVersion, latestVersion));

                if (string.IsNullOrEmpty(latestVersion))
                {
                    // if failed getting latest version, return false
                    return new Tuple<bool, string, string>(false, currentVersion, latestVersion);
                }

                if (latestVersion!.Contains(currentVersion))
                {
                    // if latest version not contains current version, return false
                    return new Tuple<bool, string, string>(false, currentVersion, latestVersion);
                }
                else
                {
                    // if there is differrence between current version and latest version, check old or new
                    var currentVersionNumber = Int32.Parse(currentVersion.Replace(".", string.Empty));
                    var latestVersionNumber = Int32.Parse(latestVersion.Replace("v", string.Empty).Replace(".", string.Empty));

                    if (currentVersionNumber < latestVersionNumber)
                    {
                        // if latest version is greater than current version, return true
                        return new Tuple<bool, string, string>(true, currentVersion, latestVersion);
                    }
                    else
                    {
                        // return false
                        return new Tuple<bool, string, string>(false, currentVersion, latestVersion);
                    }
                }
            }
            catch (Exceptions.GitCommandException)
            {
                // if git command exception occured, throw;
                throw;
            }
            catch (Exception ex)
            {
                // fatal log
                Logger.Instance.TraceFatal(Resources.Text.Components.TextLogTagBackendFatal, Resources.Text.LogMessages.FailureSomethingWrongOccured, ex);

                // if exception occured, throw frontend util exception
                throw new Exceptions.FrontendUtilException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// ExecuteUpdate
        /// </summary>
        /// <exception cref="Exceptions.FrontendUtilException">FrontendUtilException</exception>
        internal static void ExecuteUpdate()
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // get installer path
                var localRepositoryPath = $"{System.IO.Path.GetTempPath()}{Resources.Text.Environments.DirNameGit}";
                var installerPath = $"{localRepositoryPath}\\{Resources.Text.Environments.DirNameRepInstaller}\\{Resources.Text.Environments.FileNameRepInstaller}";

                // execute latest installer and terminate rep
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo() { FileName = installerPath, UseShellExecute = true });
                Environment.Exit(1);
            }
            catch (Exception ex)
            {
                // fatal log
                Logger.Instance.TraceFatal(Resources.Text.Components.TextLogTagFrontendFatal, Resources.Text.LogMessages.FailureSomethingWrongOccured, ex);

                // if exception occured, throw frontend util exception
                throw new Exceptions.FrontendUtilException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// GetWorkTime
        /// </summary>
        /// <param name="frontFrom">frontFrom</param>
        /// <param name="frontTo">frontTo</param>
        /// <param name="timeFormat">timeFormat</param>
        /// <exception cref="Exceptions.AbnormalInputException">abnormal input  exception</exception>
        /// <exception cref="OverflowException">overflow exception</exception>
        internal static string GetWorkTime(DateTime? frontFrom, DateTime? frontTo, string timeFormat)
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // declare vars for time
                DateTime from;
                DateTime to;

                if (!frontFrom.HasValue)
                {
                    // if failed to parse from, throw abnormal input exception
                    throw new Exceptions.AbnormalInputException(nameof(from), new Exception());
                }

                if (!frontTo.HasValue)
                {
                    // if failed to parse to, throw abnormal input exception
                    throw new Exceptions.AbnormalInputException(nameof(to), new Exception());
                }

                // set value
                from = frontFrom.Value;
                to = frontTo.Value;

                if (to <= from)
                {
                    // if incorrect order, throw abnormal input exception
                    throw new Exceptions.AbnormalInputException($"{nameof(from)}, {nameof(to)}", new Exception());
                }

                // calk work time
                var workTime = to - from;

                if (workTime > new TimeSpan(8, 0, 0))
                {
                    // if work time more than 8 hours, substruct one hour for break time
                    workTime = workTime - new TimeSpan(1, 0, 0);
                }

                // extract the integer part of the work time in hours
                int workTimeHourIntegerPart = Convert.ToInt32(Math.Floor(workTime.TotalHours));

                // extract a small portion of the work time in hours
                double tmpWorkTimeHourAfterDecimalPoint = workTime.TotalHours % 1;

                // multiply by 10 until integer
                while (tmpWorkTimeHourAfterDecimalPoint != 0 && tmpWorkTimeHourAfterDecimalPoint % 1 != 0)
                {
                    tmpWorkTimeHourAfterDecimalPoint *= 10;
                }

                // stores the decimal part as an integer
                int workTimeHourAfterDecimalPoint = Convert.ToInt32(Math.Round(tmpWorkTimeHourAfterDecimalPoint));

                if (workTimeHourAfterDecimalPoint.ToString().Length <= 1)
                {
                    // if less than 1 digit, multiply by 10
                    workTimeHourAfterDecimalPoint *= 10;
                }

                // return time
                return timeFormat.Replace("%from%", from.ToString("HH:mm"))
                    .Replace("%to%", to.ToString("HH:mm"))
                    .Replace("%worktime%", $"{workTimeHourIntegerPart:D2}.{workTimeHourAfterDecimalPoint:D2}");
            }
            catch (Exceptions.AbnormalInputException ex)
            {
                // warn log
                Logger.Instance.TraceWarning(ex.FormattedMessage);

                // if abnormal exception occured, throw
                throw;
            }
            catch (OverflowException ex)
            {
                // warn log
                Logger.Instance.TraceWarning(Resources.Text.LogMessages.FailureInputtingAbnormalTime);

                // if overflow exception occured, throw
                throw new Exceptions.AbnormalInputException(Resources.Text.Messages.FailureInputtingAbnormalTime, ex);
            }
            catch (Exception ex)
            {
                // fatal log
                Logger.Instance.TraceFatal(Resources.Text.Components.TextLogTagBackendFatal, Resources.Text.LogMessages.FailureSomethingWrongOccured, ex);

                // if exception occrued, throw
                throw;
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// BindParams
        /// </summary>
        /// <param name="prop">prop</param>
        /// <returns>bindedParams</returns>
        /// <exception cref="Exceptions.FrontendUtilException">frontend util exception</exception>
        internal static string BindParams(string prop)
        {
            //  start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // firstname
                prop = prop.Replace("%firstname%", DataHolder.Instance.GetPreferences().User.FirstName);

                // lastname
                prop = prop.Replace("%lastname%", DataHolder.Instance.GetPreferences().User.LastName);

                // company
                prop = prop.Replace("%company%", DataHolder.Instance.GetPreferences().User.Company);

                // projectname
                prop = prop.Replace("%projectname%", DataHolder.Instance.GetPreferences().User.ProjectName);

                // today
                prop = prop.Replace("%today%", DateTime.Today.ToString("yyyy/MM/dd"));

                // thismonth
                prop = prop.Replace("%thismonth%", DateTime.Today.ToString("MMM"));

                return prop;
            }
            catch (Exception ex)
            {
                // fatal log
                Logger.Instance.TraceFatal(Resources.Text.Components.TextLogTagFrontendFatal, Resources.Text.LogMessages.FailureSomethingWrongOccured, ex);

                // if exception occured, throw frontend util exception
                throw new Exceptions.FrontendUtilException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// DestsToDestsString
        /// </summary>
        /// <param name="dests">dests</param>
        /// <returns>destsString</returns>
        /// <exception cref="Exceptions.UnexpectedException">frontend util exception</exception>
        internal static string DestsToDestsString(List<Types.Config.Props.Dests.Interfaces.IDest> dests)
        {
            //  start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // declare var for dests string
                string? destsString = null;

                dests.ForEach(dest =>
                {
                    // add to var in the format "name/mailaddress"
                    destsString += $"{dest.Name}/{dest.Mailaddress}";

                    if (dest != dests.Last())
                    {
                        // if not last, add comma
                        destsString += ",";
                    }
                });

                // return destsString
                return destsString;
            }
            catch (Exception ex)
            {
                // fatal log
                Logger.Instance.TraceFatal(Resources.Text.Components.TextLogTagFrontendFatal, Resources.Text.LogMessages.FailureSomethingWrongOccured, ex);

                // if exception occured, throw frontend util exception
                throw new Exceptions.FrontendUtilException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// DestsStringToDests
        /// </summary>
        /// <param name="destsString">destsString</param>
        /// <returns>dests</returns>
        /// <exception cref="Exceptions.UnexpectedException">frontend util exception</exception>
        internal static List<Type> DestsStringToDests<Type>(string destsString) where Type : Types.Config.Props.Dests.Interfaces.IDest, new()
        {
            //  start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // declare var for dests
                var dests = new List<Type>();

                if (!string.IsNullOrEmpty(destsString))
                {
                    // if not dests string null, split the input string by commas
                    var destTokens = destsString.Split(',');

                    foreach (string destToken in destTokens)
                    {
                        // split each dest token by "/" to separate name and mailaddress
                        var parts = destToken.Split('/');

                        // add dest to list
                        dests.Add(new Type
                        {
                            Name = parts[0],
                            Mailaddress = parts[1]
                        });
                    }
                }

                // return dests
                return dests;
            }
            catch (Exception ex)
            {
                // fatal log
                Logger.Instance.TraceFatal(Resources.Text.Components.TextLogTagFrontendFatal, Resources.Text.LogMessages.FailureSomethingWrongOccured, ex);

                // if exception occured, throw frontend util exception
                throw new Exceptions.FrontendUtilException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// CacheAttachedFiles
        /// </summary>
        /// <param name="attachedFiles">attachedFiles</param>
        /// <param name="addDateToAttachedFileConfig">addDateToAttachedFileConfig</param>
        /// <returns></returns>
        internal static async Task<List<string>> CacheAttachedFiles(List<IBrowserFile> attachedFiles, bool addDateToAttachedFileConfig)
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // declare var for cached file paths
                var cachedFilePaths = new List<string>();

                // get app config
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                foreach (var attachedFile in attachedFiles)
                {
                    // declare var for path for temp files
                    var cacheFilesDirectory = $"{config.AppSettings.Settings["REP_DIR"].Value}\\{Resources.Text.Environments.DirNameCachedFiles}";

                    // declare var for cache file name
                    var cacheFileName = attachedFile.Name;

                    if (addDateToAttachedFileConfig)
                    {
                        // if config enabled, add file name to var with date
                        cacheFileName = ($"{System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetFileName(attachedFile.Name))}_{DateTime.Now.ToString("yyyyMMdd")}{System.IO.Path.GetExtension(System.IO.Path.GetFileName(attachedFile.Name))}");
                    }

                    // check cache file name
                    while (System.IO.File.Exists($"{cacheFilesDirectory}\\{cacheFileName}"))
                    {
                        // if same name file exist, add "_"
                        cacheFileName = $"_{cacheFileName}";
                    }

                    using (var fileStream = new System.IO.FileStream($"{cacheFilesDirectory}\\{cacheFileName}", System.IO.FileMode.Create))
                    {
                        // create cache
                        await attachedFile.OpenReadStream().CopyToAsync(fileStream);
                    }

                    // add cached attached file path to list
                    cachedFilePaths.Add(($"{cacheFilesDirectory}\\{cacheFileName}"));
                }

                // return cached file paths
                return cachedFilePaths;
            }
            catch (Exception ex)
            {
                // fatal log
                Logger.Instance.TraceFatal(Resources.Text.Components.TextLogTagFrontendFatal, Resources.Text.LogMessages.FailureSomethingWrongOccured, ex);

                // if exception occured, throw backend util exception
                throw new Exceptions.FrontendUtilException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// DeleteCachedFiles
        /// </summary>
        /// <param name="cachedFilePaths">cachedFilePaths</param>
        internal static void DeleteCachedFiles(List<string> cachedFilePaths)
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // on a-cached-file-path basis
                foreach (var cachedFilePath in cachedFilePaths)
                {
                    // delete cached file
                    System.IO.File.Delete(cachedFilePath);
                }
            }
            catch (Exception ex)
            {
                // fatal log
                Logger.Instance.TraceFatal(Resources.Text.Components.TextLogTagFrontendFatal, Resources.Text.LogMessages.FailureSomethingWrongOccured, ex);

                // if exception occured, throw backend util exception
                throw new Exceptions.FrontendUtilException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// BuildConfirmDialogMessage
        /// </summary>
        /// <param name="dailySubject">dailySubject</param>
        /// <param name="weeklySubject">weeklySubject</param>
        /// <param name="monthlySubject">monthlySubject</param>
        /// <returns>confirm dialog message</returns>
        /// <exception cref="Exceptions.FrontendUtilException">FrontendUtilException</exception>
        internal static string BuildConfirmDialogMessage(string dailySubject, string weeklySubject, string monthlySubject)
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // declare var for confirm dialog message
                var confirmDialogMessage = new System.Text.StringBuilder();

                // build confirm message
                if (dailySubject != null)
                {
                    confirmDialogMessage.AppendLine($"[ {dailySubject} ]");
                }

                if (weeklySubject != null)
                {
                    confirmDialogMessage.AppendLine($"[ {weeklySubject} ]");
                }

                if (monthlySubject != null)
                {
                    confirmDialogMessage.AppendLine($"[ {monthlySubject} ]");
                }

                // return confirm dialog message
                return confirmDialogMessage.ToString();
            }
            catch (Exception ex)
            {
                // fatal log
                Logger.Instance.TraceFatal(Resources.Text.Components.TextLogTagFrontendFatal, Resources.Text.LogMessages.FailureSomethingWrongOccured, ex);

                // if exception occured, throw backend util exception
                throw new Exceptions.FrontendUtilException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// SaveFiles
        /// </summary>
        /// <param name="reports">reports</param>
        /// <param name="saveTextsConfig">saveTextsConfig</param>
        /// <param name="saveDestsConfig">saveDestsConfig</param>
        /// <exception cref="Exceptions.FrontendUtilException">FrontendUtilException</exception>
        internal static void SaveFiles(Dictionary<Database.Models.Interfaces.IReport,
            Tuple<List<Types.Config.Props.Dests.To>, List<Types.Config.Props.Dests.Cc>, List<Types.Config.Props.Dests.Bcc>, List<string>>>
            reports, bool saveTextsConfig, bool saveDestsConfig)
        {
            //  start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // on a per-report basis
                foreach (var report in reports)
                {
                    // write sqlite db file
                    Database.IO.DBReadWriter.WriteReport(report.Key, report.Value.Item4);
                }

                // on a per-report-file basis
                foreach (var report in reports)
                {
                    if (report.Value.Item4 == null || report.Value.Item4.Count == 0)
                    {
                        // if no report files exist, skip
                        continue;
                    }

                    // move cached attached files to report directory
                    MoveCachedFiles(report.Key, report.Value.Item4);
                }

                if (saveTextsConfig)
                {
                    // if config enabled, on a-report basis
                    foreach (var report in reports)
                    {
                        // save report to text
                        SaveText(report.Key);
                    }
                }

                if (saveDestsConfig)
                {
                    // if config enabled, on a-report basis
                    foreach (var report in reports)
                    {
                        // save dest config
                        SaveDests(report.Key, report.Value.Item1, report.Value.Item2, report.Value.Item3);
                    }
                }
            }
            catch (Exception ex)
            {
                // fatal log
                Logger.Instance.TraceFatal(Resources.Text.Components.TextLogTagFrontendFatal, Resources.Text.LogMessages.FailureSomethingWrongOccured, ex);

                // if exception occured, throw frontend util exception
                throw new Exceptions.FrontendUtilException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// BuildResultDialogMessage
        /// </summary>
        /// <param name="resultList">resultList</param>
        /// <returns>result dialog message</returns>
        /// <exception cref="Exceptions.FrontendUtilException">FrontendUtilException</exception>
        internal static string BuildResultDialogMessage(Dictionary<Database.Models.Interfaces.IReport, Tuple<DateTime, int>> resultList)
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // declare var for result dialog message
                var resultDialogMessage = new System.Text.StringBuilder();

                foreach (var result in resultList)
                {
                    if (result.Value.Item2 == 1)
                    {
                        resultDialogMessage.AppendLine(string.Format(Resources.Text.Messages.SuccessResultMessage, result.Key.SUBJECT));
                    }
                    else
                    {
                        resultDialogMessage.AppendLine(string.Format(Resources.Text.Messages.FailureResultMessage, result.Key.SUBJECT));
                    }

                }

                // return result dialog message
                return resultDialogMessage.ToString();
            }
            catch (Exception ex)
            {
                // fatal log
                Logger.Instance.TraceFatal(Resources.Text.Components.TextLogTagFrontendFatal, Resources.Text.LogMessages.FailureSomethingWrongOccured, ex);

                // if exception occured, throw backend util exception
                throw new Exceptions.FrontendUtilException(ex.Message, ex);
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
        /// GetLatestVersion
        /// </summary>
        /// <returns>latestVersion</returns>
        private static string GetLatestVersion()
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                var localRepositoryPath = $"{System.IO.Path.GetTempPath()}{Resources.Text.Environments.DirNameGit}";
                if (!System.IO.Directory.Exists($"{localRepositoryPath}\\{Resources.Text.Environments.DirNameGitHidden}"))
                {
                    var repositoryUrl = Resources.Text.Components.TextRepositoryUrlForCommand;

                    // if no rep local repo, clone
                    var cloneExitCode = ExecuteGitCommand(false, $"clone -b main {repositoryUrl} {localRepositoryPath}");

                    if (cloneExitCode != 0)
                    {
                        // if clone failed
                        // throw git command exception
                        throw new Exceptions.GitCommandException() { FailedCommandName = "clone" };
                    }
                }

                // execute checkout
                var checkoutExitCode = ExecuteGitCommand(true, $"-C {localRepositoryPath} checkout main");
                if (checkoutExitCode != 0)
                {
                    // if checkout failed
                    // throw git command exception
                    throw new Exceptions.GitCommandException() { FailedCommandName = "checkout" };
                }

                // execute fetch
                var fetchExitCode = ExecuteGitCommand(true, $"-C {localRepositoryPath} fetch origin main");
                if (fetchExitCode != 0)
                {
                    // if fetch failed
                    // throw git command exception
                    throw new Exceptions.GitCommandException() { FailedCommandName = "fetch" };
                }

                // execute reset
                var resetExitCode = ExecuteGitCommand(true, $"-C {localRepositoryPath} reset origin/main");
                if (resetExitCode != 0)
                {
                    // if resetfailed
                    // throw git command exception
                    throw new Exceptions.GitCommandException() { FailedCommandName = "reset" };
                }

                // execute restore
                var restoreExitCode = ExecuteGitCommand(true, $"-C {localRepositoryPath} restore .");
                if (restoreExitCode != 0)
                {
                    // if restore failed
                    // throw git command exception
                    throw new Exceptions.GitCommandException() { FailedCommandName = "restore" };
                }

                // execute clean
                var cleanExitCode = ExecuteGitCommand(true, $"-C {localRepositoryPath} clean -dfx");
                if (cleanExitCode != 0)
                {
                    // if clean failed
                    // throw git command exception
                    throw new Exceptions.GitCommandException() { FailedCommandName = "clean" };
                }

                // get installer path
                var installerPath = $"{localRepositoryPath}\\{Resources.Text.Environments.DirNameRepInstaller}\\{Resources.Text.Environments.FileNameRepInstaller}";

                // return latest version
                return System.Diagnostics.FileVersionInfo.GetVersionInfo(installerPath).ProductVersion!.TrimEnd();
            }
            catch (Exception ex)
            {
                // fatal log
                Logger.Instance.TraceFatal(Resources.Text.Components.TextLogTagFrontendFatal, Resources.Text.LogMessages.FailureSomethingWrongOccured, ex);

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
        /// ExecuteGitCommand
        /// </summary>
        /// <param name="arguments">arguments</param>
        /// <param name="isShown">isShown</param>
        /// <returns>exitCode</returns>
        /// <exception cref="Exceptions.FrontendUtilException">FrontendUtilException</exception>
        private static int ExecuteGitCommand(bool isShown, string arguments)
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // return git command exit code 
                return ExecuteCommand("git", isShown, arguments);
            }
            catch (Exception ex)
            {
                // fatal log
                Logger.Instance.TraceFatal(Resources.Text.Components.TextLogTagFrontendFatal, Resources.Text.LogMessages.FailureSomethingWrongOccured, ex);

                // if exception occured, throw frontend util exception
                throw new Exceptions.FrontendUtilException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// ExecuteCommand
        /// </summary>
        /// <param name="arguments">arguments</param>
        /// <param name="isShown">isShown</param>
        /// <returns>exitCode</returns>
        /// <exception cref="Exceptions.FrontendUtilException">FrontendUtilException</exception>
        private static int ExecuteCommand(string command, bool isShown, string arguments = "")
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // create process
                var process = new System.Diagnostics.Process
                {
                    StartInfo = new System.Diagnostics.ProcessStartInfo(command)
                    {
                        Arguments = arguments,
                        UseShellExecute = false,
                        CreateNoWindow = isShown,
                    }
                };

                // execute process
                process.Start();
                process.WaitForExit();

                // return exit code
                return process.ExitCode;
            }
            catch (Exception ex)
            {
                // fatal log
                Logger.Instance.TraceFatal(Resources.Text.Components.TextLogTagFrontendFatal, Resources.Text.LogMessages.FailureSomethingWrongOccured, ex);

                // if exception occured, throw frontend util exception
                throw new Exceptions.FrontendUtilException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// MoveCachedFiles
        /// </summary>
        /// <param name="report">report</param>
        /// <param name="cachedFilePaths">cachedFilePaths</param>
        /// <exception cref="Exceptions.FrontendUtilException">FrontendUtilException</exception>
        private static void MoveCachedFiles(Database.Models.Interfaces.IReport report, List<string> cachedFilePaths)
        {
            //  start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                if (cachedFilePaths == null || cachedFilePaths.Count == 0)
                {
                    // if no cached files, return
                    return;
                }

                // get app config
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                // get cached files path
                var reportPath = $"{config.AppSettings.Settings["REP_DIR"].Value}\\{Resources.Text.Environments.DirNameReport}";

                // declare var for path for dest directory name
                string destDirName = string.Empty;

                // check year directory
                destDirName = report is Database.Models.WEEKLY ?
                    $"{reportPath}\\{Resources.Text.Environments.DirNameWeekly}\\{DateTime.Now.ToString("yyyy")}" :
                    $"{reportPath}\\{Resources.Text.Environments.DirNameMonthly}\\{DateTime.Now.ToString("yyyy")}";

                if (!System.IO.Directory.Exists(destDirName))
                {
                    // if no directory exist, create
                    System.IO.Directory.CreateDirectory(destDirName);
                }

                // check month directory
                destDirName = $"{destDirName}\\{DateTime.Now.ToString("MM")}";

                if (!System.IO.Directory.Exists(destDirName))
                {
                    // if no directory exist, create
                    System.IO.Directory.CreateDirectory(destDirName);
                }

                // on a-cached-file basis
                foreach (var cachedFilePath in cachedFilePaths)
                {
                    // check moved file name
                    var movedFileName = System.IO.Path.GetFileName(cachedFilePath);
                    while (System.IO.File.Exists($"{destDirName}\\{movedFileName}"))
                    {
                        // if same name file exist, add "_"
                        movedFileName = $"_{movedFileName}";
                    }

                    System.IO.File.Move(cachedFilePath, $"{destDirName}\\{movedFileName}");
                }
            }
            catch (Exception ex)
            {
                // fatal log
                Logger.Instance.TraceFatal(Resources.Text.Components.TextLogTagFrontendFatal, Resources.Text.LogMessages.FailureSomethingWrongOccured, ex);

                // if exception occured, throw frontend util exception
                throw new Exceptions.FrontendUtilException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }


        /// <summary>
        /// SaveText
        /// </summary>
        /// <param name="report">report</param>
        /// <exception cref="Exceptions.FrontendUtilException">FrontendUtilException</exception>
        private static void SaveText(Database.Models.Interfaces.IReport report)
        {
            //  start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // get app config
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                // get report path
                var reportPath = $"{config.AppSettings.Settings["REP_DIR"].Value}\\{Resources.Text.Environments.DirNameReport}";

                // declare var for path for saving files
                string saveDirName = string.Empty;

                if (report is Database.Models.DAILY daily)
                {
                    // if daily, get daily report path
                    saveDirName = daily.DAILY_TYPE == 0 ?
                        $"{reportPath}\\{Resources.Text.Environments.DirNameDaily}\\{Resources.Text.Environments.DirNameOpening}" :
                        $"{reportPath}\\{Resources.Text.Environments.DirNameDaily}\\{Resources.Text.Environments.DirNameClosing}";
                }
                else if (report is Database.Models.WEEKLY)
                {
                    // if weekly, get weekly report path
                    saveDirName = $"{reportPath}\\{Resources.Text.Environments.DirNameWeekly}";
                }
                else if (report is Database.Models.MONTHLY)
                {
                    // if monthly, get monthly report path
                    saveDirName = $"{reportPath}\\{Resources.Text.Environments.DirNameMonthly}";
                }

                // check year directory
                saveDirName = $"{saveDirName}\\{DateTime.Now.ToString("yyyy")}";
                if (!System.IO.Directory.Exists(saveDirName))
                {
                    // if no directory exist, create
                    System.IO.Directory.CreateDirectory(saveDirName);
                }

                // check month directory
                saveDirName = $"{saveDirName}\\{DateTime.Now.ToString("MM")}";

                if (!System.IO.Directory.Exists(saveDirName))
                {
                    // if no directory exist, create
                    System.IO.Directory.CreateDirectory(saveDirName);
                }

                // declare var for file name
                var saveFileName = $"{DateTime.Now.ToString("yyyyMMdd")}.txt";

                while (System.IO.File.Exists($"{saveDirName}\\{saveFileName}"))
                {
                    // if file exist, add "_"
                    saveFileName = $"_{saveFileName}";
                }

                using (var streamWriter = new System.IO.StreamWriter($"{saveDirName}\\{saveFileName}"))
                {
                    // write head
                    streamWriter.WriteLine(report.HEAD);

                    if (report is Database.Models.DAILY _daily && _daily.DAILY_TYPE == 1)
                    {
                        // if report type is daily, add blank line between head and time
                        streamWriter.WriteLine(string.Empty);

                        // add time
                        streamWriter.WriteLine(_daily.TIME);
                    }

                    // add blank line between head (or time) and body
                    streamWriter.WriteLine(string.Empty);

                    // add body
                    streamWriter.WriteLine(report.BODY);

                    // add blank line between body and foot
                    streamWriter.WriteLine(string.Empty);

                    // add foot
                    streamWriter.WriteLine(report.FOOT);
                }
            }
            catch (Exception ex)
            {
                // fatal log
                Logger.Instance.TraceFatal(Resources.Text.Components.TextLogTagFrontendFatal, Resources.Text.LogMessages.FailureSomethingWrongOccured, ex);

                // if exception occured, throw frontend util exception
                throw new Exceptions.FrontendUtilException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// SaveText
        /// </summary>
        /// <param name="report">report</param>
        /// <param name="tos">tos</param>
        /// <param name="ccs">ccs</param>
        /// <param name="bccs">bccs</param>
        /// <exception cref="Exceptions.FrontendUtilException">FrontendUtilException</exception>
        private static void SaveDests(Database.Models.Interfaces.IReport report, List<Types.Config.Props.Dests.To> tos, List<Types.Config.Props.Dests.Cc> ccs, List<Types.Config.Props.Dests.Bcc> bccs)
        {
            //  start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                if (report is Database.Models.DAILY)
                {
                    // if daily, declare var for daily dest config
                    var dailyDestConfig = new Types.Config.Dests.DailyDestConfig()
                    {
                        To = tos,
                        Cc = ccs,
                        Bcc = bccs
                    };

                    // write daily dest config
                    ConfigReadWriter.WriteDailyDestConfig(dailyDestConfig);

                    // re-set daily dest config
                    DataHolder.Instance.SetDailyDestConfig(dailyDestConfig);
                }
                else if (report is Database.Models.WEEKLY)
                {
                    // if weekly, declare var for weekly dest config
                    var weeklyDestConfig = new Types.Config.Dests.WeeklyDestConfig()
                    {
                        To = tos,
                        Cc = ccs,
                        Bcc = bccs
                    };

                    // write weekly dest config
                    ConfigReadWriter.WriteWeeklyDestConfig(weeklyDestConfig);

                    // re-set weekly dest config
                    DataHolder.Instance.SetWeeklyDestConfig(weeklyDestConfig);
                }
                else if (report is Database.Models.MONTHLY)
                {
                    // if monthly, declare var for monthly dest config
                    var monthlyDestConfig = new Types.Config.Dests.MonthlyDestConfig()
                    {
                        To = tos,
                        Cc = ccs,
                        Bcc = bccs
                    };

                    // write monthly dest config
                    ConfigReadWriter.WriteMonthlyDestConfig(monthlyDestConfig);

                    // re-set monthly dest config
                    DataHolder.Instance.SetMonthlyDestConfig(monthlyDestConfig);
                }
            }
            catch (Exception ex)
            {
                // fatal log
                Logger.Instance.TraceFatal(Resources.Text.Components.TextLogTagFrontendFatal, Resources.Text.LogMessages.FailureSomethingWrongOccured, ex);

                // if exception occured, throw frontend util exception
                throw new Exceptions.FrontendUtilException(ex.Message, ex);
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