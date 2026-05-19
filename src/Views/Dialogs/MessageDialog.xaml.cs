using System.Media;
using System.Windows;
using System.Windows.Media;

namespace ScritchyScratchyCheater.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for MessageDialog.xaml
    /// </summary>
    public partial class MessageDialog : Window
    {
        public MessageDialog(string title, string body, DialogOptions options = DialogOptions.Ok, DialogSound sound = DialogSound.None, DialogType type = DialogType.Neutral)
        {
            InitializeComponent();

            MessageTitle.Text = title;
            MessageBody.Text = body;
            Title = title;

            switch (options)
            {
                case DialogOptions.Ok:
                    ButtonOk.Visibility = Visibility.Visible;
                    break;
                case DialogOptions.YesNo:
                    ButtonYesNo.Visibility = Visibility.Visible;
                    break;
                default:
                    ButtonOk.Visibility = Visibility.Visible;
                    break;
            }

            switch (type)
            {
                case DialogType.Neutral: break;
                case DialogType.Info:
                    NoticeInfo.Visibility = Visibility.Visible;
                    break;
                case DialogType.Warning:
                    NoticeWarning.Visibility = Visibility.Visible;
                    break;
                case DialogType.Error:
                    NoticeError.Visibility = Visibility.Visible;
                    break;
                default: break;
            }

            switch (sound)
            {
                case DialogSound.None: break;
                case DialogSound.Info:
                    SystemSounds.Asterisk.Play(); break;
                case DialogSound.Error:
                    SystemSounds.Hand.Play(); break;
                default: break;
            }
        }

        private void ButtonYes_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void ButtonNo_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Button_Close(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        public enum DialogOptions
        {
            Ok,
            YesNo
        }

        public enum DialogType
        {
            Neutral,
            Info,
            Warning,
            Error
        }

        public enum DialogSound
        {
            None,
            Info,
            Error
        }
    }
}
