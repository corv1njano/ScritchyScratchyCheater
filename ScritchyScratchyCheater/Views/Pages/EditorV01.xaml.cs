using ScritchyScratchyCheater.ViewModels.Pages;
using System.Windows;
using System.Windows.Controls;

namespace ScritchyScratchyCheater.Views.Pages
{
    /// <summary>
    /// Interaction logic for EditorV01.xaml
    /// </summary>
    public partial class EditorV01 : UserControl
    {
        private EditorV01ViewModel ViewModel => (EditorV01ViewModel)DataContext;

        public EditorV01()
        {
            InitializeComponent();
            DataContext = new EditorV01ViewModel();
        }

        private async void EditorV01_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadUiAsync();
        }
    }
}

