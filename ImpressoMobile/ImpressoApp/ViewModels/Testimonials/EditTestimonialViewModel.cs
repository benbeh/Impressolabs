using System;
using BaseMvvmToolkit.ViewModels;
using BaseMvvmToolkit.Services;
using ImpressoApp.Services.Testimonials;
using System.Threading.Tasks;
using ImpressoApp.Models.Testimonial;
using System.Windows.Input;
using Xamarin.Forms;
using ImpressoApp.Exceptions;

namespace ImpressoApp.ViewModels.Testimonials
{
    public class EditTestimonialViewModel : BaseViewModel
    {
        private readonly ITestimonialsService testimonialsService;

        public ICommand UpdateTestimonialCommand => new Command(UpdateTestimonialCommandExecute);

        public TestimonialServerModel Testimonial { get; set; }

        public EditTestimonialViewModel(INavigationService navigationService, 
                                        ITestimonialsService testimonialsService) : base(navigationService)
        {
            this.testimonialsService = testimonialsService;

            Title = "Edit Testimonial";
        }

        public override Task Init(object args)
        {
            if(args is TestimonialServerModel testimonial)
            {
                Testimonial = testimonial;
            }

            return base.Init(args);
        }

        private async void UpdateTestimonialCommandExecute(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(Testimonial.Content))
            {
                return;
            }

            try
            {
                IsBusy = true;

                var result = await testimonialsService.EditTestimonial(Testimonial.Id, Testimonial.Content);

                if(result.IsSuccess)
                {
                    await NavigationService.DisplayAlert("", "Testimonial has been updated sucessfully.", "Close");
                    await NavigationService.PopAsync();
                }
            }
            catch (ServiceAuthenticationException e)
            {
                await NavigationService.DisplayAlert("Error", "Error authorization. Please verify your credentials and try again.", "Close");
            }
            catch (Exception e)
            {
                await NavigationService.DisplayAlert("Error", "Error updating testimonial. Check internet connection and try again.", "Close");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
