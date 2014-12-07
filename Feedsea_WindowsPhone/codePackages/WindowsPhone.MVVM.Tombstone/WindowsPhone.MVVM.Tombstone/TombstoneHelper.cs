using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Microsoft.Phone.Controls;
using System.Reflection;
using Newtonsoft.Json;
using System.Windows.Navigation;
using Microsoft.Phone.Shell;
using System.Windows;

namespace WindowsPhone.MVVM.Tombstone
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

        private static List<PhoneApplicationPage> restoredpages = new List<PhoneApplicationPage>();
        public static void page_OnNavigatedTo(PhoneApplicationPage sender, NavigationEventArgs e)
        {
            if (sender != null && _hasbeentombstoned && !restoredpages.Contains(sender))
            {
                restoredpages.Add(sender);
                ApplicationState.Restore(sender);
            }                      
        }

        public static void page_OnNavigatedFrom(PhoneApplicationPage sender, NavigationEventArgs e)
        {
            if (!(e.NavigationMode == NavigationMode.Back))
            {
                if (sender != null)
                {
                    ApplicationState.Save(sender);
                }
            }
        }
    }
}