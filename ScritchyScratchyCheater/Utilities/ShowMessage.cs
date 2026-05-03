using ScritchyScratchyCheater.Views.Dialogs;
using System.Windows;
using static ScritchyScratchyCheater.Views.Dialogs.MessageDialog;

namespace ScritchyScratchyCheater.Utilities
{
    /// <summary>
    /// Provides methods for displaying modal message dialogs with various visual styles and sounds.
    /// </summary>
    public static class ShowMessage
    {
        /// <summary>
        /// Displays a neutral modal dialog.
        /// </summary>
        /// <param name="title">The text to display as the dialog's title. Cannot be null.</param>
        /// <param name="body">The main message content to display in the dialog. Cannot be null.</param>
        /// <param name="options">The set of dialog buttons to display. The default is <see cref="DialogOptions.Ok"/>.</param>
        /// <param name="sound">The sound to play when the dialog is shown. The default is <see cref="DialogSound.None"/>.</param>
        /// <param name="owner">The window that will own the dialog. If null, the application's main window is used.</param>
        /// <returns>true if the user selects an affirmative option (such as OK); otherwise, false.</returns>
        public static bool Neutral(string title, string body, DialogOptions options = DialogOptions.Ok, DialogSound sound = DialogSound.None, Window? owner = null)
        {
            var d = new MessageDialog(title, body, options, sound, DialogColor.Neutral);
            d.Owner = owner ?? App.Current.MainWindow;

            return d.ShowDialog() ?? false;
        }

        /// <summary>
        /// Displays an informational modal dialog.
        /// </summary>
        /// <param name="title">The text to display as the dialog's title. Cannot be null.</param>
        /// <param name="body">The main message content to display in the dialog. Cannot be null.</param>
        /// <param name="options">The set of dialog buttons to display. The default is <see cref="DialogOptions.Ok"/>.</param>
        /// <param name="sound">The sound to play when the dialog is shown. The default is <see cref="DialogSound.Info"/>.</param>
        /// <param name="owner">The window that will own the dialog. If null, the application's main window is used.</param>
        /// <returns>true if the user selects an affirmative option (such as OK); otherwise, false.</returns>
        public static bool Info(string title, string body, DialogOptions options = DialogOptions.Ok, DialogSound sound = DialogSound.Info, Window? owner = null)
        {
            var d = new MessageDialog(title, body, options, sound, DialogColor.Info);
            d.Owner = owner ?? App.Current.MainWindow;

            return d.ShowDialog() ?? false;
        }

        /// <summary>
        /// Displays a warning modal dialog.
        /// </summary>
        /// <param name="title">The text to display as the dialog's title. Cannot be null.</param>
        /// <param name="body">The main message content to display in the dialog. Cannot be null.</param>
        /// <param name="options">The set of dialog buttons to display. The default is <see cref="DialogOptions.Ok"/>.</param>
        /// <param name="sound">The sound to play when the dialog is shown. The default is <see cref="DialogSound.None"/>.</param>
        /// <param name="owner">The window that will own the dialog. If null, the application's main window is used.</param>
        /// <returns>true if the user selects an affirmative option (such as OK); otherwise, false.</returns>
        public static bool Warning(string title, string body, DialogOptions options = DialogOptions.Ok, DialogSound sound = DialogSound.None, Window? owner = null)
        {
            var d = new MessageDialog(title, body, options, sound, DialogColor.Warning);
            d.Owner = owner ?? App.Current.MainWindow;

            return d.ShowDialog() ?? false;
        }

        /// <summary>
        /// Displays an error modal dialog.
        /// </summary>
        /// <param name="title">The text to display as the dialog's title. Cannot be null.</param>
        /// <param name="body">The main message content to display in the dialog. Cannot be null.</param>
        /// <param name="options">The set of dialog buttons to display. The default is <see cref="DialogOptions.Ok"/>.</param>
        /// <param name="sound">The sound to play when the dialog is shown. The default is <see cref="DialogSound.Error"/>.</param>
        /// <param name="owner">The window that will own the dialog. If null, the application's main window is used.</param>
        /// <returns>true if the user selects an affirmative option (such as OK); otherwise, false.</returns>
        public static bool Error(string title, string body, DialogOptions options = DialogOptions.Ok, DialogSound sound = DialogSound.Error, Window? owner = null)
        {
            var d = new MessageDialog(title, body, options, sound, DialogColor.Error);
            d.Owner = owner ?? App.Current.MainWindow;

            return d.ShowDialog() ?? false;
        }
    }
}
