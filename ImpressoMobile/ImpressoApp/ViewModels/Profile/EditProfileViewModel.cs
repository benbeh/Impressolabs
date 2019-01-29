using System;
using BaseMvvmToolkit.ViewModels;
using BaseMvvmToolkit.Services;
using ImpressoApp.Services.User;
using ImpressoApp.Models.User;
using ImpressoApp.Exceptions;
using System.Collections.ObjectModel;
using ImpressoApp.Controls;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using ImpressoApp.Services.Media;
using System.Collections.Generic;
using Java.Sql;
using ImpressoApp.Models.Company;
using ImpressoApp.Models.Testimonial;
using ImpressoApp.Models.People;
using ImpressoApp.Services.Testimonials;
using ImpressoApp.ViewModels.Testimonials;
using ImpressoApp.Enums;
using JobType = ImpressoApp.Models.User.JobType;
using ImpressoApp.Extensions;

namespace ImpressoApp.ViewModels.Profile
{
    public class EditProfileViewModel : BaseViewModel
    {
        private const string ProfilePhotoHeaderName = "Upload your photo";
        private const string TakePhotoButtonName = "Take from camera";
        private const string ChooseFromLibraryButtonName = "Choose from library";
        private const string DeletePhotoButtonName = "Delete a photo";
        private const string CancelButtonName = "Cancel";

        private readonly IUserService userService;
        private readonly IMediaPickerService mediaPickerService;
        private readonly ITestimonialsService testimonialsService;

        public bool IsLoaded { get; private set; }
        public bool IsEditMode { get; set; } = true;

        public UserModel UserModel { get; private set; }

        public string NewTestimonialText { get; set; }
        public string NewEducationText { get; set; }
        public string NewSkillText { get; set; }
        public string NewCertificateText { get; set; }
        public string NewPastCompanyItemText { get; set; }
        public string NewPresentCompanyItemText { get; set; }
        public string NewAccomplishmentItemText { get; set; }

        public ObservableCollection<JobType> JobTypes => new ObservableCollection<JobType> { JobType.None, JobType.Freelancer, JobType.Contractor };
        public ObservableCollection<ExperienceType> Experiences =>
        new ObservableCollection<ExperienceType>
        {
            ExperienceType.None,
            ExperienceType.Beginner,
            ExperienceType.Intermediate,
            ExperienceType.Edvanced
        };

        public ObservableCollection<string> Industries { get; private set; }


        public ObservableCollection<TabIndicatorModel> Tabs { get; set; }

        public ICommand TabChangedCommand { get; private set; }
        public ICommand SelectTopSkillCommand { get; private set; }
        public ICommand AddNewAccomplishmentCommand { get; private set; }
        public ICommand AddEducationCommand { get; private set; }
        public ICommand AddSkillCommand { get; private set; }
        public ICommand AddCertificateCommand { get; private set; }
        public ICommand AddConnectedWithCommand { get; private set; }
        public ICommand AddPresentCompanyCommand { get; private set; }
        public ICommand AddPastCompanyCommand { get; private set; }
        public ICommand AddExperienceCommand { get; private set; }
        public ICommand ViewProfileCommand { get; private set; }
        public ICommand ChangePhotoCommand { get; private set; }
        public ICommand AddNewTestimonialCommand { get; private set; }
        public ICommand VerifyTestimonialCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand ViewAllTestimonialsCommand { get; private set; }
        public ICommand SelectTestimonialCommand { get; private set; }

        public bool FirstTabActive { get; set; } = true;
        public bool SecondTabActive { get; set; }
        public bool ThirdTabActive { get; set; }

        public EditProfileViewModel(INavigationService navigationService,
                                    IUserService userService,
                                    IMediaPickerService mediaPickerService,
                                    ITestimonialsService testimonialsService) : base(navigationService)
        {
            this.userService = userService;
            this.mediaPickerService = mediaPickerService;
            this.testimonialsService = testimonialsService;

            SetTabs();

            Title = "Profile Edit";

            TabChangedCommand = new Command(TabSelectedCommandExecute);
            SelectTopSkillCommand = new Command(SelectTopSkillCommandExecute);

            AddEducationCommand = new Command(AddEducationCommandExecute);
            AddSkillCommand = new Command(AddSkillCommandExecute);
            AddNewAccomplishmentCommand = new Command(AddNewAccomplishmentCommandExecute);
            AddConnectedWithCommand = new Command(AddConnectedWithCommandExecute);
            AddPresentCompanyCommand = new Command(AddPresentCompanyCommandExecute);
            AddPastCompanyCommand = new Command(AddPastCompanyCommandExecute);
            AddExperienceCommand = new Command(AddExperienceCommandExecute);
            AddCertificateCommand = new Command(AddCertificateCommandExecute);
            ViewProfileCommand = new Command(ViewProfileCommandExecute);
            ChangePhotoCommand = new Command(ChangePhotoCommandExecute);
            AddNewTestimonialCommand = new Command(AddNewTestimonialCommandExecute);
            SaveCommand = new Command(SaveCommandExecute);
            ViewAllTestimonialsCommand = new Command(ViewAllTestimonialsCommandExecute);
            SelectTestimonialCommand = new Command(SelectTestimonialCommandExecute);

            InitUI();
        }

