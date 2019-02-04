using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PropertyChanged;

namespace ImpressoApp.Models.Testimonial
{
    [AddINotifyPropertyChangedInterface]
    public class TestimonialServerModel
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

        [AlsoNotifyFor("VerifyText")]
        public bool IsVerified { get; set; }

        public DateTime DateOfPost { get; set; }

        public ICollection<VerificatorModel> Verificators { get; set; }


        [JsonIgnore]
        public string Creator
        {
            get
            {
                return $"{RecommenderName}, {RecommenderCompanyPosition}";
            }
        }

        [JsonIgnore]
        public int VerifiersCount
        {
            get
            {
                return Verificators != null ? Verificators.Count : 0;
            }
        }

        [JsonIgnore]
        public string VerifyText
        {
            get
            {
                return IsVerified ? "VERIFIED" : "VERIFY";
            }
        }

        [JsonIgnore]
        public bool HasAnyVerifies
        {
            get
            {
                return Verificators != null ? Verificators.Any() : false;
            }
        }

    }
}
