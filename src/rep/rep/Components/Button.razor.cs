using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace rep.Components
{
    /// <summary>
    /// Button
    /// </summary>
    public partial class Button
    {
        #region params

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
        /// bg
        /// </summary>
        [Parameter]
        public string bg { get; set; }

        /// <summary>
        /// bg_hover
        /// </summary>
        [Parameter]
        public string bg_hover { get; set; }

        /// <summary>
        /// font_color
        /// </summary>
        [Parameter]
        public string font_color { get; set; }

        /// <summary>
        /// OnClick
        /// </summary>
        [Parameter]
        public EventCallback OnClick { get; set; }

        #endregion

        #region events

        /// <summary>
        /// OnClickHandler
        /// </summary>
        private async Task OnClickHandler()
        {
            await OnClick.InvokeAsync();
        }

        #endregion
    }
}
