using ScritchyScratchyCheater.Views.Pages;
using System.Windows.Controls;

namespace ScritchyScratchyCheater.Services
{
    /// <summary>
    /// Provides navigation functionality for switching between pages within a user interface.
    /// </summary>
    public class PageNavigator
    {
        public UserControl CurrentPage { get; private set; }
        public event Action<UserControl>? CurrentPageChanged;

        public PageNavigator()
        {
            CurrentPage = new StartingPage();
        }

        /// <summary>
        /// Navigates to the specified page and updates the current page of MainWindow.
        /// </summary>
        /// <remarks>This method raises the CurrentPageChanged event if the navigation is successful,
        /// allowing subscribers to respond to the page change.</remarks>
        /// <param name="page">The page to navigate to. This parameter must not be null; if null, the navigation is ignored.</param>
        public void Navigate(UserControl page)
        {
            if (page == null) return;

            CurrentPage = page;
            CurrentPageChanged?.Invoke(page);
        }
    }
}
