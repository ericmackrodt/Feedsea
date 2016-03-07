﻿using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Feedsea.UserControls
{
    public sealed partial class ArticleListViewItem : UserControl
    {
        public ICommand ShareCommand
        {
            get { return (ICommand)GetValue(ShareCommandProperty); }
            set { SetValue(ShareCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShareCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShareCommandProperty =
            DependencyProperty.Register("ShareCommand", typeof(ICommand), typeof(ArticleListViewItem), new PropertyMetadata(null));

        public ICommand ToggleReadCommand
        {
            get { return (ICommand)GetValue(ToggleReadCommandProperty); }
            set { SetValue(ToggleReadCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ToggleReadCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ToggleReadCommandProperty =
            DependencyProperty.Register("ToggleReadCommand", typeof(ICommand), typeof(ArticleListViewItem), new PropertyMetadata(null));

        public ICommand ToggleSavedForLaterCommand
        {
            get { return (ICommand)GetValue(ToggleSavedForLaterCommandProperty); }
            set { SetValue(ToggleSavedForLaterCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ToggleSavedForLaterCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ToggleSavedForLaterCommandProperty =
            DependencyProperty.Register("ToggleSavedForLaterCommand", typeof(ICommand), typeof(ArticleListViewItem), new PropertyMetadata(null));

        public ArticleListViewItem()
        {
            this.InitializeComponent();
        }
    }
}
