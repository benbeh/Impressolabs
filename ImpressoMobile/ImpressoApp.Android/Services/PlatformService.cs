using System;
using Android.Content;
using ImpressoApp.Droid.Services;
using ImpressoApp.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(PlatformService))]
namespace ImpressoApp.Droid.Services
{
    public class PlatformService : IPlatformService
    {
        public PlatformService()
        {
        }

        public bool GoToInbox()
        {
            Intent intent = Intent.MakeMainSelectorActivity(Intent.ActionMain, Intent.CategoryAppEmail);
            intent.AddFlags(ActivityFlags.NewTask);

            Android.App.Application.Context.StartActivity(intent);

            return true;
        }
    }
}
