using System;
using BaseMvvmToolkit.ViewModels;
using BaseMvvmToolkit.Services;
using System.Threading.Tasks;
using ImpressoApp.Models.Testimonial;
using System.Windows.Input;
using Xamarin.Forms;
using ImpressoApp.Services.Testimonials;
using ImpressoApp.Exceptions;

namespace ImpressoApp.ViewModels.Testimonials
{
    public class ViewTestimonialViewModel : BaseViewModel
    {
        private readonly ITestimonialsService testimonialsService;

        public TestimonialServerModel Testimonial { get; set; }

        public bool CanVerify { get; set; }

        public ICommand VerifyTestimonialCommand => new Command(VerifyTestimonialCommandExecute);

        public ViewTestimonialViewModel(INavigationService navigationService, 
                                        ITestimonialsService testimonialsService) : base(navigationService)
        {
            this.testimonialsService = testimonialsService;

            Title = "Testimonial";
        }

        public override Task Init(object args)
        {
            if(args is TestimonialServerModel model)
            {
                Testimonial = model;
                CanVerify = Testimonial.RecommenderId != Settings.UserID
                                       && !Testimonial.IsVerified
                                       && Testimonial.VerifiersCount < 4;
            }

            return base.Init(args);
        }

        private async void VerifyTestimonialCommandExecute(object obj)
        {
            if(IsBusy || !CanVerify)
            {
                return;
            }
            try
            {
                IsBusy = true;

                Testimonial.IsVerified = true;

                var result = await testimonialsService.VerifyTestimonial(Testimonial.Id);

                if(!result.IsSuccess)
                {
                    Testimonial.IsVerified = false;
                }
            }
            catch (ServiceAuthenticationException)
            {
                await NavigationService.DisplayAlert("Error", "Error authorization. Please verify your credentials and try again.", "Ok");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await NavigationService.DisplayAlert("Error", "Verify testimonial error. Check internet connection and try again.", "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
