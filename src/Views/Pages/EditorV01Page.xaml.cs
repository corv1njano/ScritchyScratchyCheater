using ScritchyScratchyCheater.ViewModels.Pages.EditorV01;
using System.Windows;
using System.Windows.Controls;

namespace ScritchyScratchyCheater.Views.Pages
{
    /// <summary>
    /// Interaction logic for EditorV01.xaml
    /// </summary>
    public partial class EditorV01Page : UserControl
    {
        private EditorV01ViewModel ViewModel => (EditorV01ViewModel)DataContext;

        public EditorV01Page()
        {
            InitializeComponent();
            DataContext = new EditorV01ViewModel();
        }

        private async void Editor_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadUiAsync();
        }
    }
}

