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
        public MessageDialog(string title, string body,
            DialogOptions options = DialogOptions.Ok,
            DialogSound sound = DialogSound.None,
            DialogColor color = DialogColor.Neutral)
        {
            InitializeComponent();

            MessageTitle.Text = title;
            MessageBody.Text = body;
            Title = title;

            if (options == DialogOptions.Ok)
            {
                HideAllButons();
                ButtonOk.Visibility = Visibility.Visible;
            }
            else if (options == DialogOptions.YesNo)
            {
                HideAllButons();
                ButtonYesNo.Visibility = Visibility.Visible;
            }

            Background = color switch
            {
                DialogColor.Neutral => (SolidColorBrush)FindResource("Dialog.TypeColor.Neutral"),
                DialogColor.Info => (SolidColorBrush)FindResource("Dialog.TypeColor.Info"),
                DialogColor.Warning => (SolidColorBrush)FindResource("Dialog.TypeColor.Warning"),
                DialogColor.Error => (SolidColorBrush)FindResource("Dialog.TypeColor.Error"),
                _ => (SolidColorBrush)FindResource("Dialog.TypeColor.Neutral")
            };

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

        private void HideAllButons()
        {
            ButtonOk.Visibility = Visibility.Hidden;
            ButtonYesNo.Visibility = Visibility.Hidden;
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

        public enum DialogOptions
        {
            Ok,
            YesNo
        }

        public enum DialogColor
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
