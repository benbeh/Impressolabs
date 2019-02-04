using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using ImpressoApp.Droid.Services;
using ImpressoApp.Enums;
using ImpressoApp.Models.Authentication;
using ImpressoApp.Services.Facebook;
using Org.Json;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;
using Xamarin.Forms;

[assembly: Dependency(typeof(FacebookService))]
namespace ImpressoApp.Droid.Services
{
    public class FacebookService : Java.Lang.Object, IFacebookService, IFacebookCallback, GraphRequest.IGraphJSONObjectCallback, GraphRequest.ICallback
    {
        public static FacebookService Instance => DependencyService.Get<IFacebookService>() as FacebookService;

        readonly ICallbackManager callbackManager = CallbackManagerFactory.Create();
        readonly string[] permissions = { @"public_profile", @"email", @"user_status" };

        FacebookLoginResult loginResult;
        TaskCompletionSource<FacebookLoginResult> completionSource;

        public FacebookService()
        {
            LoginManager.Instance.RegisterCallback(callbackManager, this);
        }

        public Task<FacebookLoginResult> Login()
        {
            completionSource = new TaskCompletionSource<FacebookLoginResult>();
            LoginManager.Instance.LogInWithReadPermissions(Forms.Context as Activity, permissions);

            return completionSource.Task;
        }

        public void Logout()
        {
            LoginManager.Instance.LogOut();
        }

        public void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            callbackManager?.OnActivityResult(requestCode, resultCode, data);
        }

        public void OnCancel()
        {
            completionSource?.TrySetResult(new FacebookLoginResult { LoginState = FacebookLoginState.Canceled });
        }

        public void OnCompleted(JSONObject data, GraphResponse response)
        {
            OnCompleted(response);
        }

        public void OnCompleted(GraphResponse response)
        {
            if (response?.JSONObject == null)
                completionSource?.TrySetResult(new FacebookLoginResult { LoginState = FacebookLoginState.Canceled });
            else
            {
                loginResult = new FacebookLoginResult
                {
                    FirstName = Profile.CurrentProfile.FirstName,
                    LastName = Profile.CurrentProfile.LastName,
                    Email = response.JSONObject.Has("email") ? response.JSONObject.GetString("email") : string.Empty,
                    ImageUrl = response.JSONObject.GetJSONObject("picture")?.GetJSONObject("data")?.GetString("url"),
                    Token = AccessToken.CurrentAccessToken.Token,
                    UserId = AccessToken.CurrentAccessToken.UserId,
                    ExpireAt = FromJavaDateTime(AccessToken.CurrentAccessToken?.Expires?.Time),
                    LoginState = FacebookLoginState.Success
                };

                completionSource?.TrySetResult(loginResult);
            }
        }

        public void OnError(FacebookException error)
        {
            completionSource?.TrySetResult(new FacebookLoginResult
            {
                LoginState = FacebookLoginState.Failed,
                ErrorString = error.Message
            });
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            var facebookLoginResult = result.JavaCast<LoginResult>();
            if (facebookLoginResult == null) return;

            var parameters = new Bundle();
            parameters.PutString("fields", "id,email,picture.type(large)");
            var request = GraphRequest.NewMeRequest(facebookLoginResult.AccessToken, this);
            request.Parameters = parameters;
            request.ExecuteAsync();
        }

        static DateTimeOffset FromJavaDateTime(long? longTimeMillis)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return longTimeMillis != null ? epoch.AddMilliseconds(longTimeMillis.Value) : DateTimeOffset.MinValue;
        }
    }
}
