using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using feedsea.Common.Converters;
using System.Windows.Media.Imaging;
using feedsea.Resources;
using System.Windows.Media;
using feedsea.Common.Providers;

namespace feedsea.UserControls
{
    public partial class SelectedSourceControl : UserControl
    {
        public SelectedSourceControl()
        {
            InitializeComponent();
        }

        //public INewsSource SelectedSource
        //{
        //    get { return (INewsSource)GetValue(SelectedSourceProperty); }
        //    set { SetValue(SelectedSourceProperty, value); }
        //}

        //public static readonly DependencyProperty SelectedSourceProperty =
        //    DependencyProperty.Register(
        //        "SelectedSource",
        //        typeof(INewsSource),
        //        typeof(SelectedSourceControl),
        //        new PropertyMetadata(null, OnSelectedSourceChanged));

        //private static void OnSelectedSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    if (e.NewValue == e.OldValue)
        //        return;

        //    var control = (d as SelectedSourceControl);

        //    if (e.NewValue == null)
        //    {
        //        control.Visibility = Visibility.Collapsed;
        //        return;
        //    }

        //    var source = (INewsSource)e.NewValue;

        //    var isCategory = source is ICategory;
        //    var isOwn = isCategory && (source as ICategory).Own;

        //    var id = isCategory ? null : (source as ISubscription).Link;
        //    var alternative = isOwn ? "../Assets/Icons/" + source.Name + ".png" : "../Assets/Icons/source-icon.png";
        //    control.sourceImage.Source = new BitmapImage(new Uri((string)new UrlToFaviconConverter().Convert(id, null, alternative, null), UriKind.RelativeOrAbsolute));

        //    if (isCategory)
        //    {
        //        var cat = (source as ICategory);
        //        if (cat.Own)
        //        {
        //            control.sourceName.Text = AppResources.ResourceManager.GetString(cat.Name);
        //        }
        //        else control.sourceName.Text = cat.Name;
        //    }
        //    else
        //        control.sourceName.Text = source.Name;

        //    control.Visibility = Visibility.Visible;
        //}
    }
}
