using System;
using System.Collections.Generic;
using System.Linq;

namespace rep.Libs.Database.IO
{
    /// <summary>
    /// utility class
    /// </summary>
    internal static class DBReadWriter
    {
        #region methods

        /// <summary>
        /// GetLastOpening
        /// </summary>
        /// <returns>lastOpening</returns>
        /// <exception cref="Exceptions.ReadSqliteDBFileException">ReadSqliteDBFileException</exception>
        internal static Models.DAILY GetLastOpening()
        {

            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // get db context
                var context = new RepContext();

                // get last opening from sqlite db
                var openings = context.DAILY
                    .Where
                    (
                        openings =>
                            openings.DAILY_TYPE.Equals((int)Types.Enums.DailyType.Opening)
                    );

                var lastOpening = openings
                    .FirstOrDefault
                    (
                        opening =>
                            opening.SEND_TIME.Equals(openings.Max(max => max.SEND_TIME))
                    );

                // if no opening, return empty opening
                return lastOpening == null ? new Models.DAILY() : lastOpening;
            }
            catch (Exception ex)
            {
                // if exception occured, throw read sqlite db file exception
                throw new Exceptions.ReadSqliteDBFileException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// GetLastClosing
        /// </summary>
        /// <returns>lastClosing</returns>
        /// <exception cref="Exceptions.ReadSqliteDBFileException">ReadSqliteDBFileException</exception>
        internal static Models.DAILY GetLastClosing()
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // get db context
                var context = new RepContext();

                // get last closing from sqlite db
                var closings = context.DAILY
                    .Where
                    (
                        closings =>
                            closings.DAILY_TYPE.Equals((int)Types.Enums.DailyType.Closing)
                    );

                var lastClosing = closings
                    .FirstOrDefault
                    (
                        closing =>
                            closing.SEND_TIME.Equals(closings.Max(max => max.SEND_TIME))
                    );

                // if no closing, return empty closing
                return lastClosing == null ? new Models.DAILY() : lastClosing;
            }
            catch (Exception ex)
            {
                // if exception occured, throw read sqlite db file exception
                throw new Exceptions.ReadSqliteDBFileException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// GetLastWeekly
        /// </summary>
        /// <returns>lastWeekly</returns>
        /// <exception cref="Exceptions.ReadSqliteDBFileException">ReadSqliteDBFileException</exception>
        internal static Models.WEEKLY GetLastWeekly()
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // get db context
                var context = new RepContext();

                // get last weekly from sqlite db
                var lastWeekly = context.WEEKLY.AsQueryable().
                    FirstOrDefault
                    (
                        weekly =>
                            weekly.SEND_TIME.Equals(context.WEEKLY.Max(max => max.SEND_TIME))
                    );


                // if no weekly, return empty weekly
                return lastWeekly == null ? new Models.WEEKLY() : lastWeekly;
            }
            catch (Exception ex)
            {
                // if exception occured, throw read sqlite db file exception
                throw new Exceptions.ReadSqliteDBFileException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// GetLastMonthly
        /// </summary>
        /// <returns>lastMonthly</returns>
        /// <exception cref="Exceptions.ReadSqliteDBFileException">ReadSqliteDBFileException</exception>
        internal static Models.MONTHLY GetLastMonthly()
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // get db context
                var context = new RepContext();

                // get last monthly from sqlite db
                var lastMonthly = context.MONTHLY.AsQueryable().
                    FirstOrDefault
                    (
                        monthly =>
                            monthly.SEND_TIME.Equals(context.MONTHLY.Max(max => max.SEND_TIME))
                    );


                // if no monthly, return empty monthly
                return lastMonthly == null ? new Models.MONTHLY() : lastMonthly;
            }
            catch (Exception ex)
            {
                // if exception occured, throw read sqlite db file exception
                throw new Exceptions.ReadSqliteDBFileException(ex.Message, ex);
            }
            finally
            {
                // end log
                Logger.Instance.TraceDebug(Resources.Text.Components.TextLogEnd);
            }
        }

        /// <summary>
        /// WriteReport
        /// </summary>
        /// <param name="report">report</param>
        /// <exception cref="Exceptions.WriteSqliteDBFileException">WriteSqliteDBFileException</exception>
        internal static void WriteReport(Models.Interfaces.IReport report, List<string> reportFilePaths)
        {
            // start log
            Logger.Instance.TraceDebug(Resources.Text.Components.TextLogStart);

            try
            {
                // get db context
                var context = new RepContext();

                if (report is Models.DAILY daily)
                {
                    // if weekly, write daily
                    context.DAILY.Add(daily);
                }
                else if (report is Models.WEEKLY weekly)
                {
                    // if weekly, declare var for weekly records
                    var weeklyList = new List<Models.WEEKLY>();

                    // on a per-report-file basis
                    foreach (var reportFilePath in reportFilePaths)
                    {
                        // create weekly record
                        var weeklyWithFile = weekly;

                        // set weekly file
                        weeklyWithFile.REPORT_FILE = System.IO.File.ReadAllBytes(reportFilePath);

                        // add weekly records list
                        weeklyList.Add(weeklyWithFile);
                    }

                    // write weekly
                    context.WEEKLY.AddRange(weeklyList);
                }
                else if (report is Models.MONTHLY monthly)
                {
                    // if monthly, declare var for monthly records
                    var monthlyList = new List<Models.MONTHLY>();

                    // on a per-report-file basis
                    foreach (var reportFilePath in reportFilePaths)
                    {
                        // create monthly record
                        var monthlyWithFile = monthly;

                        // set monthly file
                        monthlyWithFile.REPORT_FILE = System.IO.File.ReadAllBytes(reportFilePath);

                        // add monthly records list
                        monthlyList.Add(monthlyWithFile);
                    }

                    // write monthly
                    context.MONTHLY.AddRange(monthlyList);
                }

                // commit changes
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                // if exception occured, throw write sqlite db file exception
                throw new Exceptions.WriteSqliteDBFileException(ex.Message, ex);
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
