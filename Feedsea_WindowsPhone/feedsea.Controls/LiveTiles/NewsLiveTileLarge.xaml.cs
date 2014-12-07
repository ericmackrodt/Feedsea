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
    public partial class NewsLiveTileLarge : UserControl
    {
        public NewsLiveTileLarge()
        {
            InitializeComponent();
        }

        public string Title1
        {
            get { return txtTitle1.Text; }
            set { txtTitle1.Text = value; }
        }

        public System.Windows.Media.ImageSource SourceImage1
        {
            get { return imgSource1.Source; }
            set
            {
                imgSource1.Source = value;
                if (value == null)
                    imgSource1.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        public System.Windows.Media.ImageSource ArticleImage1
        {
            get { return imgSource1.Source; }
            set
            {
                imgArticle1.Source = value;
                if (value == null)
                    recImg1.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        public string Source1
        {
            get { return txtSource1.Text; }
            set { txtSource1.Text = value; }
        }

        public string Title2
        {
            get { return txtTitle2.Text; }
            set { txtTitle2.Text = value; }
        }

        public System.Windows.Media.ImageSource SourceImage2
        {
            get { return imgSource2.Source; }
            set
            {
                imgSource2.Source = value;
                if (value == null)
                    imgSource2.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        public string Source2
        {
            get { return txtSource2.Text; }
            set { txtSource2.Text = value; }
        }

        public System.Windows.Media.ImageSource ArticleImage2
        {
            get { return imgArticle2.Source; }
            set
            {
                imgArticle2.Source = value;
                if (value == null)
                    recImg2.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
    }
}
