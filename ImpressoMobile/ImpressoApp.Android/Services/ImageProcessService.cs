using System;
using ImpressoApp.Droid.Services;
using Xamarin.Forms;
using ImpressoApp.Services.Media;
using System.Threading.Tasks;
using Android.Graphics;
using System.IO;

[assembly: Dependency(typeof(ImageProcessService))]
namespace ImpressoApp.Droid.Services
{
    public class ImageProcessService : IImageProcessService
    {
        public byte[] GetResizedImageBytes(byte[] input, float desireWidth, float desireHeight)
        {
                var image = BitmapFactory.DecodeByteArray(input, 0, input.Length);

                int scaleFactor = (int)(image.Width / desireWidth);
                int resizeWidth = (int)(image.Width / scaleFactor);
                int resizeHeight = (int)(image.Height / scaleFactor);

                var resized = Bitmap.CreateScaledBitmap(image,resizeWidth, resizeHeight, false);

                using(MemoryStream stream = new MemoryStream())
                {
                    resized.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);

                    return stream.ToArray();
                }
        }
    }
}
