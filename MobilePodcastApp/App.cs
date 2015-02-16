using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace MobilePodcastApp
{
	public class App : Application
	{
        public static AppSettings AppSettings { get; set; }

		public App ()
		{
            //// The root page of your application
            //MainPage = new ContentPage {
            //    Content = new StackLayout {
            //        VerticalOptions = LayoutOptions.Center,
            //        Children = {
            //            new Label {
            //                XAlign = TextAlignment.Center,
            //                Text = "Welcome to Xamarin Forms!"
            //            }
            //        }
            //    }
            //};

            AppSettings = new AppSettings();

            MainPage = GetMainPage();
		}

	    public static Page GetMainPage()
	    {
	        var mainNav = new NavigationPage(new TabsPage());
	        return mainNav;
	    }

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
