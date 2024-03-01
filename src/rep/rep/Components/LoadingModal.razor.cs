using Microsoft.AspNetCore.Components;

namespace rep.Components
{
    /// <summary>
    /// LoadingModal
    /// </summary>
    public partial class LoadingModal
    {
        #region params

        /// <summary>
        /// text
        /// </summary>
        [Parameter]
        public string text { get; set; }

        #endregion
    }
}
