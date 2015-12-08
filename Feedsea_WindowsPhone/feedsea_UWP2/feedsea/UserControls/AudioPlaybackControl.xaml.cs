using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using feedsea.Common.Providers;
using System.Windows.Input;
using feedsea.Resources;
using Windows.UI.Xaml.Media.Imaging;
using feedsea.Common.Providers.Data;

namespace feedsea.UserControls
{

   public partial class AudioPlaybackControl
      : Windows.UI.Xaml.Controls.UserControl
   {

      public EnclosureData AudioFile
      {
         get
         {
            return (EnclosureData)GetValue(AudioFileProperty);
         }
         set
         {
            SetValue(AudioFileProperty, value);
         }
      }

      // Using a DependencyProperty as the backing store for AudioFile.  This enables animation, styling, binding, etc...
      public static readonly Windows.UI.Xaml.DependencyProperty AudioFileProperty = Windows.UI.Xaml.DependencyProperty.Register("AudioFile", typeof(EnclosureData), typeof(AudioPlaybackControl), new PropertyMetadata(null, AudioFileChanged));

      private static void AudioFileChanged(Windows.UI.Xaml.DependencyObject d, Windows.UI.Xaml.DependencyPropertyChangedEventArgs e)
      {
         var ct = (d as AudioPlaybackControl);
         if ( e.NewValue != null )
            ct.Visibility = Windows.UI.Xaml.Visibility.Visible;
         else
            ct.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
      }

      public string ArticleTitle
      {
         get
         {
            return (string)GetValue(ArticleTitleProperty);
         }
         set
         {
            SetValue(ArticleTitleProperty, value);
         }
      }

      // Using a DependencyProperty as the backing store for ArticleTitle.  This enables animation, styling, binding, etc...
      public static readonly Windows.UI.Xaml.DependencyProperty ArticleTitleProperty = Windows.UI.Xaml.DependencyProperty.Register("ArticleTitle", typeof(string), typeof(AudioPlaybackControl), new PropertyMetadata(null, ArticleTitleChanged));

      private static void ArticleTitleChanged(Windows.UI.Xaml.DependencyObject d, Windows.UI.Xaml.DependencyPropertyChangedEventArgs e)
      {
         var ct = (d as AudioPlaybackControl);
         ct.txtTitle.Text = (string)e.NewValue;
      }

      public ICommand PlayCommand
      {
         get
         {
            return (ICommand)GetValue(PlayCommandProperty);
         }
         set
         {
            SetValue(PlayCommandProperty, value);
         }
      }

      // Using a DependencyProperty as the backing store for PlayCommand.  This enables animation, styling, binding, etc...
      public static readonly Windows.UI.Xaml.DependencyProperty PlayCommandProperty = Windows.UI.Xaml.DependencyProperty.Register("PlayCommand", typeof(ICommand), typeof(AudioPlaybackControl), new PropertyMetadata(null));

      public bool IsPlaying
      {
         get
         {
            return (bool)GetValue(IsPlayingProperty);
         }
         set
         {
            SetValue(IsPlayingProperty, value);
         }
      }

      // Using a DependencyProperty as the backing store for IsPlaying.  This enables animation, styling, binding, etc...
      public static readonly Windows.UI.Xaml.DependencyProperty IsPlayingProperty = Windows.UI.Xaml.DependencyProperty.Register("IsPlaying", typeof(bool), typeof(AudioPlaybackControl), new PropertyMetadata(false, IsPlayingChanged));

      private static void IsPlayingChanged(Windows.UI.Xaml.DependencyObject d, Windows.UI.Xaml.DependencyPropertyChangedEventArgs e)
      {
         var ct = (d as AudioPlaybackControl);
         if ( (bool)e.NewValue )
         {
            ct.txtPlay.Text = AppResources.AudioFile_Playing;
            ct.imgPlay.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(new Uri("ms-appx://"), "../Assets/Icons/pause.png"));
         }
         else
         {
            ct.txtPlay.Text = AppResources.AudioFile_Play;
            ct.imgPlay.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(new Uri("ms-appx://"), "../Assets/Icons/play.png"));
         }
      }


      public AudioPlaybackControl()
      {
         InitializeComponent();
      }

      private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
      {
         if ( PlayCommand != null && PlayCommand.CanExecute(AudioFile) )
            PlayCommand.Execute(AudioFile);
      }

   }

}