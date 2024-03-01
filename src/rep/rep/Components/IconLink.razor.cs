using Microsoft.AspNetCore.Components;

namespace rep.Components
{
    /// <summary>
    /// IconLink
    /// </summary>
    public partial class IconLink
    {
        #region params

        /// <summary>
        /// href
        /// </summary>
        [Parameter]
        public string href { get; set; }

        /// <summary>
        /// icon
        /// </summary>
        [Parameter]
        public string icon { get; set; }

        /// <summary>
        /// targetBlank
        /// </summary>
        [Parameter]
        public bool? targetBlank { get; set; }

        #endregion
    }
}
