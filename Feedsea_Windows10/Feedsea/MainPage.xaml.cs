using Feedsea.UserControls;
using Feedsea.ViewModels;
using Feedsea.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Feedsea
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private IArticleViewControl _currentArticleViewView;

        public MainViewModel ViewModel { get { return (MainViewModel)DataContext; } }

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.New)
                Frame.BackStack.Clear();

            await ViewModel.LoadData(null);
        }

        private void BtnSplitView_Click(object sender, RoutedEventArgs e)
        {
            SplMain.IsPaneOpen = !SplMain.IsPaneOpen;
        }

        private void SideBarControl_SidebarClicked(object sender, EventArgs e)
        {
            var currentState = OrientationStates.CurrentState.Name;

            switch (currentState)
            {
                case "MobileState":
                case "Mobile6InchState":
                case "NarrowState":
                    SplMain.IsPaneOpen = false;
                    break;
                default:
                    break;
            }
        }

        private void BtnGoToTop_Click(object sender, RoutedEventArgs e)
        {
            if (_currentArticleViewView != null && ViewModel.Articles != null && ViewModel.Articles.Any())
                _currentArticleViewView.ScrollToTop(ViewModel.Articles.FirstOrDefault());
        }

        private void ViewControl_Loaded(object sender, RoutedEventArgs e)
        {
            _currentArticleViewView = (IArticleViewControl)sender;
        }
    }
}
