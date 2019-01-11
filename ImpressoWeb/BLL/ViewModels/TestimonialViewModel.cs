using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.ViewModels
{
    public class TestimonialViewModel
    {
        public int Id { get; set; }

        public string RecommenderId { get; set; }
        public string RecommenderName { get; set; }
        public string RecommenderCompanyPosition { get; set; }
        public string RecommenderCompanyName { get; set; }

        public string RecommendedUserId { get; set; }
        public string RecommendedUserName { get; set; }
        public string RecommendedUserPhoto { get; set; }
        public string RecommendedUserCompanyPosition { get; set; }

        public string Content { get; set; }
        public bool IsVerified { get; set; }
        public DateTime DateOfPost { get; set; }

        public virtual ICollection<VerificatorViewModel> Verificators { get; set; }
    }
}
