using System;
using ImpressoApp.iOS.Services;
using Xamarin.Forms;
using ImpressoApp.Services.Media;
using System.Threading.Tasks;
using UIKit;
using Foundation;
using CoreGraphics;
using System.Drawing;

[assembly: Dependency(typeof(ImageProcessService))]
namespace ImpressoApp.iOS.Services
{
    public class ImageProcessService : IImageProcessService
    {
        public byte[] GetResizedImageBytes(byte[] input, float desireWidth, float desireHeight)
        { 
            UIImage image = new UIImage(NSData.FromArray(input));

            nint scaleFactor = (nint)(image.Size.Width / desireWidth);
            nint resizeWidth = (nint)(image.Size.Width / scaleFactor);
            nint resizeHeight = (nint)(image.Size.Height / scaleFactor);

            var scaledImage = image.Scale(new CGSize(resizeWidth, resizeHeight));

            return scaledImage.AsJPEG().ToArray();
        }
    }
}
