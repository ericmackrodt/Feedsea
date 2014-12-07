using System.Diagnostics;
using System.Windows;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Info;
using System;
using System.Linq;
using System.Net;
using System.IO.IsolatedStorage;
using System.Threading.Tasks;
using feedsea.BackgroundAgent.Common;

namespace feedsea.BackgroundAgent
{
    public class FeedSeaBackgroundAgent : ScheduledTaskAgent
    {
        /// <remarks>
        /// ScheduledAgent constructor, initializes the UnhandledException handler
        /// </remarks>
        static FeedSeaBackgroundAgent()
        {
            // Subscribe to the managed exception handler
            Deployment.Current.Dispatcher.BeginInvoke(delegate
            {
                Application.Current.UnhandledException += UnhandledException;
            });
        }

        /// Code to execute on Unhandled Exceptions
        private static void UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }
        }

        /// <summary>
        /// Agent that runs a scheduled task
        /// </summary>
        /// <param name="task">
        /// The invoked task
        /// </param>
        /// <remarks>
        /// This method is called when a periodic or resource intensive task is invoked
        /// </remarks>
        protected async override void OnInvoke(ScheduledTask task)
        {
            try
            {
                var manager = new BackgroundAgentManager();
                manager.ApplicationMemoryLimit = () => DeviceStatus.ApplicationMemoryUsageLimit;
                manager.ApplicationMemoryUsage = () => DeviceStatus.ApplicationPeakMemoryUsage;
                manager.ExecutionStarted = DateTime.Now;
                manager.TileBuilder = null;
                manager.CreationEnded += manager_CreationEnded;

                await manager.Start();
            }
            catch
            {
                NotifyComplete();
            }   
        }

        void manager_CreationEnded(bool error)
        {
            NotifyComplete();
        }
    }
}