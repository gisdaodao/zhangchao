using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace OKr.MXReader.Client.View
{
    public partial class PageBa : PhoneApplicationPage
    {
        public PageBa()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
         string str=    this.NavigationContext.QueryString["name"];
         wb.Navigate(new Uri(str, UriKind.RelativeOrAbsolute));
            base.OnNavigatedTo(e);
        }
       // protected
   
    }
}