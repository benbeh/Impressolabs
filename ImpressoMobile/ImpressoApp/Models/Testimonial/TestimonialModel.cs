using System;
using ImpressoApp.Models.People;
using System.Collections.Generic;
using Newtonsoft.Json;
using PropertyChanged;
using System.Linq;
using Android.Text;
namespace ImpressoApp.Models.Testimonial
{
    [AddINotifyPropertyChangedInterface]
    public class TestimonialModel
    {
        public PeopleModel Owner { get; set; }
        public List<PeopleModel> Verifiers { get; set; }
        public bool Verified { get; set; }
        public string Content { get; set; }
        public DateTime TimeStamp { get; set; }

        [JsonIgnore]
        public string Creator {
            get 
            {
                return $"{Owner.Name}, {Owner.Job}";
            }
        }

        [JsonIgnore]
        public int VerifiersCount 
        {
            get 
            {
                return Verifiers != null ? Verifiers.Count : 0;
            }
        }

        [JsonIgnore]
        public string VerifyText
        {
            get
            {
                return Verified ? "VERIFIED" : "VERIFY";
            }
        }

        [JsonIgnore]
        public bool HasAnyVerifies
        {
            get 
            {
                return Verifiers != null ? Verifiers.Any() : false;
            }
        }
    }
}
