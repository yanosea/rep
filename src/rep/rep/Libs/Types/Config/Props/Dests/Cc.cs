using System;
using System.Runtime.Serialization;
using Tomlyn.Model;

namespace rep.Libs.Types.Config.Props.Dests
{
    /// <summary>
    /// cc
    /// </summary>
    internal sealed class Cc : Interfaces.IDest, ITomlMetadataProvider
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
        public Cc()
        {
            Id = Guid.NewGuid();
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="name">name</param>>
        /// <param name="mailaddress">mailaddress</param>>
        public Cc(string name, string mailaddress)
        {
            Name = name;
            Mailaddress = mailaddress;
        }

        #endregion
    }
}
