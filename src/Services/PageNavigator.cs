using ScritchyScratchyCheater.Views.Pages;
using System.Windows.Controls;

namespace ScritchyScratchyCheater.Services
{
    /// <summary>
    /// Provides navigation functionality for switching between pages within a user interface.
    /// </summary>
    public class PageNavigator
    {
        /// <summary>
        /// The current page that is being loaded and shown in the app.
        /// </summary>
        public UserControl CurrentPage { get; private set; }
        public event Action<UserControl>? CurrentPageChanged;

        public PageNavigator()
        {
            CurrentPage = new StartingPage();
        }

        /// <summary>
        /// Navigates to the specified page and updates the current page of MainWindow.
        /// </summary>
        /// <param name="page">The page to navigate to. This parameter must not be null; if null, the navigation is ignored.</param>
        public void Navigate(UserControl page)
        {
            if (page == null) return;

            CurrentPage = page;
            CurrentPageChanged?.Invoke(page);
        }
    }
}