        private void InitUI()
        {
            Industries = new ObservableCollection<string>();

            var industries = Enum.GetValues(typeof(JobIndustryType)).Cast<JobIndustryType>();
            foreach (var industry in industries)
            {
                Industries.Add(industry.DescriptionAttr());
            }
        }

        private async void SelectTestimonialCommandExecute(object obj)
        {
            if (obj is TestimonialServerModel model)
            {
                await NavigationService.NavigateToAsync<EditTestimonialViewModel>(model);
            }
        }

        private async void ViewAllTestimonialsCommandExecute(object obj)
        {
            await NavigationService.NavigateToAsync<AllTestimonialsViewModel>(UserModel.Testimonials);
        }

        private async void AddNewTestimonialCommandExecute(object obj)
        {
            if (string.IsNullOrWhiteSpace(NewTestimonialText))
            {
                return;
            }

            try
            {
                IsBusy = true;

                var result = await testimonialsService.CreateTestimonial(NewTestimonialText);
                if (!result.IsSuccess)
                {
                    await NavigationService.DisplayAlert("Error", result.Message, "Close");
                    return;
                }

                if (UserModel.Testimonials == null)
                {
                    UserModel.Testimonials = new List<TestimonialServerModel>();
                }

                UserModel.Testimonials.Add(new TestimonialServerModel
                {
                    Content = NewTestimonialText,
                    DateOfPost = DateTime.Now,
                    RecommenderId = UserModel.Id.ToString(),
                    RecommenderName = UserModel.FullName,
                    RecommenderCompanyPosition = UserModel.CompanyPosition,
                    RecommenderCompanyName = UserModel.PastCompanies != null ? UserModel.PresentCompanies.LastOrDefault() : ""
                });

                var newCollection = new List<TestimonialServerModel>(UserModel.Testimonials);

                UserModel.Testimonials = newCollection;

                NewTestimonialText = string.Empty;

            }
            catch (ServiceAuthenticationException e)
            {
                await NavigationService.DisplayAlert("Error", "Error authorization. Please verify your credentials and try again.", "Close");
            }
            catch (Exception e)
            {
                await NavigationService.DisplayAlert("Error", "Error adding new testimonial. Check internet connection and try again.", "Close");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void AddCertificateCommandExecute(object obj)
        {
            if (string.IsNullOrWhiteSpace(NewCertificateText))
            {
                return;
            }

            if (UserModel.Certificates == null)
            {
                UserModel.Certificates = new List<string>();
            }

            UserModel.Certificates.Add(NewCertificateText);

            var newCollection = new List<string>(UserModel.Certificates);

            UserModel.Certificates = newCollection;

            NewCertificateText = string.Empty;
        }

        private void AddSkillCommandExecute(object obj)
        {
            if (string.IsNullOrWhiteSpace(NewSkillText))
            {
                return;
            }

            if (UserModel.Skills == null)
            {
                UserModel.Skills = new List<SkillModel>();
            }

            UserModel.Skills.Add(new SkillModel { Name = NewSkillText });

            var newCollection = new List<SkillModel>(UserModel.Skills);

            UserModel.Skills = newCollection;

            NewSkillText = string.Empty;
        }

        private void AddEducationCommandExecute(object obj)
        {
            if (string.IsNullOrWhiteSpace(NewEducationText))
            {
                return;
            }

            if (UserModel.Educations == null)
            {
                UserModel.Educations = new List<string>();
            }

            UserModel.Educations.Add(NewEducationText);

            var newCollection = new List<string>(UserModel.Educations);

            UserModel.Educations = newCollection;

            NewEducationText = string.Empty;
        }

        private void UpdateColelction<T>(IList<T> source)
        {
            var newCollection = new List<T>(source);
            source = newCollection;
        }

        private async void SaveCommandExecute(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            try
            {
                IsBusy = true;

                UserModel.LastUpdate = DateTimeOffset.Now;
                var result = await userService.EditUserProfileAsync(UserModel);

                if (result.IsSuccess)
                {
                    if (!string.IsNullOrEmpty(UserModel.FirstName) && !string.IsNullOrEmpty(UserModel.LastName))
                    {
                        Settings.UserName = UserModel.FullName;
                    }
                    Settings.UserProfileImage = UserModel.Photo;
                    Settings.UserPosition = UserModel.CompanyPosition;

                    await NavigationService.DisplayAlert("Message", "Profile has been updated sucessfully.", "Ok");
                }
                else
                {
                    await NavigationService.DisplayAlert("Error", result.Message, "Ok");
                }
            }
            catch (ServiceAuthenticationException)
            {
                await NavigationService.DisplayAlert("Error", "Error authorization. Please verify your credentials and try again.", "Ok");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await NavigationService.DisplayAlert("Error", "Save profile info error. Check internet connection and try again.", "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void ViewProfileCommandExecute(object obj)
        {
            await NavigationService.NavigateToAsync<ViewProfileViewModel>(UserModel.Id);
        }

        private async void AddExperienceCommandExecute(object obj)
        {
            await NavigationService.DisplayAlert("Not Implemented", "This feature has not been implemented yet.", "Ok");
        }

        private void AddPastCompanyCommandExecute(object obj)
        {
            if (string.IsNullOrWhiteSpace(NewPastCompanyItemText))
            {
                return;
            }

            if (UserModel.PastCompanies == null)
            {
                UserModel.PastCompanies = new List<string>();
            }

            UserModel.PastCompanies.Add(NewPastCompanyItemText);

            var newCollection = new List<string>(UserModel.PastCompanies);

            UserModel.PastCompanies = newCollection;

            NewPastCompanyItemText = string.Empty;
        }

        private void AddPresentCompanyCommandExecute(object obj)
        {
            if (string.IsNullOrWhiteSpace(NewPresentCompanyItemText))
            {
                return;
            }

            if (UserModel.PresentCompanies == null)
            {
                UserModel.PresentCompanies = new List<string>();
            }

            UserModel.PresentCompanies.Add(NewPresentCompanyItemText);

            var newCollection = new List<string>(UserModel.PresentCompanies);

            UserModel.PresentCompanies = newCollection;

            NewPresentCompanyItemText = string.Empty;
        }

        private async void AddConnectedWithCommandExecute(object obj)
        {
            await NavigationService.DisplayAlert("Not Implemented", "This feature has not been implemented yet.", "Ok");
        }

        private void AddNewAccomplishmentCommandExecute(object obj)
        {
            if (string.IsNullOrWhiteSpace(NewAccomplishmentItemText))
            {
                return;
            }

            if (UserModel.Accomplishments == null)
            {
                UserModel.Accomplishments = new List<string>();
            }

            UserModel.Accomplishments.Add(NewAccomplishmentItemText);

            var newCollection = new List<string>(UserModel.Accomplishments);

            UserModel.Accomplishments = newCollection;

            NewAccomplishmentItemText = string.Empty;
        }

        private void SelectTopSkillCommandExecute(object obj)
        {
            if (obj is SkillModel skillModel)
            {
                if (skillModel.IsTop)
                {
                    skillModel.IsTop = !skillModel.IsTop;
                    return;
                }

                var topCount = UserModel.Skills.Where(s => s.IsTop).ToList().Count;

                if (topCount < 3)
                {
                    skillModel.IsTop = !skillModel.IsTop;
                }
            }
        }

        public override void OnAppearing()
        {
            base.OnAppearing();

            LoadUserInfo();
        }

        private void SetTabs()
        {
            Tabs = new ObservableCollection<TabIndicatorModel>();
            Tabs.Add(new TabIndicatorModel { Title = "Professional" });
            Tabs.Add(new TabIndicatorModel { Title = "Experience" });
            Tabs.Add(new TabIndicatorModel { Title = "Honors" });
        }

        private void TabSelectedCommandExecute(object obj)
        {
            var tabIndex = (int)obj;

            FirstTabActive = false;
            SecondTabActive = false;
            ThirdTabActive = false;

            switch (tabIndex)
            {
                case 0:
                    FirstTabActive = true;
                    break;
                case 1:
                    SecondTabActive = true;
                    break;
                case 2:
                    ThirdTabActive = true;
                    break;
                default:
                    FirstTabActive = true;
                    break;
            }
        }

        private async void LoadUserInfo()
        {
            if (IsLoaded)
            {
                return;
            }

            try
            {
                IsBusy = true;

                if (!string.IsNullOrEmpty(Settings.UserID))
                {
                    var result = await userService.GetUserById(Settings.UserID);

                    if (result.IsSuccess)
                    {
                        UserModel = result;
                    }

                    IsLoaded = true;
                }
            }
            catch (ServiceAuthenticationException e)
            {
                await NavigationService.DisplayAlert("Error", "Error authorization. Please verify your credentials and try again.", "Close");
            }
            catch (Exception ex)
            {
                await NavigationService.DisplayAlert("Error", "Error loading user data. Check internet connection and try again.", "Close");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void ChangePhotoCommandExecute(object obj)
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
                    UserModel.Photo = null;
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
                UserModel.Photo = await mediaPickerService.TakePictureFromLibrary();
            }
            catch (NotSupportedException e)
            {
                await NavigationService.DisplayAlert("Error", "Not supported on this device.", "Ok");
            }
            catch (ArgumentException ae)
            {
                //skip for this case
            }
            catch (Exception ex)
            {
                await NavigationService.DisplayAlert("Error", "Unable to take a photo.", "Ok");
            }
        }

        private async void TakePictureFromCamera()
        {
            try
            {
                UserModel.Photo = await mediaPickerService.TakePictureFromCamera();

            }
            catch (NotSupportedException e)
            {
                await NavigationService.DisplayAlert("Error", "Not supported on this device.", "Ok");
            }
            catch (ArgumentException ae)
            {
                //skip for this place
            }
            catch (Exception ex)
            {
                await NavigationService.DisplayAlert("Error", "Unable to open a photo.", "Ok");
            }
        }
    }
}
