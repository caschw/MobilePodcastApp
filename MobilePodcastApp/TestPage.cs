﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace MobilePodcastApp
{
	public class TestPage : ContentPage
	{
		public TestPage ()
		{
			Content = new StackLayout {
				Children = {
					//new Label { Text = "Hello ContentPage" }
				}
			};
		}
	}
}
