using System;
using BaseMvvmToolkit.ViewModels;
using BaseMvvmToolkit.Services;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using ImpressoApp.Enums;
using ImpressoApp.ViewModels.Authentication;
namespace ImpressoApp.ViewModels
{
    public class WalkthroughtViewModel : BaseViewModel
    {
        private const string DefaultContinueButtonText = "Continue";
        
        private int currentPosition;

        public ObservableCollection<WalkthroughCarouselViewType> ViewList { get; set; } 

        public int CurrentPosition { 
            get { return currentPosition; }
            set 
            { 
                currentPosition = value;
                UpdateUiState();
                OnPropertyChanged();
            } 
        }
        public string ContinueButtonText { get; set; } = DefaultContinueButtonText;

        public ICommand ContinueCommand { get; private set; }
        public ICommand SkipCommand { get; private set; }

        public WalkthroughtViewModel(INavigationService navigationService) : base(navigationService)
        {
            ViewList = new ObservableCollection<WalkthroughCarouselViewType>
            {
                WalkthroughCarouselViewType.Blockchain,
                WalkthroughCarouselViewType.SmartProfile,
                WalkthroughCarouselViewType.QualityNetworking,
                WalkthroughCarouselViewType.Recruitment
            };

            ContinueCommand = new Command(ContinueCommandExecute);
            SkipCommand = new Command(SkipCommandExecute);
        }

        private void SkipCommandExecute(object obj)
        {
            FinishWalktrought();
        }

        private void ContinueCommandExecute(object obj)
        {
            if(CurrentPosition < ViewList.Count - 1)
            {
                CurrentPosition++;
            }
            else
            {
                FinishWalktrought();
            }
        }

        private void UpdateUiState()
        {
            ContinueButtonText = CurrentPosition == ViewList.Count - 1 ? "Sign Up" : DefaultContinueButtonText;
        }

        public override void OnAppearing()
        {
            CurrentPosition = 0;

            base.OnAppearing();
        }

        private async void FinishWalktrought()
        {
            await NavigationService.NavigateToAsync<SignUpViewModel>();
        }
    }
}
