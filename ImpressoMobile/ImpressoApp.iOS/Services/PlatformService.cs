using System;
using Foundation;
using ImpressoApp.iOS.Services;
using ImpressoApp.Services;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(PlatformService))]
namespace ImpressoApp.iOS.Services
{
    public class PlatformService : IPlatformService
    {
        public PlatformService()
        {
        }

        public bool GoToInbox()
        {
            var url = NSUrl.FromString("message://");
            if (UIApplication.SharedApplication.CanOpenUrl(url))
            {
                UIApplication.SharedApplication.OpenUrl(url);
                return true;
            }

            return false;
        }
    }
}
