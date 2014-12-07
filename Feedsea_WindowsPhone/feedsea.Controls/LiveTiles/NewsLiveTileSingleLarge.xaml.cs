using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace feedsea.Controls.LiveTiles
{
    public partial class NewsLiveTileSingleLarge : UserControl
    {
        public NewsLiveTileSingleLarge()
        {
            InitializeComponent();
        }

        public string Title
        {
            get { return txtTitle.Text; }
            set { txtTitle.Text = value; }
        }

        public System.Windows.Media.ImageSource SourceImage
        {
            get { return imgSource.Source; }
            set
            {
                imgSource.Source = value;
                if (value == null)
                    imgSource.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        public string Source
        {
            get { return txtSource.Text; }
            set { txtSource.Text = value; }
        }

        public System.Windows.Media.ImageSource ArticleImage
        {
            set
            {
                grdImage.Background = new System.Windows.Media.ImageBrush() { ImageSource = value };
                if (value == null)
                    grdImage.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
    }
}
