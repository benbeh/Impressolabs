using System.Collections.Generic;
using BLL.ViewModels;
using BLL.ViewModels.API;
using Core.Entities;

namespace BLL.Interfaces
{
    public interface ITestimonialService : IService<Testimonial, TestimonialViewModel>
    {
        IEnumerable<TestimonialViewModel> GetTestimonialsLeftByUser(string userId);
        Testimonial CreateTestimonial(string userId, string Content);
        void EditTestimonial(int testimonialId, string Content);
    }
}