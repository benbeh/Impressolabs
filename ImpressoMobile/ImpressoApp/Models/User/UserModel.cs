using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PropertyChanged;
using ImpressoApp.Models.Testimonial;
using System.Linq;

namespace ImpressoApp.Models.User
{
    [AddINotifyPropertyChangedInterface]
    public class UserModel : BaseResponseModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("photo")]
        public string Photo { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("companyPosition")]
        public string CompanyPosition { get; set; }

        [JsonProperty("status")]
        [AlsoNotifyFor("StatusItem")]
        public StatusType Status { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("connections")]
        public int Connections { get; set; }

        [JsonProperty("intro")]
        public string Intro { get; set; }

        [JsonProperty("lastUpdate")]
        public DateTimeOffset LastUpdate { get; set; }

        [JsonProperty("educations")]
        public List<string> Educations { get; set; }

        public string EthereumAddress { get; set; }

        [JsonProperty("passion")]
        public string Passion { get; set; }

        [JsonProperty("salary")]
        public long Salary { get; set; }

        [JsonProperty("personalityMatch")]
        public string PersonalityMatch { get; set; }

        [JsonProperty("jobType")]
        public JobType JobType { get; set; }

        [JsonProperty("cv")]
        public string Cv { get; set; }

        [JsonProperty("experience")]
        public ExperienceType Experience { get; set; }

        [JsonProperty("industry")]
        [AlsoNotifyFor("IndustryItem")]
        public JobIndustryType Industry { get; set; }

        [JsonProperty("presentCompanies")]
        public List<string> PresentCompanies { get; set; }

        [JsonProperty("pastCompanies")]
        public List<string> PastCompanies { get; set; }

        [JsonProperty("testimonials")]
        [AlsoNotifyFor("LastTestimonials")]
        public List<TestimonialServerModel> Testimonials { get; set; }

        [JsonProperty("accomplishments")]
        public List<string> Accomplishments { get; set; }

        [JsonProperty("skills")]
        public List<SkillModel> Skills { get; set; }

        [JsonProperty("certificates")]
        public List<string> Certificates { get; set; }

        [JsonProperty("isConnected")]
        [JsonIgnore]
        public bool IsConnected { get; set; }

        public bool IsBookmarked { get; set; }

        [JsonIgnore]
        public string FullName { get => string.Format("{0} {1}", FirstName, LastName); }

        [JsonIgnore]
        public string StatusText { get => GetStatusText(); }

        [JsonIgnore]
        public List<TestimonialServerModel> LastTestimonials
        {
            get => GetLastTestmonials();
        }


        private List<TestimonialServerModel> GetLastTestmonials()
        {
            var result = new List<TestimonialServerModel>();
            if (Testimonials != null && Testimonials.Any())
            {
                for (int i = Testimonials.Count - 1; i >= 0 && result.Count < 2; i--)
                {
                    result.Insert(0, Testimonials[i]);
                }
            }

            return result;
        }

        private string GetStatusText()
        {
            return StatusDictionary.FirstOrDefault(d => d.Value == Status).Key;
        }

        [JsonIgnore]
        public Dictionary<string, StatusType> StatusDictionary => new Dictionary<string, StatusType>
        {
            { "Not looking for job opportunities", StatusType.None },
            { "Open for job offers", StatusType.Open },
            { "Closed for job offers", StatusType.Closed }
        };

        [JsonIgnore]
        public List<string> StatusCollection => new List<string>(StatusDictionary.Keys);

        private string statusItem;

        [JsonIgnore]
        public string StatusItem
        {
            get => statusItem = GetStatusText();
            set
            {
                statusItem = value;
                Status = GetStatus(statusItem);
            }
        }

        private StatusType GetStatus(string statusText)
        {
            if (StatusDictionary.ContainsKey(statusText))
            {
                return StatusDictionary[statusText];
            }

            return StatusType.None;
        }

        private int industryItem;

        [JsonIgnore]
        public int IndustryItem
        {
            get => industryItem = (int)Industry;
            set
            {
                if (value >= 0)
                {
                    industryItem = value;
                    Industry = (JobIndustryType)industryItem;
                }
            }
        }
    }
}
