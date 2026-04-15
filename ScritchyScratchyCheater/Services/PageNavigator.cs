using ScritchyScratchyCheater.Views.Pages;
using System.Windows.Controls;

namespace ScritchyScratchyCheater.Services
{
    public class PageNavigator
    {
        public Page CurrentPage { get; private set; }
        public event Action<Page>? CurrentPageChanged;

        public PageNavigator()
        {
            CurrentPage = new StartPage();
        }

        /// <summary>
        /// Navigates to the specified page and updates the current page of MainWindow.
        /// </summary>
        /// <remarks>This method raises the CurrentPageChanged event if the navigation is successful,
        /// allowing subscribers to respond to the page change.</remarks>
        /// <param name="page">The page to navigate to. This parameter must not be null; if null, the navigation is ignored.</param>
        public void Navigate(Page page)
        {
            if (page == null) return;

            CurrentPage = page;
            CurrentPageChanged?.Invoke(page);
        }
    }
}
