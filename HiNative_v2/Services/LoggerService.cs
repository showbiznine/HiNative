using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiNative.Services
{
    public class LoggerService
    {
        public static void LogEvent(string eventName)
        {
            Microsoft.HockeyApp.HockeyClient.Current.TrackEvent(eventName);
            App.StoreLogger.Log(eventName);
            Analytics.TrackEvent(eventName);
        }

        public static void LogEvent(string eventName, Dictionary<string, string> parameters)
        {
            Microsoft.HockeyApp.HockeyClient.Current.TrackEvent(eventName);
            App.StoreLogger.Log(eventName);
            Analytics.TrackEvent(eventName, parameters);
        }
    }
}
