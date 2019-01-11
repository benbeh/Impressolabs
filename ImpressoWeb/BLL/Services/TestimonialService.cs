using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.AutoMapper;
using BLL.Interfaces;
using BLL.ViewModels;
using BLL.ViewModels.API;
using Core.Entities;
using DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class TestimonialService : Service<Testimonial, TestimonialViewModel>, ITestimonialService
    {
        private readonly UserManager<AppUser> _userManager;

        public TestimonialService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager) : base(unitOfWork, unitOfWork.Testimonials)
        {
            _userManager = userManager;
        }

        public IEnumerable<TestimonialViewModel> GetTestimonialsLeftByUser(string userId)
        {
            var currentUser = _userManager.Users.Include(user => user.SentTestimonials).ThenInclude(testimonial => testimonial.RecommendedUser)
                .Include(user => user.SentTestimonials).ThenInclude(testimonial => testimonial.Verificators)
                .First(user => user.Id == userId);
            return Mapping.Map<IEnumerable<Testimonial>, IEnumerable<TestimonialViewModel>>(currentUser.SentTestimonials, opt => opt.Items["CurrentUserId"] = userId);
        }

        public Testimonial CreateTestimonial(string userId, string Content)
        {
            Testimonial createdTestimonial = new Testimonial()
            {
                RecommendedUserId = userId,
                RecommenderId = userId,
                Content = Content
            };

            Database.Testimonials.Add(createdTestimonial);
            Database.Save();
            return createdTestimonial;
        }
        public void EditTestimonial(int testimonialId, string Content)
        {
            var testimonial = Database.Testimonials.Get(testimonialId);
            testimonial.Content = Content;
            Database.Testimonials.Update(testimonial);
            Database.Save();
        }
    }
}
