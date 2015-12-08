using feedsea.Common.Providers;
using Telerik.Windows.Controls;
using feedsea.Common.MVVM.Tombstone;
using feedsea.BackgroundAgent.Common;
using feedsea.Common;
using System.IO.IsolatedStorage;
using feedsea.ViewModels;
using feedsea.Resources;
using System.Resources;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.ApplicationInsights;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=402347&clcid=0x409
namespace feedsea
{

   /// <summary>
   /// Provides application-specific behavior to supplement the default Application class.
   /// </summary>
   sealed partial class App
      : Application
   {
      /// <summary>
      /// Allows tracking page views, exceptions and other telemetry through the Microsoft Application Insights service.
      /// </summary>
      public TelemetryClient TelemetryClient = new TelemetryClient();

      /// <summary>
      /// Initializes the singleton application object.  This is the first line of authored code
      /// executed, and as such is the logical equivalent of main() or WinMain().
      /// </summary>
      public App()
      {
         this.InitializeComponent();
         this.Suspending += OnSuspending;
      // this.Suspending
      //  this.
      }

      /// <summary>
      /// Invoked when the application is launched normally by the end user.  Other entry points
      /// will be used such as when the application is launched to open a specific file.
      /// </summary>
      /// <param name="e">Details about the launch request and process.</param>
      protected override void OnLaunched(LaunchActivatedEventArgs e)
      {
#if DEBUG
if(System.Diagnostics.Debugger.IsAttached){this.DebugSettings.EnableFrameRateCounter=true;}
      #endif
         Frame rootFrame = Window.Current.Content as Frame;
         // Do not repeat app initialization when the Window already has content,
         // just ensure that the window is active
         if ( rootFrame == null )
         {
            // Create a Frame to act as the navigation context and navigate to the first page
            rootFrame = new Frame();
            // Set the default language
            rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];
            rootFrame.NavigationFailed += OnNavigationFailed;
            if ( e.PreviousExecutionState == ApplicationExecutionState.Terminated )
            {
            //TODO: Load state from previously suspended application
            }
            // Place the frame in the current Window
            Window.Current.Content = rootFrame;
         }
         if ( rootFrame.Content == null )
         {
            // When the navigation stack isn't restored navigate to the first page,
            // configuring the new page by passing required information as a navigation
            // parameter
            rootFrame.Navigate(typeof(MainPage), e.Arguments);
         }
         // Ensure the current window is active
         Window.Current.Activate();
         //[WP8SL_TO_UWP] The following code was added to emulate the default behavoir of
         // the back button on WP8SL
         Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Windows.UI.Core.AppViewBackButtonVisibility.Visible;
         Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += (object sender, Windows.UI.Core.BackRequestedEventArgs backEventArgs) =>
            {
               if ( !backEventArgs.Handled && rootFrame.CanGoBack )
               {
                  rootFrame.GoBack();
                  backEventArgs.Handled = true;
               }
            };
         Application_Launching(e);
      }

      /// <summary>
      /// Invoked when Navigation to a certain page fails
      /// </summary>
      /// <param name="sender">The Frame which failed navigation</param>
      /// <param name="e">Details about the navigation failure</param>
      void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
      {
         throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
      }

      /// <summary>
      /// Invoked when application execution is being suspended.  Application state is saved
      /// without knowing whether the application will be terminated or resumed with the contents
      /// of memory still intact.
      /// </summary>
      /// <param name="sender">The source of the suspend request.</param>
      /// <param name="e">Details about the suspend request.</param>
      private void OnSuspending(object sender, SuspendingEventArgs e)
      {
         var deferral = e.SuspendingOperation.GetDeferral();
         //TODO: Save application state and stop any background activity
         deferral.Complete();
         Application_Deactivated(sender, e);
         Application_Closing(sender, e);
      }

      public const string ApplicationVersion = "1.5.0.25";
      public RadDiagnostics Diagnostics;

      async void Application_Launching(Windows.ApplicationModel.Activation.LaunchActivatedEventArgs args)
      {
         ApplicationUsageHelper.Init(ApplicationVersion);
         try // try block recommended to detect compilation errors in VCD file

         {
            await Windows.ApplicationModel.VoiceCommands.VoiceCommandDefinitionManager.InstallCommandDefinitionsFromStorageFileAsync(new Uri("ms-appx:///FeedseaVoiceCommands.xml"));
         }
         catch
         {
         // Handle exception
         }
         CheckAndLoadInitialData();
      }

      protected override void OnActivated(Windows.ApplicationModel.Activation.IActivatedEventArgs args)
      {
         // Ensure that application state is restored appropriately
         ApplicationUsageHelper.OnApplicationActivated();
         TombstoneHelper.Application_Activated(sender, e);
      }

      void Application_Deactivated(object obj, Windows.ApplicationModel.SuspendingEventArgs args)
      {
      }

      void Application_Closing(object obj, Windows.ApplicationModel.SuspendingEventArgs args)
      {
      }

   }

}