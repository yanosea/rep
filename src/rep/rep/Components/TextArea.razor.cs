using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace rep.Components
{
    /// <summary>
    /// TextArea
    /// </summary>
    public partial class TextArea
    {
        #region injects

        /// <summary>
        /// JSRuntime
        /// </summary>
        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        #endregion

        #region params

        /// <summary>
        /// icon
        /// </summary>
        [Parameter]
        public string icon { get; set; }

        /// <summary>
        /// placeholder
        /// </summary>
        [Parameter]
        public string placeholder { get; set; }

        /// <summary>
        /// text
        /// </summary>
        [Parameter]
        public string text { get; set; }

        /// <summary>
        /// value
        /// </summary>
        [Parameter]
        public string value { get; set; }

        /// <summary>
        /// valueChanged
        /// </summary>
        [Parameter]
        public EventCallback<string> valueChanged { get; set; }

        #endregion

        #region events

        /// <summary>
        /// ValueChanged
        /// </summary>
        /// <param name="e">event</param>
        /// <returns>changed value</returns>
        private async Task ValueChanged(ChangeEventArgs e)
        {
            await JSRuntime.InvokeVoidAsync("autoResizeTextarea", "textarea-autoresize");

            value = e.Value.ToString();
            await valueChanged.InvokeAsync(value);
        }

        /// <summary>
        /// OnAfterRenderAsync
        /// </summary>
        protected override async Task OnAfterRenderAsync(bool _)
        {
            await JSRuntime.InvokeVoidAsync("autoResizeTextarea", "textarea-autoresize");
        }

        #endregion
    }
}
