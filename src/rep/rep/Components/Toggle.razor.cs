using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace rep.Components
{
    /// <summary>
    /// Toggle 
    /// </summary>
    public partial class Toggle
    {
        #region params

        /// <summary>
        /// text
        /// </summary>
        [Parameter]
        public string text { get; set; }

        /// <summary>
        /// value
        /// </summary>
        [Parameter]
        public bool value { get; set; }

        /// <summary>
        /// valueChanged
        /// </summary>
        [Parameter]
        public EventCallback<bool> valueChanged { get; set; }

        #endregion

        #region events

        /// <summary>
        /// ValueChanged
        /// </summary>
        /// <param name="e">event</param>
        /// <returns>changed value</returns>
        private async Task ValueChanged(ChangeEventArgs e)
        {
            value = Boolean.Parse(e.Value.ToString());
            await valueChanged.InvokeAsync(value);
        }

        #endregion
    }
}
