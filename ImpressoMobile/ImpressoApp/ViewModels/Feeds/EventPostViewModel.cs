using System;
using BaseMvvmToolkit.ViewModels;
using BaseMvvmToolkit.Services;
using System.Windows.Input;
using ImpressoApp.Models.Feeds;
using System.Threading.Tasks;
using Xamarin.Forms;
using ImpressoApp.Services.Event;
using ImpressoApp.Exceptions;

namespace ImpressoApp.ViewModels.Feeds
{
    public class EventPostViewModel : BaseViewModel
    {
        private IEventService eventService;

        public ICommand InterestedCommand { get; private set; }
        public ICommand ShareCommand { get; set; }

        public ConnectEventModel EventModel { get; set; }

        public EventPostViewModel(INavigationService navigationService, 
                                  IEventService eventService) : base(navigationService)
        {
            this.eventService = eventService;

            Title = "Event Post";

            InterestedCommand = new Command(InterestedEventCommandExecute);
            ShareCommand = new Command(ShareCommandExecute);
        }

        private async void ShareCommandExecute(object obj)
        {
            await NavigationService.DisplayDialog(new NotImplementedDialogViewModel());
        }

        public override Task Init(object args)
        {
            EventModel = args as ConnectEventModel;

            return base.Init(args);
        }

        private async void InterestedEventCommandExecute(object obj)
        {
            try
            {
                var result = await eventService.SetAsInterestedAsync(EventModel.Id.ToString());

                if (result.IsSuccess)
                {
                    EventModel.IsInterested = true;
                }
            }
            catch (ServiceAuthenticationException)
            {
                await NavigationService.DisplayAlert("Error", "Error authorization. Please verify your credentials and try again.", "Close");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await NavigationService.DisplayAlert("Error", "Check internet connection and try again.", "Ok");
            }
        }
    }
}
