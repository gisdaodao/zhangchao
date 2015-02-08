﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Tasks;

namespace OKr.MXReader.Client.Core
{
    public class OKrHelper
    {
        public static void Download(string appId)
        {
            if (!string.IsNullOrEmpty(appId))
            {
                MarketplaceDetailTask task = new MarketplaceDetailTask();
                task.ContentIdentifier = appId;
                task.Show();

                //IAppService service = new AppService();
                //service.Download(appId, uid, x =>
                //{

                //}, x => { });
            }
            else
            {
                OkrToast.Show("近期上线，敬请期待。");
            }
        }

        public static void Feedback(string app, string version, string email)
        {
            string subject = "【" + app + " v" + version + "feedback";
            string body = "";

            EmailComposeTask task = new EmailComposeTask();
            task.Subject = subject;
            task.Body = body;
            task.To = "nan06jing06ok@hotmail.com";
            task.Show();
        }

        public static void Comment()
        {
            MarketplaceReviewTask task = new MarketplaceReviewTask();
            task.Show();
        }

        public static void Share(string app, string appId)
        {
            string subject = "" + app;
            string body;

            if (string.IsNullOrEmpty(appId))
            {
                body = "【" + app + "】。" + "\n\n" + "";
            }
            else
            {
                body = "【" + app + "】" + "http://windowsphone.com/s?appid=" + appId + "\n\n" + "]";
            }

            EmailComposeTask task = new EmailComposeTask();
            task.Subject = subject;
            task.Body = body;
            task.Show();
        }

        public static void Dowith(string app, string email)
        {
            string subject = "希望更多合作";
            string body = "你好，" + "\n\n" + "我是【" + app + "】用户，希望有机会建立广告和开发相关合作：";
            EmailComposeTask task = new EmailComposeTask();
            task.Subject = subject;
            task.Body = body;
            task.To = email;
            task.Show();
        }
    }
}
