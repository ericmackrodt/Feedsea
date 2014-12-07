using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using feedsea.Common.Providers;
using System.Windows.Input;
using feedsea.Resources;
using System.Windows.Media.Imaging;
using feedsea.Common.Providers.Data;

namespace feedsea.UserControls
{
    public partial class AudioPlaybackControl : UserControl
    {
        public EnclosureData AudioFile
        {
            get { return (EnclosureData)GetValue(AudioFileProperty); }
            set { SetValue(AudioFileProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AudioFile.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AudioFileProperty =
            DependencyProperty.Register("AudioFile", typeof(EnclosureData), typeof(AudioPlaybackControl), new PropertyMetadata(null, AudioFileChanged));

        private static void AudioFileChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ct = (d as AudioPlaybackControl);

            if (e.NewValue != null)
                ct.Visibility = Visibility.Visible;
            else
                ct.Visibility = Visibility.Collapsed;
        }

        public string ArticleTitle
        {
            get { return (string)GetValue(ArticleTitleProperty); }
            set { SetValue(ArticleTitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ArticleTitle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ArticleTitleProperty =
            DependencyProperty.Register("ArticleTitle", typeof(string), typeof(AudioPlaybackControl), new PropertyMetadata(null, ArticleTitleChanged));

        private static void ArticleTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ct = (d as AudioPlaybackControl);
            ct.txtTitle.Text = (string)e.NewValue;
        }

        public ICommand PlayCommand
        {
            get { return (ICommand)GetValue(PlayCommandProperty); }
            set { SetValue(PlayCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlayCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlayCommandProperty =
            DependencyProperty.Register("PlayCommand", typeof(ICommand), typeof(AudioPlaybackControl), new PropertyMetadata(null));

        public bool IsPlaying
        {
            get { return (bool)GetValue(IsPlayingProperty); }
            set { SetValue(IsPlayingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsPlaying.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsPlayingProperty =
            DependencyProperty.Register("IsPlaying", typeof(bool), typeof(AudioPlaybackControl), new PropertyMetadata(false, IsPlayingChanged));

        private static void IsPlayingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ct = (d as AudioPlaybackControl);

            if ((bool)e.NewValue)
            {
                ct.txtPlay.Text = AppResources.AudioFile_Playing;
                ct.imgPlay.Source = new BitmapImage(new Uri("../Assets/Icons/pause.png", UriKind.Relative));
            }
            else
            {
                ct.txtPlay.Text = AppResources.AudioFile_Play;
                ct.imgPlay.Source = new BitmapImage(new Uri("../Assets/Icons/play.png", UriKind.Relative));
            }
        }
        
        public AudioPlaybackControl()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (PlayCommand != null && PlayCommand.CanExecute(AudioFile))
                PlayCommand.Execute(AudioFile);
        }
    }
}
