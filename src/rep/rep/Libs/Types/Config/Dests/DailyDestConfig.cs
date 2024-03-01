using System.Collections.Generic;

namespace rep.Libs.Types.Config.Dests
{
    /// <summary>
    /// daily_dest_conf
    /// </summary>
    internal sealed class DailyDestConfig : Interfaces.IDestConfig
    {
        #region props

        /// <summary>
        /// tos
        /// </summary>
        public List<Props.Dests.To> To { get; set; }

        /// <summary>
        /// ccs
        /// </summary>
        public List<Props.Dests.Cc> Cc { get; set; }

        /// <summary>
        /// bccs
        /// </summary>
        public List<Props.Dests.Bcc> Bcc { get; set; }

        #endregion

        #region constructor

        /// <summary>
        /// constructor
        /// </summary>
        public DailyDestConfig()
        {
            To = new List<Props.Dests.To>() { };
            Cc = new List<Props.Dests.Cc>() { };
            Bcc = new List<Props.Dests.Bcc>() { };
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="tos">tos</param>>
        /// <param name="ccs">ccs</param>>
        /// <param name="bccs">bccs</param>>
        public DailyDestConfig(List<Props.Dests.To> tos, List<Props.Dests.Cc> ccs, List<Props.Dests.Bcc> bccs)
        {
            To = tos;
            Cc = ccs;
            Bcc = bccs;
        }

        #endregion
    }
}
