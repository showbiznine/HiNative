using HiNative.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Storage;

namespace HiNative.Services
{
    public class BackgroundService
    {
        private static BackgroundTaskDeferral _deferral;
        private static int _lastID;

        public static async Task CheckNotificationsTask(IBackgroundTaskInstance taskInstance)
        {
            _deferral = taskInstance.GetDeferral();
            var res = DataService.GetUnreadCount();
            var unreadCount = res.activities.unread_count;
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            //localSettings.Values["to_ignore"] = 0; //Init
            _lastID = (localSettings.Values["to_ignore"] != null ? (int)localSettings.Values["to_ignore"] : 0);

            if (unreadCount > 0)
            {
                var activities = await DataService.GetNotifications(1);
                for (int i = 0; i < unreadCount; i++)
                {
                    if (activities.activities[i].id == _lastID)
                    {
                        break;
                    }
                    NotificationService.PopToast(activities.activities[i]);
                    NotificationService.SendBadge((uint)unreadCount);
                    //PopTile(activities.activities[i]);
                    localSettings.Values["to_ignore"] = activities.activities[0].id;
                }
            }
            else
            {
                NotificationService.ClearBadge();
            }

            _deferral.Complete();
        }
    }
}
