using System;
using System.Threading.Tasks;

namespace ImpressoApp.Services.Media
{
    public interface IMediaPickerService
    {
        Task<string> TakePictureFromLibrary();
        Task<string> TakePictureFromCamera();
    }
}
