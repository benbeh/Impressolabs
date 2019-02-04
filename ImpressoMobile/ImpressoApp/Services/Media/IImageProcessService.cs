using System;
using Java.Security;
using System.Threading.Tasks;
namespace ImpressoApp.Services.Media
{
    public interface IImageProcessService
    {
        byte[] GetResizedImageBytes(byte[] input, float desireWidth, float desireHeight);
    }
}
