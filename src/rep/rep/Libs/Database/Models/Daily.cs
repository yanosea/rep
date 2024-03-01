using System;

namespace rep.Libs.Database.Models
{
    /// <summary>
    /// daily
    /// </summary>
    public class DAILY : Interfaces.IReport
    {
        #region columns

        /// <summary>
        /// send_time
        /// </summary>
        [System.ComponentModel.DataAnnotations.Key]
        public DateTime SEND_TIME { get; set; }

        /// <summary>
        /// dailly_type
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required]
        public int DAILY_TYPE { get; set; }

        /// <summary>
        /// is_succeeded (1 : succeeded / 0 : failed)
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required]
        public int IS_SUCCEEDED { get; set; }

        /// <summary>
        /// open_time
        /// </summary>
        public DateTime? OPEN_TIME { get; set; }

        /// <summary>
        /// close_time
        /// </summary>
        public DateTime? CLOSE_TIME { get; set; }

        /// <summary>
        /// from
        /// </summary>
        public string FROM { get; set; }

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
        /// time
        /// </summary>
        public string? TIME { get; set; }

        /// <summary>
        /// body
        /// </summary>
        public string? BODY { get; set; }

        /// <summary>
        /// foot
        /// </summary>
        public string? FOOT { get; set; }

        #endregion
    }
}
