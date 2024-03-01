using Microsoft.AspNetCore.Components;

namespace rep.Components
{
    /// <summary>
    /// IconTextLink
    /// </summary>
    public partial class IconTextLink
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
        /// text
        /// </summary>
        [Parameter]
        public string text { get; set; }

        /// <summary>
        /// targetBlank
        /// </summary>
        [Parameter]
        public bool? targetBlank { get; set; }

        #endregion
    }
}
