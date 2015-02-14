using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using OKr.MXReader.Client.Core.Data;
using System.IO.IsolatedStorage;
using OKr.MXReader.Client.Core;
using OKr.MXReader.Client.View.Shared;
using OKr.MXReader.Client.Core.Context;
using At.Phone.Common.Utils;
using GoogleAds;
using SurfaceAd.SDK.WP;
using Microsoft.Phone.Tasks;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;
using 股票新闻;
using System.Windows.Navigation;

namespace OKr.MXReader.Client.View
{
    public partial class Home : PhoneApplicationPage
    {
        private InterstitialAd interstitialAd;
        WebClient txtPclient = new WebClient();
          WebClient groupclientPclient = new WebClient();
          WebClient othermusicclient = new WebClient();
        IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
        List<Info> txtinfo = new List<Info>();
        List<Info> groupmusicinfo = new List<Info>();
        string gourpmusic="https://raw.githubusercontent.com/commonusechina/data/master/data/groupmusic.xml";
        List<Info> othermusic = new List<Info>();
        string oterhmusic = "https://raw.githubusercontent.com/commonusechina/data/master/data/othermusic.xml";
        public Home()
        {
            InitializeComponent();
            interstitialAd = new InterstitialAd("ca-app-pub-1598808565430684/4412492859");
            // NOTE: You can edit the event handler to do something custom here. Once the
            // interstitial is received it can be shown whenever you want.
            interstitialAd.ReceivedAd += OnAdReceived;
            AdRequest adRequest = new AdRequest();
            adRequest.ForceTesting = false;
            interstitialAd.LoadAd(adRequest);
            base.Loaded += new RoutedEventHandler(this.OnLoaded);
           // ad.Start();http://tieba.baidu.com/f?kw
            groupclientPclient.OpenReadCompleted += groupclientPclient_OpenReadCompleted;
            groupclientPclient.OpenReadAsync(new Uri(gourpmusic, UriKind.RelativeOrAbsolute));
            //groups.Add(new Info() { text = "歌唱吧", dataurl = "http://tieba.baidu.com/f?kw=歌唱" });
            //groups.Add(new Info() { text = "", dataurl = "" }); ;
            //groups.Add(new Info() { text = "", dataurl = "" }); ;
            //groups.Add(new Info() { text = "", dataurl = "" }); ;
            //groups.Add(new Info() { text = "", dataurl = "" }); ;
            //groups.Add(new Info() { text = "", dataurl = "" }); ;
            //groups.Add(new Info() { text = "", dataurl = "" }); ;
            //groups.Add(new Info() { text = "", dataurl = "" }); ;
            //groups.Add(new Info() { text = "", dataurl = "" }); ;
            //groups.Add(new Info() { text = "", dataurl = "" }); ;
           
            this.surfaceAdImageXaml.InitAdControl(AdModeType.Normal); 
        }

