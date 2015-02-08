using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using aiwanshua.Resources;
using System.IO;
using System.Xml.Linq;
using Microsoft.Phone.Tasks;
using System.Diagnostics;
using 股票新闻;
using System.IO.IsolatedStorage;

namespace aiwanshua
{
    public partial class MainPage : PhoneApplicationPage
    {
        // 构造函数
    
        public MainPage()
        {
            InitializeComponent();
            Pclient.OpenReadCompleted += Pclient_OpenReadCompleted;
            Pclient.OpenReadAsync(new Uri("https://raw.githubusercontent.com/gisdaodao/MYPROJECT/master/data/yundongsahipindaquan.xml", UriKind.Absolute));
            indicator.Text = "请求中...";
            indicator.IsVisible = true;
            indicator.IsIndeterminate = true;
        }
        WebClient Pclient = new WebClient();
        List<string> urls = new List<string>();
        List<Info> items = new List<Info>();


        void Pclient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    Stream stream = e.Result;
                    XElement p = XElement.Load(stream);
                    //XName xname = XName.Get("url");
                    //IEnumerable<XElement> nodes = p.Descendants(xname).ToList<XElement>();
                    //foreach (var a in nodes)
                    //{
                    //    urls.Add(a.Value);
                    //}
                    // Deployment.Current.Dispatcher.BeginInvoke(() => { lstbox.ItemsSource = urls; });
                    XName xitemname = XName.Get("item");
                    IEnumerable<XElement> itemnodes = p.Descendants(xitemname).ToList<XElement>();
                    foreach (var b in itemnodes)
                    {
                        XName xname = XName.Get("url");
                          XName xpicname = XName.Get("picurl");
                          items.Add(new Info() { text = b.FirstAttribute.Value, info = b.LastAttribute.Value, dataurl = b.Descendants(xname).First().Value, picurl = b.Descendants(xpicname).First().Value });
                    }
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        lstbox.ItemsSource = items; indicator.IsVisible = false;
                        indicator.IsIndeterminate = false;
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }


        }
        IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.IsNavigationInitiator == false && e.NavigationMode == NavigationMode.Back)
            {
                int i = 1;
                if (!settings.Contains("ad"))
                {
                    settings.Add("ad", DateTime.Now.ToString("yyyy/mm/dd"));
                    settings.Save();
                    SetHIDE();
                }
                else
                {
                    string k = (string)settings["ad"];
                   if(k==DateTime.Now.ToString("yyyy/mm/dd"))
                   {
                       SetHIDE();
                   }
                }

            }
            if (e.NavigationMode == NavigationMode.New)
            {
                   if(settings.Contains("ad"))
                   {
                       string k = (string)settings["ad"];
                       if (k == DateTime.Now.ToString("yyyy/mm/dd"))
                       {
                           SetHIDE();
                       }
                   }
            }
            base.OnNavigatedTo(e);
        }

        private void SetHIDE()
        {
          //  googlead.Visibility = Visibility.Collapsed;
            //throw new NotImplementedException();
        }
        private void Border_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Border border = sender as Border;
            Info info = border.DataContext as Info;
            Debug.WriteLine(info.dataurl);
            WebBrowserTask task = new WebBrowserTask();
            task.Uri = new Uri(info.dataurl, UriKind.RelativeOrAbsolute);
            task.Show();

        }

        private void googlead_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            DateTime time = DateTime.Now;
            string str = time.ToString("yyyy/mm/dd");
            if(!settings.Contains("ad"))
            {
                settings.Add("ad", str);
                settings.Save();
            }
            else
            {
              if(str!=(string)settings["ad"])
              {
                  settings["ad"] = str;
                  settings.Save();
              }
            }
        }
        // 用于生成本地化 ApplicationBar 的示例代码
        //private void BuildLocalizedApplicationBar()
        //{
        //    // 将页面的 ApplicationBar 设置为 ApplicationBar 的新实例。
        //    ApplicationBar = new ApplicationBar();

        //    // 创建新按钮并将文本值设置为 AppResources 中的本地化字符串。
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // 使用 AppResources 中的本地化字符串创建新菜单项。
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}