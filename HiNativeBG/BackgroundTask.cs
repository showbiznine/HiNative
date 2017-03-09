using HiNativeShared.API.Models;
using HiNativeShared.Services;
using Microsoft.QueryStringDotNET;
using Microsoft.Toolkit.Uwp.Notifications;
using NotificationsExtensions.Badges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Notifications;

namespace HiNativeBG
{
    public sealed class BackgroundTask : IBackgroundTask
    {
        BackgroundTaskDeferral _deferral;
        int _lastID;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            _deferral = taskInstance.GetDeferral();
            var unreadCount = await DataService.GetUnreadCount();
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            //localSettings.Values["to_ignore"] = 0; //Init
            _lastID = (localSettings.Values["to_ignore"] != null ? (int)localSettings.Values["to_ignore"] : 0);

            if (unreadCount.unread_count > 0)
            {
                var activities = await DataService.GetNotifications(1);
                for (int i = 0; i < unreadCount.unread_count; i++)
                {
                    if (activities.activities[i].id == _lastID)
                    {
                        break;
                    }
                    PopToast(activities.activities[i]);
                    SendBadge((uint)unreadCount.unread_count);
                    //PopTile(activities.activities[i]);
                    localSettings.Values["to_ignore"] = activities.activities[0].id;
                }
            }
            else
            {
                ClearBadge();
            }

            _deferral.Complete();
        }

        private void PopToast(HNActivity activity)
        {
            string type = GetActivityType(activity.payload.activity_type);
            string body = DataService.GetBodyText(activity);

            #region Toast Visual
            ToastVisual visual = new ToastVisual()
            {
                BindingGeneric = new ToastBindingGeneric()
                {
                    Children =
                    {
                        new AdaptiveText() {Text = activity.action_user_name +
                                                    type + body},
                    },
                    AppLogoOverride = new ToastGenericAppLogo()
                    {
                        Source = activity.action_user_image_url,
                        HintCrop = ToastGenericAppLogoCrop.Circle
                    },                }
            };
            #endregion

            ToastContent toastContent = new ToastContent()
            {
                Visual = visual,
                ActivationType = ToastActivationType.Foreground,
                Launch = new QueryString()
                {
                    { "type", activity.payload.activity_type },
                    { "id", activity.payload.id.ToString() },
                }.ToString()
            };

            var toast = new ToastNotification(toastContent.GetXml());
            toast.Tag = activity.id.ToString();
            toast.Group = activity.payload.id.ToString();
            toast.NotificationMirroring = NotificationMirroring.Allowed;
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

    }
}
