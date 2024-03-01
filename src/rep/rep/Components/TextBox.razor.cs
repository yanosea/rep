using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace rep.Components
{
    /// <summary>
    /// TextBox
    /// </summary>
    public partial class TextBox
    {
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
        /// isPassword
        /// </summary>
        [Parameter]
        public bool? is_password { get; set; }

        /// <summary>
        /// value
        /// </summary>
        [Parameter]
        public string value { get; set; }

        #endregion

        #region events

        /// <summary>
        /// valueChanged
        /// </summary>
        [Parameter]
        public EventCallback<string> valueChanged { get; set; }

        /// <summary>
        /// ValueChanged
        /// </summary>
        /// <param name="e">event</param>
        /// <returns>changed value</returns>
        private async Task ValueChanged(ChangeEventArgs e)
        {
            value = e.Value.ToString();
            await valueChanged.InvokeAsync(value);
        }

        #endregion
    }
}
