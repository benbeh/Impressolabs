using System.Threading.Tasks;
using CoreGraphics;
using Facebook.CoreKit;
using Facebook.LoginKit;
using Foundation;
using ImpressoApp.Enums;
using ImpressoApp.iOS.Services;
using ImpressoApp.Models.Authentication;
using ImpressoApp.Services.Facebook;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: Dependency(typeof(FacebookService))]
namespace ImpressoApp.iOS.Services
{
    public class FacebookService : IFacebookService
    {
        readonly LoginManager loginManager = new LoginManager();
        readonly string[] permissions = { @"email" };

        FacebookLoginResult loginResult;
        TaskCompletionSource<FacebookLoginResult> completionSource;

        public Task<FacebookLoginResult> Login()
        {
            completionSource = new TaskCompletionSource<FacebookLoginResult>();
            loginManager.LogInWithReadPermissions(permissions, GetCurrentViewController(), LoginManagerLoginHandler);

            return completionSource.Task;
        }

        public void Logout()
        {
            loginManager.LogOut();
        }

        void LoginManagerLoginHandler(LoginManagerLoginResult result, NSError error)
        {
            if (result.IsCancelled)
                completionSource.TrySetResult(new FacebookLoginResult { LoginState = FacebookLoginState.Canceled });
            else if (error != null)
                completionSource.TrySetResult(new FacebookLoginResult { LoginState = FacebookLoginState.Failed, ErrorString = error.LocalizedDescription });
            else
            {
                loginResult = new FacebookLoginResult
                {
                    Token = result.Token.TokenString,
                    UserId = result.Token.UserID,
                    ExpireAt = result.Token.ExpirationDate.ToDateTime()
                };

                var request = new GraphRequest(@"me", new NSDictionary(@"fields", @"email"));
                request.Start(GetEmailRequestHandler);
            }
        }

        void GetEmailRequestHandler(GraphRequestConnection connection, NSObject result, NSError error)
        {
            if (error != null)
                completionSource.TrySetResult(new FacebookLoginResult { LoginState = FacebookLoginState.Failed, ErrorString = error.LocalizedDescription });
            else
            {
                loginResult.FirstName = Profile.CurrentProfile?.FirstName;
                loginResult.LastName = Profile.CurrentProfile?.LastName;
                loginResult.ImageUrl = Profile.CurrentProfile?.ImageUrl(ProfilePictureMode.Square, new CGSize()).ToString();

                var dict = result as NSDictionary;
                var emailKey = new NSString(@"email");
                if (dict != null && dict.ContainsKey(emailKey))
                    loginResult.Email = dict[emailKey]?.ToString();

                loginResult.LoginState = FacebookLoginState.Success;
                completionSource.TrySetResult(loginResult);
            }
        }

        static UIViewController GetCurrentViewController()
        {
            var viewController = UIApplication.SharedApplication.KeyWindow.RootViewController;
            while (viewController.PresentedViewController != null)
                viewController = viewController.PresentedViewController;

            return viewController;
        }


    }
}
