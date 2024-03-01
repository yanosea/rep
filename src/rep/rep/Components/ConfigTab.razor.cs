using Microsoft.AspNetCore.Components;

namespace rep.Components
{
    /// <summary>
    /// ConfigTab
    /// </summary>
    public partial class ConfigTab
    {
        #region params

        /// <summary>
        /// config
        /// </summary>
        [Parameter]
        public string config { get; set; }

        #endregion
    }
}
