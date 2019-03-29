using System;
using System.Windows.Media.Imaging;

namespace WoofWoof.Helpers
{
	public static class ImageHelper
	{
		public static BitmapImage Load(string imageName)
		{
			var image = new BitmapImage(new Uri(imageName, UriKind.RelativeOrAbsolute));
			return image;
		}
	}
}