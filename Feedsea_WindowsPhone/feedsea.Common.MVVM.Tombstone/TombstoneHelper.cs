using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Microsoft.Phone.Controls;
using System.Reflection;
using System.Windows.Navigation;
using Microsoft.Phone.Shell;
using System.Windows;

namespace feedsea.Common.MVVM.Tombstone
{
    public sealed class TombstoneHelper
    {
        private static bool _hasbeentombstoned = false;       
        public static void Application_Activated(object sender, ActivatedEventArgs e)
        {
            if (!e.IsApplicationInstancePreserved)
            {
                _hasbeentombstoned = true;
            }
        }

        private static List<string> restoredpages = new List<string>();
        public static void page_OnNavigatedTo(PhoneApplicationPage sender, NavigationEventArgs e)
        {
            if (sender != null && _hasbeentombstoned && !restoredpages.Contains(sender.GetType().Name))
            {
                restoredpages.Add(sender.GetType().Name);
                ApplicationState.Restore(sender);
            }                      
        }

        public static void page_OnNavigatedFrom(PhoneApplicationPage sender, NavigationEventArgs e)
        {
            if (e.NavigationMode != NavigationMode.Back)
            {
                if (sender != null)
                {
                    ApplicationState.Save(sender);
                }
            }
        }
    }
}