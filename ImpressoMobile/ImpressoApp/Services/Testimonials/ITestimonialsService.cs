using System;
using System.Threading.Tasks;
using ImpressoApp.Models.Testimonial;
using ImpressoApp.Models;
namespace ImpressoApp.Services.Testimonials
{
    public interface ITestimonialsService
    {
        Task<CreateTestimonialResponseModel> CreateTestimonial(string content);

        Task<BaseResponseModel> VerifyTestimonial(int testimonialId);

        Task<BaseResponseModel> EditTestimonial(int testimonialId, string content);

        Task<GeneratedTestimonialInfo> GetInformationNeededForTestimonial();
    }
}
