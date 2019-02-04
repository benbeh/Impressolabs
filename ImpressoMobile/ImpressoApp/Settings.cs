using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace ImpressoApp
{
    public static class Settings
    {
        private static ISettings AppSettings => CrossSettings.Current;

        #region Setting Constants
        private const string UserIDKey = "UserID";
        private const string TokenKey = "Token";
        private const string UserNameKey = "UserName";
        private const string UserPositionKey = "UserPosition";
        private const string UserProfileImageKey = "UserProfileImage";
        private const string DoNotShowAgainEmailKey = "DoNotShowAgainEmail";

        private static readonly string DefaultStringValue = string.Empty;
        #endregion

        public static void ClearSettings()
        {
            UserID = DefaultStringValue;
            Token = DefaultStringValue;
            UserName = DefaultStringValue;
            UserProfileImage = DefaultStringValue;
            UserPosition = DefaultStringValue;
            DoNotShowAgainEthereumDialogEmail = DefaultStringValue;
        }

        public static string UserID
        {
            get => AppSettings.GetValueOrDefault(UserIDKey, DefaultStringValue);
            set => AppSettings.AddOrUpdateValue(UserIDKey, value);
        }


        public static string Token
        {
            get => AppSettings.GetValueOrDefault(TokenKey, DefaultStringValue);
            set => AppSettings.AddOrUpdateValue(TokenKey, value);
        }

        public static string UserName
        {
            get => AppSettings.GetValueOrDefault(UserNameKey, DefaultStringValue);
            set => AppSettings.AddOrUpdateValue(UserNameKey, value);
        }

        public static string UserProfileImage
        {
            get => AppSettings.GetValueOrDefault(UserProfileImageKey, DefaultStringValue);
            set => AppSettings.AddOrUpdateValue(UserProfileImageKey, value);
        }

        public static string UserPosition
        {
            get => AppSettings.GetValueOrDefault(UserPositionKey, DefaultStringValue);
            set => AppSettings.AddOrUpdateValue(UserPositionKey, value);
        }

        public static string DoNotShowAgainEthereumDialogEmail
        {
            get => AppSettings.GetValueOrDefault(DoNotShowAgainEmailKey, DefaultStringValue);
            set => AppSettings.AddOrUpdateValue(DoNotShowAgainEmailKey, value);
        }
    }
}
