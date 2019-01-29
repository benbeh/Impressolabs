using System;
using BaseMvvmToolkit.ViewModels;
using BaseMvvmToolkit.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using ImpressoApp.Models.Testimonial;
using PropertyChanged;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace ImpressoApp.ViewModels.Testimonials
{
    public class AllTestimonialsViewModel : BaseViewModel
    {
        public ObservableCollection<TestimonialServerModel> Testimonials { get; set; }

        public bool CanVerify { get; set; }

        public ICommand VerifyTestimonialCommand => new Command(VerifyTestimonialCommandExecute);
        public ICommand SelectTestimonialCommand => new Command(SelectTestimonialCommandExecute);

        public AllTestimonialsViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "All Testimonials";
        }

        public override Task Init(object args)
        {
            if(args is IList<TestimonialServerModel> testimonials)
            {
                Testimonials = new ObservableCollection<TestimonialServerModel>(testimonials);

                if(Testimonials != null && Testimonials.Any())
                {
                    var testimonial = Testimonials.FirstOrDefault();
                    CanVerify = testimonial.RecommenderId != Settings.UserID 
                                           && !testimonial.IsVerified
                                           && testimonial.VerifiersCount < 4;
                }
            }

            return base.Init(args);
        }

        private void VerifyTestimonialCommandExecute(object obj)
        {
            
        }

        private async void SelectTestimonialCommandExecute(object obj)
        {
            if (obj is TestimonialServerModel testimonialServerModel)
            {
                await NavigationService.NavigateToAsync<ViewTestimonialViewModel>(testimonialServerModel);
            }
        }
    }
}
