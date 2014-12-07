using feedsea.UserControls;
using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Navigation;

namespace feedsea.Common.Controls
{
    public class OAuthLogin : IOAuthLogin
    {
        Popup popup;
        WebBrowser browser;
        FullSizeLoadingPanel loadingControl;

        public bool IsOpen
        {
            get { return popup != null && popup.IsOpen; }
        }

        public async Task<string> Login(string loginUrl, string redirectUrl)
        {
            await BuildBrowser();
            try
            {
                var result = await browser.AuthenticateAsync(new Uri(loginUrl), redirectUrl, loadingControl);
                if (popup != null)
                    popup.IsOpen = false;
                return result;
            }
            catch
            {

            }

            return null;
        }

        public void Cancel()
        {
            if (popup.IsOpen)
                browser.Navigate(new Uri("http://www.feedsea.tst/"));
        }

        private async Task BuildBrowser()
        {
            popup = new Popup();
            var grid = new Grid();
            browser = new WebBrowser();
            await browser.ClearCookiesAsync();
            await browser.ClearInternetCacheAsync();
            loadingControl = new FullSizeLoadingPanel();
            loadingControl.Visibility = Visibility.Collapsed;
            browser.IsScriptEnabled = true;
            popup.Child = grid;
            grid.Children.Add(browser);
            grid.Children.Add(loadingControl);

            grid.Width = Application.Current.Host.Content.ActualWidth;
            grid.Height = Application.Current.Host.Content.ActualHeight;
            popup.IsOpen = true;
        }
    }
    
    public static class AsyncBrowser
    {
        public static Task<string> AuthenticateAsync(this WebBrowser browser, Uri uri, string redirectUrl, FullSizeLoadingPanel loadingControl)
        {
            var tcs = new TaskCompletionSource<string>();

            browser.Navigating += (o, e) =>
            {
                if (tcs.Task.IsCompleted)
                    return;

                if (!loadingControl.IsOpen)
                    loadingControl.Open();

                if (e.Uri.ToString().StartsWith(redirectUrl))
                {
                    if (loadingControl.IsOpen)
                        loadingControl.Close();

                    if (e.Uri.Query.Contains("code="))
                        tcs.SetResult(e.Uri.ToString().ParseQueryString()["code"]);
                    else
                        tcs.SetResult(e.Uri.ToString());
                }
            };

            browser.Navigated += (o, e) =>
            {
                if (tcs.Task.IsCompleted)
                    return;

                if (loadingControl.IsOpen)
                    loadingControl.Close();
            };

            browser.NavigationFailed += (o, e) =>
            {
                if (tcs.Task.IsCompleted)
                    return;

                if (loadingControl.IsOpen)
                    loadingControl.Close();

                if (e.Exception != null)
                    tcs.SetException(e.Exception);
                else
                    tcs.SetResult(null);
            };

            if (!loadingControl.IsOpen)
                loadingControl.Open();
            browser.Navigate(uri);
            return tcs.Task;
        }

        public static Dictionary<string, string> ParseQueryString(this string uri)
        {
            string substring = uri.Substring(((uri.LastIndexOf('?') == -1) ? 0 : uri.LastIndexOf('?') + 1));

            string[] pairs = substring.Split('&');

            Dictionary<string, string> output = new Dictionary<string, string>();

            foreach (string piece in pairs)
            {
                string[] pair = piece.Split('=');
                output.Add(pair[0], pair[1]);
            }

            return output;

        }
    }
}
