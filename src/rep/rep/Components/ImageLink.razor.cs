using Microsoft.AspNetCore.Components;

namespace rep.Components
{
    /// <summary>
    /// ImageLink 
    /// </summary>
    public partial class ImageLink
    {
        #region params

        /// <summary>
        /// href
        /// </summary>
        [Parameter]
        public string href { get; set; }

        /// <summary>
        /// img
        /// </summary>
        [Parameter]
        public string img { get; set; }

        /// <summary>
        /// alt
        /// </summary>
        [Parameter]
        public string alt { get; set; }

        /// <summary>
        /// targetBlank
        /// </summary>
        [Parameter]
        public bool? targetBlank { get; set; }

        #endregion
    }
}
