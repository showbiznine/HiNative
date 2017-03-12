using HiNative.API.Models;
using HiNative.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.QueryStringDotNET;
using Windows.UI.Notifications;
using NotificationsExtensions.Badges;

namespace HiNative.Services
{
    public class NotificationService
    {
        public static void PopToast(HNActivity activity)
        {
            try
            {
                string type = DataService.GetActivityType(activity.payload.activity_type);
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
                        },
                    }
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
            catch (Exception)
            {
                LoggerService.LogEvent("Toast_failed");
            }
        }

        public static void SendBadge(uint count)
        {
            BadgeNumericNotificationContent badgeContent = new BadgeNumericNotificationContent(count);
            BadgeNotification notif = new BadgeNotification(badgeContent.GetXml());
            BadgeUpdateManager.CreateBadgeUpdaterForApplication().Update(notif);
        }

        public static void ClearBadge()
        {
            BadgeUpdateManager.CreateBadgeUpdaterForApplication().Clear();
        }
    }
}
