using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace rep.Components
{
    /// <summary>
    /// NumberBox 
    /// </summary>
    public partial class NumberBox
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
        /// min
        /// </summary>
        [Parameter]
        public int min { get; set; }

        /// <summary>
        ///max
        /// </summary>
        [Parameter]
        public int max { get; set; }

        /// <summary>
        /// value
        /// </summary>
        [Parameter]
        public int value { get; set; }

        /// <summary>
        /// valueChanged
        /// </summary>
        [Parameter]
        public EventCallback<int> valueChanged { get; set; }

        #endregion

        #region events

        /// <summary>
        /// ValueChanged
        /// </summary>
        /// <param name="e">event</param>
        /// <returns>changed value</returns>
        private async Task ValueChanged(ChangeEventArgs e)
        {
            int changedvalue;

            if (Int32.TryParse(e.Value.ToString(), out changedvalue))
            {
                value = changedvalue;
            }
            else
            {
                value = 0;
            }

            await valueChanged.InvokeAsync(value);
        }

        #endregion
    }
}
