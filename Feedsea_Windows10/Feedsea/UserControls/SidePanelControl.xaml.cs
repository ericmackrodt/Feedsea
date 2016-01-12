using Feedsea.Common.Providers.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Feedsea.UserControls
{
    public delegate void SidebarClickEventHandler(INewsSource selectedSource);

    public sealed partial class SidePanelControl : UserControl
    {
        public event SidebarClickEventHandler SidebarClicked;



        public bool IsBarExpanded
        {
            get { return (bool)GetValue(IsBarExpandedProperty); }
            set { SetValue(IsBarExpandedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsBarExpanded.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsBarExpandedProperty =
            DependencyProperty.Register("IsBarExpanded", typeof(bool), typeof(SidePanelControl), new PropertyMetadata(true, IsExpandedChanged));

        private static void IsExpandedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var isExpanded = (bool)e.NewValue;
            var ctrl = (SidePanelControl)d;

            if (isExpanded)
            {
                ctrl.FindName("IcoSource");
                ctrl.IcoSource.Visibility = Visibility.Collapsed;
                ctrl.ScrollSources.Visibility = Visibility.Visible;
            }
            else
            {
                ctrl.FindName("IcoSource");
                ctrl.IcoSource.Visibility = Visibility.Visible;
                ctrl.ScrollSources.Visibility = Visibility.Collapsed;
            }
        }

        public ICommand ItemSelectedCommand
        {
            get { return (ICommand)GetValue(ItemSelectedCommandProperty); }
            set { SetValue(ItemSelectedCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemSelectedCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemSelectedCommandProperty =
            DependencyProperty.Register("ItemSelectedCommand", typeof(ICommand), typeof(SidePanelControl), new PropertyMetadata(null));

        public SidePanelControl()
        {
            this.InitializeComponent();
        }

        private void CategoryItemControl_OnSelectedChanged(object sender, INewsSource e)
        {
            UnmarkAll(sender);

            SetSelectedItem(e);
        }

        private void UnmarkAll(object exception = null)
        {
            if (exception != null)
                LstMain.SelectedIndex = -1;

            foreach (var itm in LstSources.Items)
            {
                var container = LstSources.ContainerFromItem(itm);
                
                var categoryItemControl = FindVisualChild<CategoryItemControl>(container);

                if (categoryItemControl != null)
                {
                    if (exception != null && categoryItemControl == exception)
                        continue;

                    categoryItemControl.Unselect();

                    continue;
                }

                var subscriptionItemControl = FindVisualChild<ListViewItem>(container);
                if (exception != null && exception == subscriptionItemControl)
                    continue;

                subscriptionItemControl.IsSelected = false;
            }
        }

        public static T FindVisualChild<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        return (T)child;
                    }

                    T childItem = FindVisualChild<T>(child);
                    if (childItem != null) return childItem;
                }
            }
            return null;
        }

        private void LstMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LstMain.SelectedIndex == -1)
                return;

            UnmarkAll();

            SetSelectedItem(LstMain.SelectedItem as INewsSource);
        }

        private void SetSelectedItem(INewsSource source)
        {
            if (SidebarClicked != null)
                SidebarClicked(source);

            if (ItemSelectedCommand != null && ItemSelectedCommand.CanExecute(source))
                ItemSelectedCommand.Execute(source);
        }

        private void SubscriptionItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var item = sender as ListViewItem;

            if (item == null)
                return;

            item.IsSelected = true;
            var source = item.DataContext as INewsSource;

            UnmarkAll(sender);

            SetSelectedItem(source);
        }
    }
}
