using feedsea.Settings;
using Microsoft.Phone.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common
{
    public class BackgroundAgentController
    {
        private PeriodicTask periodicTask;
        private const string periodicTaskName = "FeedSeaBackgroundAgent";
        private bool AgentIsEnabled;

        public BackgroundAgentController()
        {
            periodicTask = ScheduledActionService.Find(periodicTaskName) as PeriodicTask;
        }

        public bool IsEnabled
        {
            get { return (periodicTask == null ? false : periodicTask.IsEnabled); }
        }

        public void StartPeriodicAgent()
        {
            LiveTileSettings settings = new LiveTileSettings();
            StartPeriodicAgent(settings.LiveTileEnabledSetting);
        }

        public void StartPeriodicAgent(bool isEnabled)
        {
            AgentIsEnabled = true;

            // If this task already exists, remove it
            //periodicTask = ScheduledActionService.Find(periodicTaskName) as PeriodicTask;
            if (periodicTask != null)
            {
                RemoveAgent();
            }

            if (!isEnabled)
                return;

            periodicTask = new PeriodicTask(periodicTaskName);
            periodicTask.Description = "This demonstrates a periodic task.";

            try
            {
                ScheduledActionService.Add(periodicTask);

//#if(DEBUG_AGENT)
                ScheduledActionService.LaunchForTest(
                    periodicTaskName,
                    TimeSpan.FromSeconds(1));
//#endif
            }
            catch (InvalidOperationException exception)
            {
                if (exception.Message.Contains("BNS Error: The action is disabled"))
                {
                    AgentIsEnabled = false;
                }
                else if (exception.Message.Contains("BNS Error: The maximum number of ScheduledActions of this type have already been added."))
                {
                    // No user action required. The system prompts the user when the hard limit of periodic tasks has been reached.
                }
            }
            catch (SchedulerServiceException) { }
        }

        public void RemoveAgent()
        {
            try
            {
                ScheduledActionService.Remove(periodicTaskName);
            }
            catch
            {
                //noop
            }
        }
    }
}
