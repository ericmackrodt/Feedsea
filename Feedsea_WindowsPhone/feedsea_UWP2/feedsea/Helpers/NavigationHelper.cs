using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.ApplicationInsights;


namespace WP8SL_TO_UWP
{
    internal static class NavigationHelper
    {
        public static void NavigateTo(System.Uri url)
        {
            string page, arguments;
            if (url.IsAbsoluteUri)
            {
                page = url.AbsolutePath;
                arguments = url.Query;
            }
            else
            {
                var indexOfQuestionMark = url.OriginalString.IndexOf('?');
                page = indexOfQuestionMark != -1 ? url.OriginalString.Substring(0, indexOfQuestionMark) : url.OriginalString;
                arguments = indexOfQuestionMark != -1 ? url.OriginalString.Substring(indexOfQuestionMark + 1) : "";
            }
              switch(page)
              {
                case "/Views/WelcomePage.xaml":
                   ((Frame)Window.Current.Content).
                      Navigate(typeof(feedsea.Views.WelcomePage));
                   break;
                case "/Views/SettingsThirdPartyPage.xaml":
                   ((Frame)Window.Current.Content).
                      Navigate(typeof(feedsea.Views.SettingsThirdPartyPage));
                   break;
                case "/Views/SettingsPage.xaml":
                   ((Frame)Window.Current.Content).
                      Navigate(typeof(feedsea.Views.SettingsPage));
                   break;
                case "/Views/SettingsLiveTilesPage.xaml":
                   ((Frame)Window.Current.Content).
                      Navigate(typeof(feedsea.Views.SettingsLiveTilesPage));
                   break;
                case "/Views/SettingsGeneralPage.xaml":
                   ((Frame)Window.Current.Content).
                      Navigate(typeof(feedsea.Views.SettingsGeneralPage));
                   break;
                case "/Views/SettingsAppearancePage.xaml":
                   ((Frame)Window.Current.Content).
                      Navigate(typeof(feedsea.Views.SettingsAppearancePage));
                   break;
                case "/Views/MainPage.xaml":
                   ((Frame)Window.Current.Content).
                      Navigate(typeof(feedsea.Views.MainPage));
                   break;
                case "/Views/ArticlePage.xaml":
                   ((Frame)Window.Current.Content).
                      Navigate(typeof(feedsea.Views.ArticlePage));
                   break;
                case "/Views/AddSource.xaml":
                   ((Frame)Window.Current.Content).
                      Navigate(typeof(feedsea.Views.AddSource));
                   break;
                case "/Views/AboutPage.xaml":
                   ((Frame)Window.Current.Content).
                      Navigate(typeof(feedsea.Views.AboutPage));
                   break;
                default:
                    throw new System.Exception("Could not find page");
              }
        }
    }
}