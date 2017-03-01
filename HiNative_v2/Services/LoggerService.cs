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
        }
    }
}
