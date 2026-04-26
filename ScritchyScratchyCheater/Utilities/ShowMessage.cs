using ScritchyScratchyCheater.Views.Dialogs;
using System.Windows;
using static ScritchyScratchyCheater.Views.Dialogs.MessageDialog;

namespace ScritchyScratchyCheater.Utilities
{
    public static class ShowMessage
    {
        public static bool Neutral(string title, string body, DialogOptions options = DialogOptions.Ok, DialogSound sound = DialogSound.None, Window? owner = null)
        {
            var d = new MessageDialog(title, body, options, sound, DialogColor.Neutral);
            d.Owner = owner ?? App.Current.MainWindow;

            return d.ShowDialog() ?? false;
        }

        public static bool Info(string title, string body, DialogOptions options = DialogOptions.Ok, DialogSound sound = DialogSound.Info, Window? owner = null)
        {
            var d = new MessageDialog(title, body, options, sound, DialogColor.Info);
            d.Owner = owner ?? App.Current.MainWindow;

            return d.ShowDialog() ?? false;
        }

        public static bool Warning(string title, string body, DialogOptions options = DialogOptions.Ok, DialogSound sound = DialogSound.None, Window? owner = null)
        {
            var d = new MessageDialog(title, body, options, sound, DialogColor.Warning);
            d.Owner = owner ?? App.Current.MainWindow;

            return d.ShowDialog() ?? false;
        }

        public static bool Error(string title, string body, DialogOptions options = DialogOptions.Ok, DialogSound sound = DialogSound.Error, Window? owner = null)
        {
            var d = new MessageDialog(title, body, options, sound, DialogColor.Error);
            d.Owner = owner ?? App.Current.MainWindow;

            return d.ShowDialog() ?? false;
        }
    }
}
