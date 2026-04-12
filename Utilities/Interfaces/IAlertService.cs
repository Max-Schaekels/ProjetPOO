using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPOO.Utilities.Interfaces
{
    public interface IAlertService
    {
        // show alert with just a title and a message
        Task ShowAlert(string title, string message);
        //show confirmation message style Yes No
        Task<bool> ShowConfirmation(string title, string message);
        Task<bool> ShowConfirmation(string title, string message, string accept, string cancel);
        //show alert with a pop up display with a list of buttons(multiple choices)
        Task<string> ShowQuestion(string title, params string[] buttons);
        Task<string> ShowPrompt(string title, string message);
    }
}
