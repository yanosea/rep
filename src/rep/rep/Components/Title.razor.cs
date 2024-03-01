using Microsoft.AspNetCore.Components;

namespace rep.Components
{
    /// <summary>
    /// Title
    /// </summary>
    public partial class Title
    {
        #region params

        /// <summary>
        /// title
        /// </summary>
        [Parameter]
        public string title { get; set; }

        #endregion
    }
}
