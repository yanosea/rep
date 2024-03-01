using System;

namespace rep.Libs.Database.Models.Interfaces
{
    /// <summary>
    /// IReport
    /// </summary>
    internal interface IReport
    {
        #region props

        /// <summary>
        /// send_time
        /// </summary>
        internal DateTime SEND_TIME { get; set; }

        /// <summary>
        /// is_succeeded (1 : succeeded / 0 : failed)
        /// </summary>
        internal int IS_SUCCEEDED { get; set; }

        /// <summary>
        /// from
        /// </summary>
        internal string? FROM { get; set; }

        /// <summary>
        /// to
        /// </summary>
        internal string? TO { get; set; }

        /// <summary>
        /// cc
        /// </summary>
        internal string? CC { get; set; }

        /// <summary>
        /// bcc
        /// </summary>
        internal string? BCC { get; set; }

        /// <summary>
        /// subject
        /// </summary>
        internal string? SUBJECT { get; set; }

        /// <summary>
        /// head
        /// </summary>
        internal string? HEAD { get; set; }

        /// <summary>
        /// body
        /// </summary>
        internal string? BODY { get; set; }

        /// <summary>
        /// foot
        /// </summary>
        internal string? FOOT { get; set; }

        #endregion
    }
}
