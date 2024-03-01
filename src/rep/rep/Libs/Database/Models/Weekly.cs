using System;

namespace rep.Libs.Database.Models
{
    /// <summary>
    /// monthly
    /// </summary>
    [Microsoft.EntityFrameworkCore.PrimaryKey(nameof(SEND_TIME), nameof(REPORT_FILE))]
    public class WEEKLY : Interfaces.IReport
    {
        /// <summary>
        /// send_time
        /// </summary>
        public DateTime SEND_TIME { get; set; }

        /// <summary>
        /// report_file
        /// </summary>
        public byte[] REPORT_FILE { get; set; }

        /// <summary>
        /// is_succeeded (1 : succeeded / 0 : failed)
        /// </summary>
        public int IS_SUCCEEDED { get; set; }

        /// <summary>
        /// from
        /// </summary>
        public string? FROM { get; set; }

        /// <summary>
        /// to
        /// </summary>
        public string? TO { get; set; }

        /// <summary>
        /// cc
        /// </summary>
        public string? CC { get; set; }

        /// <summary>
        /// bcc
        /// </summary>
        public string? BCC { get; set; }

        /// <summary>
        /// subject
        /// </summary>
        public string? SUBJECT { get; set; }

        /// <summary>
        /// head
        /// </summary>
        public string? HEAD { get; set; }

        /// <summary>
        /// body
        /// </summary>
        public string? BODY { get; set; }

        /// <summary>
        /// foot
        /// </summary>
        public string? FOOT { get; set; }
    }
}
