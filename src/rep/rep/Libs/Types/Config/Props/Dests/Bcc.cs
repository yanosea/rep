using System;
using System.Runtime.Serialization;
using Tomlyn.Model;

namespace rep.Libs.Types.Config.Props.Dests
{
    /// <summary>
    /// bcc
    /// </summary>
    internal sealed class Bcc : Interfaces.IDest, ITomlMetadataProvider
    {
        #region props

        /// <summary>
        /// PropertiesMetadata
        /// </summary>
        TomlPropertiesMetadata? ITomlMetadataProvider.PropertiesMetadata { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        [IgnoreDataMember]
        public Guid Id { get; set; }

        /// <summary>
        /// name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// mailaddress
        /// </summary>
        public string Mailaddress { get; set; }

        #endregion

        #region constructor

        /// <summary>
        /// constructor
        /// </summary>
        public Bcc()
        {
            Id = Guid.NewGuid();
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="name">name</param>>
        /// <param name="mailaddress">mailaddress</param>>
        public Bcc(string name, string mailaddress)
        {
            Name = name;
            Mailaddress = mailaddress;
        }

        #endregion
    }
}
