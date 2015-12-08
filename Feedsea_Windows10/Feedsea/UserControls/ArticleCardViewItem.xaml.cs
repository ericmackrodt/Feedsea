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
    public sealed partial class ArticleCardViewItem : UserControl
    {
        public ICommand ShareCommand
        {
            get { return (ICommand)GetValue(ShareCommandProperty); }
            set { SetValue(ShareCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShareCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShareCommandProperty =
            DependencyProperty.Register("ShareCommand", typeof(ICommand), typeof(ArticleCardViewItem), new PropertyMetadata(null, OnShareCommandChanged));

        private static void OnShareCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as ArticleCardViewItem;
            ctrl.BtnShare.Command = (ICommand)e.NewValue;
            ctrl.BtnShare.CommandParameter = ctrl.DataContext;
        }

        public ICommand ToggleReadCommand
        {
            get { return (ICommand)GetValue(ToggleReadCommandProperty); }
            set { SetValue(ToggleReadCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ToggleReadCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ToggleReadCommandProperty =
            DependencyProperty.Register("ToggleReadCommand", typeof(ICommand), typeof(ArticleCardViewItem), new PropertyMetadata(null, OnToggleReadCommandChanged));

        private static void OnToggleReadCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as ArticleCardViewItem;
            ctrl.BtnToggleRead.Command = (ICommand)e.NewValue;
            ctrl.BtnToggleRead.CommandParameter = ctrl.DataContext;
        }
        
        public bool IsRead
        {
            get { return (bool)GetValue(IsReadProperty); }
            set { SetValue(IsReadProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsRead.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsReadProperty =
            DependencyProperty.Register("IsRead", typeof(bool), typeof(ArticleCardViewItem), new PropertyMetadata(false, OnIsReadChanged));

        private static void OnIsReadChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as ArticleCardViewItem;
            var isRead = (bool)e.NewValue;
            ctrl.LayoutBase.Opacity = isRead ? 0.5 : 1;
        }

        public ArticleCardViewItem()
        {
            this.InitializeComponent();           
        }
    }
}
