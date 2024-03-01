using System;
using System.Runtime.Serialization;
using Tomlyn.Model;

namespace rep.Libs.Types.Config.Props.Dests
{
    /// <summary>
    /// to
    /// </summary>
    internal sealed class To : Interfaces.IDest, ITomlMetadataProvider
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
        public To()
        {
            Id = Guid.NewGuid();
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="id">id</param>>
        /// <param name="name">name</param>>
        /// <param name="mailaddress">mailaddress</param>>
        public To(Guid id, string name, string mailaddress)
        {
            Id = id;
            Name = name;
            Mailaddress = mailaddress;
        }

        #endregion
    }
}
