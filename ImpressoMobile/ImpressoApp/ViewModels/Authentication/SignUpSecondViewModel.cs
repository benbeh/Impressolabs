using System;
using BaseMvvmToolkit.ViewModels;
using BaseMvvmToolkit.Services;
using ImpressoApp.Services.Media;
using ImpressoApp.Services.AuthenticationService;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;
using ImpressoApp.Models;
using ImpressoApp.Exceptions;

namespace ImpressoApp.ViewModels.Authentication
{
    public class SignUpSecondViewModel : BaseViewModel
    {
        private const string ProfilePhotoHeaderName = "Upload your photo";
        private const string TakePhotoButtonName = "Take from camera";
        private const string ChooseFromLibraryButtonName = "Choose from library";
        private const string DeletePhotoButtonName = "Delete a photo";
        private const string CancelButtonName = "Cancel";

        private readonly IAuthenticationService authenticationService;
        private readonly IMediaPickerService mediaPickerService;

        public UserInfoModel UserInfoModel { get; set; }
        public bool IsCompany { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string UserProfileImagePath { get; set; }

        public ICommand ContinueCommand { get; private set; }
        public ICommand UploadPhotoCommand { get; private set; }
        public ICommand BackCommand { get; private set; }

        public SignUpSecondViewModel(INavigationService navigationService,
                               IAuthenticationService authenticationService,
                               IMediaPickerService mediaPickerService) : base (navigationService)
        {
            this.authenticationService = authenticationService;
            this.mediaPickerService = mediaPickerService;

            UploadPhotoCommand = new Command(UploadPhotoCommandExecute);
            ContinueCommand = new Command(ContinueCommandExecute);
            BackCommand = new Command(BackCommandExecute);
        }

        private void BackCommandExecute(object obj)
        {

            NavigationService.PopAsync();
        }

        public override Task Init(object args)
        {
            UserInfoModel = args as UserInfoModel;

            IsCompany = UserInfoModel.UserType == Enums.UserType.Business;

            return base.Init(args);
        }

        private async void ContinueCommandExecute(object obj)
        {
            if(UserInfoModel == null)
            {
                return;
            }

            if ((UserInfoModel.UserType == Enums.UserType.Person && string.IsNullOrWhiteSpace(FirstName)) || 
                (UserInfoModel.UserType == Enums.UserType.Business && string.IsNullOrWhiteSpace(CompanyName)))
            {
                await NavigationService.DisplayAlert("Warning", "All data should be filled.", "Ok");
                return;
            }
            
            try
            {
                IsBusy = true;

                UserInfoModel.UserName = string.Format("{0} {1}", FirstName, LastName);

                var result = await authenticationService.SignUpAsync(FirstName,
                                                                     LastName,
                                                                     CompanyName,
                                                                     UserInfoModel.Email, 
                                                                     UserInfoModel.Password, 
                                                                     UserInfoModel.Password, 
                                                                     (int)UserInfoModel.UserType,
                                                                     UserInfoModel.UserProfileImage);

                IsBusy = false;

                await NavigationService.NavigateToAsync<FinishSignUpViewModel>(UserInfoModel);
            }
            catch (ServiceAuthenticationException)
            {
                await NavigationService.DisplayAlert("Error", "Error authorization. Please verify your credentials and try again.", "Ok");
            }
            catch (Exception ex)
            {
                await NavigationService.DisplayAlert("Error", "Sign up error. Check internet connection and try again.", "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void UploadPhotoCommandExecute(object obj)
        {
            var selectedOption = await NavigationService.DisplayActionSheet(
                ProfilePhotoHeaderName,
                CancelButtonName,
                DeletePhotoButtonName,
                new[] {
                    ChooseFromLibraryButtonName,
                    TakePhotoButtonName
                });

            switch (selectedOption)
            {
                case CancelButtonName:
                    break;
                case DeletePhotoButtonName:
                    UserProfileImagePath = null;
                    break;
                case ChooseFromLibraryButtonName:
                    TakePictureFromLibrary();
                    break;
                case TakePhotoButtonName:
                    TakePictureFromCamera();
                    break;
            }
        }

        private async void TakePictureFromLibrary()
        {
            try
            {
                UserProfileImagePath = await mediaPickerService.TakePictureFromLibrary();
                UserInfoModel.UserProfileImage = UserProfileImagePath;
            }
            catch (NotSupportedException)
            {
                await NavigationService.DisplayAlert("Error", "Not supported on this device.", "Ok");
            }
            catch (System.Exception ex)
            {
                await NavigationService.DisplayAlert("Error", "Unable to take a photo.", "Ok");
            }
        }

        private async void TakePictureFromCamera()
        {
            try
            {
                UserProfileImagePath = await mediaPickerService.TakePictureFromCamera();

            }
            catch (NotSupportedException)
            {
                await NavigationService.DisplayAlert("Error", "Not supported on this device.", "Ok");
            }
            catch (Exception ex)
            {
                await NavigationService.DisplayAlert("Error", "Unable to open a photo.", "Ok");
            }
        }
    }
}
