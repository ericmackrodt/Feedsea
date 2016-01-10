using Feedsea.Common.Providers.Data;
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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Feedsea.UserControls
{
    public sealed partial class CategoryItemControl : UserControl
    {
        public event EventHandler<INewsSource> OnSelectedChanged;

        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsExpanded.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsExpandedProperty =
            DependencyProperty.Register("IsExpanded", typeof(bool), typeof(CategoryItemControl), new PropertyMetadata(true, OnExpansionChanged));

        private static void OnExpansionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctr = (d as CategoryItemControl);
            
            if ((bool)e.NewValue)
            {
                var lvi = ctr.LstSubscriptions.ContainerFromIndex(0) as ListViewItem;

                if (lvi == null) return;

                var height = lvi.ActualHeight * ctr.LstSubscriptions.Items.Count;
                ctr.OpenAnim.To = height;

                ctr.LstSubscriptions.Visibility = Visibility.Visible;
                ctr.OpenStoryboard.Begin();
            }
            else
            {
                ctr.CloseAnim.From = ctr.LstSubscriptions.ActualHeight;
                ctr.CloseStoryboard.Completed += (s, arg) => { ctr.LstSubscriptions.Visibility = Visibility.Collapsed; };
                ctr.CloseStoryboard.Begin();
            }
        }

        public CategoryItemControl()
        {
            this.InitializeComponent();
        }

        private void LstSubscriptions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LstSubscriptions.SelectedIndex == -1)
                return;

            CategoryItem.IsSelected = false;

            SetSelectedItem(LstSubscriptions.SelectedItem as INewsSource);
        }

        private void SetSelectedItem(INewsSource source)
        {
            if (OnSelectedChanged != null)
                OnSelectedChanged(this, source);
        }

        private void ListViewItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            LstSubscriptions.SelectedIndex = -1;
            CategoryItem.IsSelected = true;

            SetSelectedItem(CategoryItem.DataContext as INewsSource);
        }

        internal void Unselect()
        {
            CategoryItem.IsSelected = false;
            LstSubscriptions.SelectedIndex = -1;
        }

        private void CategoryItem_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            LstSubscriptions.SelectedIndex = -1;
            CategoryItem.IsSelected = true;

            SetSelectedItem(CategoryItem.DataContext as INewsSource);
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            var lvi = LstSubscriptions.ContainerFromIndex(0) as ListViewItem;

            if (lvi == null) return;

            var height = lvi.ActualHeight * LstSubscriptions.Items.Count;
            OpenAnim.To = height;

            LstSubscriptions.Visibility = Visibility.Visible;
            OpenStoryboard.Begin();
        }

        private void ToggleExpand_Unchecked(object sender, RoutedEventArgs e)
        {
            CloseAnim.From = LstSubscriptions.ActualHeight;
            CloseStoryboard.Completed += (s, arg) => { LstSubscriptions.Visibility = Visibility.Collapsed; };
            CloseStoryboard.Begin();
        }
    }
}
