using ScritchyScratchyCheater.ViewModels.Pages;
using System.Windows.Controls;

namespace ScritchyScratchyCheater.Views.Pages
{
    /// <summary>
    /// Interaction logic for StartingPage.xaml
    /// </summary>
    public partial class StartingPage : UserControl
    {
        public StartingPage()
        {
            InitializeComponent();
            DataContext = new StartingPageViewModel();
        }        
    }
}