        void groupclientPclient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    Stream stream = e.Result;
                    XElement p = XElement.Load(stream);
                    XName xitemname = XName.Get("item");
                    IEnumerable<XElement> itemnodes = p.Descendants(xitemname).ToList<XElement>();
                    foreach (var b in itemnodes)
                    {
                        XName xname = XName.Get("url");
                        //   XName xpicname = XName.Get("picurl");
        //List<Info> groupmusicinfo = new List<Info>();
        groupmusicinfo.Add(new Info() { text = b.FirstAttribute.Value, info = b.LastAttribute.Value, dataurl = b.Descendants(xname).First().Value, });
                    }
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {

                        otherlonglist.ItemsSource = groupmusicinfo; indicator.IsVisible = false;
                        indicator.IsIndeterminate = false;
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        List<Info> groups = new List<Info>();
     private void OnAdReceived(object sender, AdEventArgs e)
        {
            Random p = new Random();
            int j = p.Next(1, 100);
         if(j>=88)
         {
             interstitialAd.ShowAd();
         }          
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if(pano.SelectedIndex==3)
            {
                e.Cancel = true;
                return;
            }
            if (pano.SelectedIndex == 7)
            {
                if(wbsearch.CanGoBack)
                {
                    wbsearch.GoBack();
                    e.Cancel = true;
                    return;
                }
              
            }
            NavHelper.Quit(this);

            //if (MessageBox.Show("确定退出" + OkrBookContext.Current.Config.Name + "吗？", OkrBookContext.Current.Config.Name, MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            //{
            //    e.Cancel = true;
            //}
            //else
            //{
            //    NavHelper.Quit(this);
            //}
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.loader.Start();

            IsolatedStorageSettings.ApplicationSettings.TryGetValue<Book>("bookinfo", out this.book);
            if (this.book == null)
            {
                this.book = TextParser.GetBook(OkrBookContext.Current.Config.Data +"/category.txt", UriKind.Relative);
                IsolatedStorageSettings.ApplicationSettings["bookinfo"] = this.book;
            }

            Progress progress = null;

            IsolatedStorageSettings.ApplicationSettings.TryGetValue<Progress>("current", out progress);
            if (progress == null)
            {
                progress = new Progress();
                progress.Chapter = 0;
                progress.Page = 0;
                progress.Percent = 0.0;
                IsolatedStorageSettings.ApplicationSettings["current"] = progress;
            }

            this.Init();
        }

        private void Init()
        {
            clist.Items.Clear();

            this.tbIntro.Text = OkrBookContext.Current.Config.Intro;
            this.tbName.Text = OkrBookContext.Current.Config.Name;
            this.tbAuthor.Text = OkrBookContext.Current.Config.Author;

            this.LoadChapter();
            this.LoadMarks();

            this.loader.Stop();
        }

        private void OnPanoChanged(object sender, SelectionChangedEventArgs e)
        {
            //switch (this.pano.SelectedIndex)
            //{
            //    case 1:
            //        LoadChapter();
            //        break;
            //    case 2:
            //        LoadMarks();
            //        break;
            //    default:
            //        break;
            //}
        }

        private void OnShow(object sender, RoutedEventArgs e)
        {
            isguanggao = false;

            NavigationService.Navigate(new Uri("/View/Viewer.xaml", UriKind.RelativeOrAbsolute));
        }

        private void OnBooks(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/Apps.xaml", UriKind.RelativeOrAbsolute));
        }

        private void OnComment(object sender, RoutedEventArgs e)
        {
            OKrHelper.Comment();
        }

        private void OnFeedback(object sender, RoutedEventArgs e)
        {
            OKrHelper.Feedback(OkrBookContext.Current.App.AppName, OkrBookContext.Current.App.Version, OkrBookContext.Current.App.Email);
        }

        private void OnShare(object sender, RoutedEventArgs e)
        {
            string appid = OkrBookContext.Current.App.AppId;

            if (string.IsNullOrEmpty(appid))
            {
                appid = WMAppManifestUtils.GetWMAppManifest().ProductID;
            }

            OKrHelper.Share(OkrBookContext.Current.App.AppName, OkrBookContext.Current.App.AppId);
        }

        private void OnSetting(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/Setting.xaml", UriKind.RelativeOrAbsolute));
        }

        private void OnAbout(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/About.xaml", UriKind.RelativeOrAbsolute));
        }

        private void LoadChapter()
        {
            int index = 0;
            foreach (var chapter in this.book.Chapters)
            {
                ChapterItem item = new ChapterItem();
                chapter.ChapterNo = index;
                item.DataContext = chapter;

                clist.Items.Add(item);
                index++;
            }
        }

        private void LoadMarks()
        {
            mlist.Items.Clear();
            this.mark = GetMark();

            foreach (var mark in this.mark.Marks)
            {
                ChapterMark tmp = mark;
                BookmarkItem item = new BookmarkItem();
                item.DataContext = tmp;

                item.Click += (sender, ex) =>
                {
                    Progress progress = null;

                    IsolatedStorageSettings.ApplicationSettings.TryGetValue<Progress>("current", out progress);
                    if (progress == null)
                    {
                        progress = new Progress();
                    }
                    progress.Chapter = tmp.ChapterNo;
                    progress.Page = tmp.Current;
                    progress.Percent = tmp.Percent;
                    IsolatedStorageSettings.ApplicationSettings["current"] = progress;

                    Dispatcher.BeginInvoke(() =>
                    {
                        (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/View/Viewer.xaml", UriKind.Relative));
                    });
                };

                this.mlist.Items.Add(item);
            }
        }
        private void txtborder_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Border border = sender as Border;
            Info info = border.DataContext as Info;
            //this.NavigationService.Navigate(new Uri("/VideoPage.xaml?url="+info.dataurl, UriKind.RelativeOrAbsolute));
            //Debug.WriteLine(info.dataurl);
            //App.tranferinfo = info;
            isguanggao = false;
            // return;
            WebBrowserTask task = new WebBrowserTask();
            task.Uri = new Uri(info.dataurl, UriKind.RelativeOrAbsolute);
            task.Show();
           
            //Debug.WriteLine(info.dataurl);
        }

        private Mark GetMark()
        {
            //todo: {WT}, change the way of marks, record marks in DB
            Mark result = null;

            IsolatedStorageSettings.ApplicationSettings.TryGetValue<Mark>("marks", out result);
            if (result == null)
            {
                result = new Mark();
            }

            return result;
        }

        private Book book;
        private Mark mark;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button border = sender as Button;
            string dataurl = border.Tag.ToString();
            isguanggao = false;
            WebBrowserTask task = new WebBrowserTask();
            task.Uri = new Uri(dataurl, UriKind.RelativeOrAbsolute);
            task.Show();
        }

