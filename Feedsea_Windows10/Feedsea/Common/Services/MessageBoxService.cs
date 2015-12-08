using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI.Popups;

namespace Feedsea.Common.Services
{
    public class MessageBoxService : IMessageBoxService
    {
        public async Task<bool> ConfirmationBox(string resourceString)
        {
            var resourceLoader = new ResourceLoader();
            var dialog = new MessageDialog(resourceLoader.GetString(resourceString));

            // Add commands and set their callbacks; both buttons use the same callback function instead of inline event handlers
            dialog.Commands.Add(new UICommand(
                resourceLoader.GetString("MessageDialogs_YesButton/Text"), null, 1));
            dialog.Commands.Add(new UICommand(
                resourceLoader.GetString("MessageDialogs_NoButton/Text"), null, 2));

            // Set the command that will be invoked by default
            dialog.DefaultCommandIndex = 0;

            // Set the command to be invoked when escape is pressed
            dialog.CancelCommandIndex = 1;

            // Show the message dialog
            var result = await dialog.ShowAsync();
            return (int)result.Id == 1;
        }

        public async Task InformationBox(string resourceString)
        {
            var resourceLoader = new ResourceLoader();
            var dialog = new MessageDialog(resourceLoader.GetString(resourceString));
            dialog.Commands.Add(new UICommand(
                resourceLoader.GetString("MessageDialogs_OkButton/Text"), null, 1));

            // Set the command that will be invoked by default
            dialog.DefaultCommandIndex = 0;

            // Set the command to be invoked when escape is pressed
            dialog.CancelCommandIndex = 0;
            await dialog.ShowAsync();
        }
    }
}
