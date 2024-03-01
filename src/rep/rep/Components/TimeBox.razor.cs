using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace rep.Components
{
    /// <summary>
    /// TimeBox
    /// </summary>
    public partial class TimeBox
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

        [Parameter]
        public int step { get; set; }

        /// <summary>
        /// value
        /// </summary>
        [Parameter]
        public DateTime? value { get; set; }

        #endregion

        #region events

        /// <summary>
        /// valueChanged
        /// </summary>
        [Parameter]
        public EventCallback<DateTime?> valueChanged { get; set; }

        /// <summary>
        /// ValueChanged
        /// </summary>
        /// <param name="e">event</param>
        /// <returns>changed value</returns>
        private async Task ValueChanged(ChangeEventArgs e)
        {
            if (DateTime.TryParse(e.Value.ToString(), out DateTime changedValue))
            {
                value = changedValue;
            }

            await valueChanged.InvokeAsync(value);
        }

        #endregion
    }
}