        private void pano_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (pano.SelectedIndex == 3 && txtlstbox.ItemsSource == null)
            {
                if (txtPclient.IsBusy) return;
                txtPclient.OpenReadCompleted += txtPclient_OpenReadCompleted;
                txtPclient.OpenReadAsync(new Uri("https://raw.githubusercontent.com/commonusechina/data/master/data/music.xml", UriKind.Absolute));

                indicator.Text = "请求中...";
                indicator.IsVisible = true;
                indicator.IsIndeterminate = true;
            }
            if (pano.SelectedIndex == 6&& longlist.ItemsSource == null)
            {
                if (othermusicclient.IsBusy) return;
                othermusicclient.OpenReadCompleted += othermusicclient_OpenReadCompleted;
                othermusicclient.OpenReadAsync(new Uri(oterhmusic, UriKind.Absolute));

                indicator.Text = "请求中...";
                indicator.IsVisible = true;
                indicator.IsIndeterminate = true;
            }
        }

        void othermusicclient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    Stream stream = e.Result;
                    XElement p = XElement.Load(stream);
                    XName xitemname = XName.Get("item");
                    IEnumerable<XElement> itemnodes = p.Descendants(xitemname).ToList<XElement>();
                    foreach (var b in itemnodes)
                    {
                        XName xname = XName.Get("url");
                        //   XName xpicname = XName.Get("picurl");
                        othermusic.Add(new Info() { text = b.FirstAttribute.Value, info = b.LastAttribute.Value, dataurl = b.Descendants(xname).First().Value, });
                    }
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {

                        longlist.ItemsSource = othermusic; indicator.IsVisible = false;
                        indicator.IsIndeterminate = false;
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void txtPclient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    Stream stream = e.Result;
                    XElement p = XElement.Load(stream);
                    XName xitemname = XName.Get("item");
                    IEnumerable<XElement> itemnodes = p.Descendants(xitemname).ToList<XElement>();
                    foreach (var b in itemnodes)
                    {
                        XName xname = XName.Get("url");
                        //   XName xpicname = XName.Get("picurl");
                        txtinfo.Add(new Info() { text = b.FirstAttribute.Value, info = b.LastAttribute.Value, dataurl = b.Descendants(xname).First().Value, });
                    }
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {

                        txtlstbox.ItemsSource = txtinfo; indicator.IsVisible = false;
                        indicator.IsIndeterminate = false;
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        bool isguanggao = true;
     
        protected override void OnNavigatingFrom(System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            //Debug.WriteLine(e.Uri);
            //if(e.Uri.AbsoluteUri=="")
            //{
            //    isguanggao = true;
            //}
            base.OnNavigatingFrom(e);
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            try
            {
                if (e.IsNavigationInitiator == false && e.NavigationMode == NavigationMode.Back)
                {
                    //if (isguanggao)
                    //{
                    //    int i = 1;
                    //    if (!settings.Contains("number"))
                    //    {
                    //        settings.Add("number", i);
                    //        settings.Save();
                    //    }
                    //    else
                    //    {
                    //        int k = (int)settings["number"];
                    //        k = k + i;
                    //        settings["number"] = k;
                    //        settings.Save();
                    //    }
                    //}
                    if (!settings.Contains("adt"))
                    {
                        settings.Add("adt", "1");
                        settings.Save();
                        SetHIDE();
                    }
                    else
                    {
                        string k = (string)settings["adt"];
                        SetHIDE();
                    }
                }
                if (e.NavigationMode == NavigationMode.New)
                {
                    //if (!settings.Contains("number"))
                    //{
                    //    settings.Add("number", 0);
                    //    settings.Save();
                    //}
                    //else
                    //{
                    //    int k = (int)settings["number"];
                    //    if (k >= 5)
                    //    {
                    //        SetHIDE();
                    //    }
                    //    //k = k + i;
                    //    //settings["number"] = k;
                    //    // settings.Save();
                    //}
                    if (!settings.Contains("adt"))
                    {
                        //settings.Add("ad", "1");
                        //settings.Save();
                      //  SetHIDE();
                    }
                    else
                    {
                        string k = (string)settings["adt"];
                        if(k=="1")
                        {
                            SetHIDE();
                        }
                      
                    }
                }
                base.OnNavigatedTo(e);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            
        }
        private void Ad1Control_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Ad15Control.Visibility = Visibility.Collapsed;
        }
        private void AdtapControl_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            UIElement uelent = sender as UIElement;
            uelent.Visibility = Visibility.Collapsed;
        }

        private void Ad1Control_AdCompleted(object sender, EventArgs e)
        {

        }
        private void SetHIDE()
        {
            tb.Visibility = Visibility.Collapsed;
           // AdControl.Visibility = Visibility.Collapsed;
           // Ad1Control.Visibility = Visibility.Collapsed;
          reviewbtn.Visibility=Visibility.Collapsed;
           // Ad2Control.Visibility = Visibility.Collapsed;
            //Ad3Control.Visibility = Visibility.Collapsed;
          
          //  Ad5Control.Visibility = Visibility.Collapsed;
            Ad6Control.Visibility = Visibility.Collapsed;
            Ad9Control.Visibility = Visibility.Collapsed;
            //Ad10Control.Visibility = Visibility.Collapsed;
            surfaceAdImageXaml.Visibility = Visibility.Collapsed;
          //  ContentPanel.Children.Remove(AdControl);
           // adpanel.Children.Remove(Ad1Control);
           // adpanel.Children.Remove(Ad2Control);
         //   spAbout.Children.Remove(Ad5Control);
        }
        bool  isreview = false;

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MarketplaceReviewTask p = new MarketplaceReviewTask();
            p.Show();
           // isreview = true;
        }

        private void searbtn_Click(object sender, RoutedEventArgs e)
        {
            string str = "http://baike.baidu.com/search/word?word=" + box.Text;
            wbsearch.Navigate(new Uri(str,UriKind.RelativeOrAbsolute));
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            //if (btn.Content.ToString() == "音乐")
            //{
                string str = "http://baike.baidu.com/fenlei/"+btn.Content.ToString();
               // wbbaike.Navigate(new Uri(str,UriKind.RelativeOrAbsolute));
            //}
        }

        private void searwenkubtn_Click(object sender, RoutedEventArgs e)
        {
             string str = "http://wapwenku.baidu.com/search?word=" + box.Text;
            wbsearch.Navigate(new Uri(str,UriKind.RelativeOrAbsolute));
        }

        private void musicbtn_Click(object sender, RoutedEventArgs e)
        {
            string str = "http://en.wikipedia.org/wiki/Category:Music";
            wbsearch.Navigate(new Uri(str, UriKind.RelativeOrAbsolute));
        }


    }
}