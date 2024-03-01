using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace rep.Components
{
    /// <summary>
    /// SelectedReportsToggle
    /// </summary>
    public partial class SelectedReportsToggle
    {
        #region params

        /// <summary>
        /// isSelectedDaily
        /// </summary>
        [Parameter]
        public bool isSelectedDaily { get; set; }

        /// <summary>
        /// isSelectedDailyChanged
        /// </summary>
        [Parameter]
        public EventCallback<bool> isSelectedDailyChanged { get; set; }

        /// <summary>
        /// isSelectedWeekly
        /// </summary>
        [Parameter]
        public bool isSelectedWeekly { get; set; }

        /// <summary>
        /// isSelectedWeeklyChanged
        /// </summary>
        [Parameter]
        public EventCallback<bool> isSelectedWeeklyChanged { get; set; }

        /// <summary>
        /// isSelectedMonthly
        /// </summary>
        [Parameter]
        public bool isSelectedMonthly { get; set; }

        /// <summary>
        /// isSelectedMonthlyChanged
        /// </summary>
        [Parameter]
        public EventCallback<bool> isSelectedMonthlyChanged { get; set; }

        #endregion

        #region events

        /// <summary>
        /// IsSelectedDailyChanged
        /// </summary>
        /// <param name="e">event</param>
        /// <returns>changed value</returns>
        private async Task IsSelectedDailyChanged(ChangeEventArgs e)
        {
            isSelectedDaily = Boolean.Parse(e.Value.ToString());
            await isSelectedDailyChanged.InvokeAsync(isSelectedDaily);
        }

        /// <summary>
        /// IsSelectedWeeklyChanged
        /// </summary>
        /// <param name="e">event</param>
        /// <returns>changed value</returns>
        private async Task IsSelectedWeeklyChanged(ChangeEventArgs e)
        {
            isSelectedWeekly = Boolean.Parse(e.Value.ToString());
            await isSelectedWeeklyChanged.InvokeAsync(isSelectedWeekly);
        }

        /// <summary>
        /// IsSelectedMonthlyChanged
        /// </summary>
        /// <param name="e">event</param>
        /// <returns>changed value</returns>
        private async Task IsSelectedMonthlyChanged(ChangeEventArgs e)
        {
            isSelectedMonthly = Boolean.Parse(e.Value.ToString());
            await isSelectedMonthlyChanged.InvokeAsync(isSelectedMonthly);
        }

        #endregion
    }
}
