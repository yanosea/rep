using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace rep.Libs.Database
{
    /// <summary>
    /// RepContext
    /// </summary>
    public class RepContext : DbContext
    {
        #region rep sqlite tables

        /// <summary>
        /// daily
        /// </summary>
        public DbSet<Models.DAILY> DAILY { get; set; }

        /// <summary>
        /// weekly
        /// </summary>
        public DbSet<Models.WEEKLY> WEEKLY { get; set; }

        /// <summary>
        /// monthly
        /// </summary>
        public DbSet<Models.MONTHLY> MONTHLY { get; set; }

        #endregion

        #region methods

        /// <summary>
        /// OnConfigure
        /// </summary>
        /// <param name="options">options</param>
        protected override void OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder options)
        {
            // get app config
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            // set sqlite db file
            options.UseSqlite($"Data Source={config.AppSettings.Settings["REP_DIR"].Value}\\{Resources.Text.Environments.DirNameReport}\\{Resources.Text.Environments.FileNameSQLiteDB}");
        }

        #endregion
    }
}
