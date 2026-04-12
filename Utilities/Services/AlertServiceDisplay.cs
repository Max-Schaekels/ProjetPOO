using ProjetPOO.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPOO.Utilities.Services
{
    public class AlertServiceDisplay : IAlertService
    {
        /// <summary>
        /// show alert with a pop up display with just a title and a message
        /// </summary>
        public async Task ShowAlert(string title, string message)
        {
            await Shell.Current.DisplayAlert(title, message, "OK");
        }
        /// <summary>
        /// show alert with a pop up display with a confirmation question Yes or No
        /// </summary>
        public async Task<bool> ShowConfirmation(string title, string message)
        {
            return await Shell.Current.DisplayAlert(title, message, "Yes", "No");
        }

        /// <summary>
        /// show alert with a pop up display with a confirmation with personnel text accept and cancel
        /// </summary>
        public async Task<bool> ShowConfirmation(string title, string message, string accept, string cancel)
        {
            return await Shell.Current.DisplayAlert(title, message, accept, cancel);
        }

        /// <summary>
        /// show alert with a pop up display with a list of buttons (multiple choices)
        /// </summary>
        public async Task<string> ShowQuestion(string title, params string[] buttons)
        {
            return await Shell.Current.DisplayActionSheet(title, "Cancel", null, buttons);
        }

        /// <summary>
        /// show alert with a pop up display with an entry box
        /// </summary>
        public async Task<string> ShowPrompt(string title, string message)
        {
            return await Shell.Current.DisplayPromptAsync(title, message);
        }

    }
}
