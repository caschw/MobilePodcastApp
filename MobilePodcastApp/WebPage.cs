using System;
using Xamarin.Forms;

// ReSharper disable once CheckNamespace
namespace MobilePodcastApp
{
	public class WebPage : ContentPage
	{
	    private readonly WebView _webview;

		public WebPage ()
		{
            _webview = new WebView();
		    Content = _webview;
		}

	    public Uri StartPage
	    {
	        set { _webview.Source = value.ToString(); }
	    }
	}
}
