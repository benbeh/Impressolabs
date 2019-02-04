using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImpressoApp.Constants;
using ImpressoApp.Models;
using ImpressoApp.Models.Testimonial;
using ImpressoApp.Services.RequestProvider;

namespace ImpressoApp.Services.Testimonials
{
    public class TestimonialsService : ITestimonialsService
    {
        private const string CreateTestimonialEndpoint = ApplicationConstants.LiveServerApi + "api/Testimonial/CreateTestimonial";
        private const string EditTestimonialEndpoint = ApplicationConstants.LiveServerApi + "api/Testimonial/EditTestimonial";
        private const string VerifyTestimonialEndpoint = ApplicationConstants.LiveServerApi + "api/Testimonial/VerifyTestimonial";
        private const string GetInformationNeededForTestimonialEndpoint = ApplicationConstants.LiveServerApi + "api/Testimonial/GetInformationNeededForTestimonial";

        private readonly IRequestProvider requestProvider;

        public TestimonialsService(IRequestProvider requestProvider)
        {
            this.requestProvider = requestProvider;
        }

        public async Task<CreateTestimonialResponseModel> CreateTestimonial(string content)
        {
            return await requestProvider.PostAsync<CreateTestimonialResponseModel>(CreateTestimonialEndpoint, content);
        }

        public async Task<BaseResponseModel> EditTestimonial(int testimonialId, string content)
        {
            var model = new EditTestimonialRequestModel { TestimonialId = testimonialId, Content = content };

            return await requestProvider.PostAsync<BaseResponseModel>(EditTestimonialEndpoint, model);
        }

        public async Task<GeneratedTestimonialInfo> GetInformationNeededForTestimonial()
        {
            return await requestProvider.GetAsync<GeneratedTestimonialInfo>(GetInformationNeededForTestimonialEndpoint);
        }

        public async Task<BaseResponseModel> VerifyTestimonial(int testimonialId)
        {
            return await requestProvider.PostAsync<BaseResponseModel>(VerifyTestimonialEndpoint, new Dictionary<string, string>() { ["testimonialId"] = testimonialId.ToString() });
        }
    }
}
