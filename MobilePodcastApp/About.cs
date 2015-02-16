using Xamarin.Forms;
using Label = Xamarin.Forms.Label;

// ReSharper disable once CheckNamespace
namespace MobilePodcastApp
{
	public class AboutPage : ContentPage
	{
        public AboutPage()
		{
			Content = new StackLayout {
				Children = {
					new Label { Text = App.AppSettings.PodcastDescription }
				}
			};
		}
	}
}
