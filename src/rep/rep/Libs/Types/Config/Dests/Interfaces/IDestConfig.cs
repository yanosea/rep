using System.Collections.Generic;

namespace rep.Libs.Types.Config.Dests.Interfaces
{
    internal interface IDestConfig
    {
        /// <summary>
        /// tos
        /// </summary>
        internal List<Props.Dests.To> To { get; set; }

        /// <summary>
        /// ccs
        /// </summary>
        internal List<Props.Dests.Cc> Cc { get; set; }

        /// <summary>
        /// bccs
        /// </summary>
        internal List<Props.Dests.Bcc> Bcc { get; set; }
    }
}
