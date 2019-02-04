using System;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;
using FFImageLoading;

namespace ImpressoApp.Services.Media
{
    public class MediaPickerService : IMediaPickerService
    {
        private readonly IImageProcessService imageService;

        public MediaPickerService(IImageProcessService imageService)
        {
            this.imageService = imageService;
        }

        public async Task<string> TakePictureFromLibrary()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                throw new NotSupportedException("This device is not supporting photo picking");
            }

            var file = await CrossMedia.Current.PickPhotoAsync();

            if (file == null)
            {
                throw new ArgumentException("File not found");
            }

            return await GetBase64Image(file);
        }

        public async Task<string> TakePictureFromCamera()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                throw new NotSupportedException("This device's camera is not available");
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = "ImpressoImages",
                Name = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString()
            });

            if (file == null)
            {
                throw new ArgumentException("File not found");
            }

            return await GetBase64Image(file);
        }

        private async Task<string> GetBase64Image(MediaFile file)
        {
            var stream = file.GetStream();
            var bytes = new byte[stream.Length];
            await stream.ReadAsync(bytes, 0, (int)stream.Length);
            var resizedBytes = imageService.GetResizedImageBytes(bytes, 500, 500);
            return System.Convert.ToBase64String(resizedBytes);
        }
    }
}
