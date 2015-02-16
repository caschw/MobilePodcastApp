using Xamarin.Forms;

// ReSharper disable once CheckNamespace
namespace MobilePodcastApp
{
	public class App : Application
	{
        public static AppSettings AppSettings { get; set; }

		public App ()
		{
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
