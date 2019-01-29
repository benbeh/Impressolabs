//using System;
//using Newtonsoft.Json;
//using ImpressoApp.Models.User;

//namespace ImpressoApp.Models.Job
//{
//    public partial class JobServerModel
//    {
//        [JsonProperty("id")]
//        public int Id { get; set; }

//        [JsonProperty("title")]
//        public string Title { get; set; }

//        [JsonProperty("description")]
//        public object Description { get; set; }

//        [JsonProperty("location")]
//        public object Location { get; set; }

//        [JsonProperty("projectId")]
//        public long ProjectId { get; set; }

//        [JsonProperty("projectName")]
//        public string ProjectName { get; set; }

//        [JsonProperty("postedTime")]
//        public DateTimeOffset PostedTime { get; set; }

//        [JsonProperty("industry")]
//        public JobIndustryType Industry { get; set; }

//        [JsonProperty("jobType")]
//        public JobType JobType { get; set; }

//        [JsonProperty("level")]
//        public ExperienceType Level { get; set; }

//        [JsonProperty("typeOfWork")]
//        public long TypeOfWork { get; set; }

//        [JsonProperty("topSkillsMatch")]
//        public bool TopSkillsMatch { get; set; }

//        [JsonProperty("isBookmarked")]
//        public bool IsBookmarked { get; set; }

//        [JsonProperty("isApplied")]
//        public bool IsApplied { get; set; }

//        [JsonProperty("companyName")]
//        public string CompanyName { get; set; }

//        [JsonProperty("companyId")]
//        public int CompanyId { get; set; }

//        [JsonProperty("companyLogoSource")]
//        public string CompanyLogoSource { get; set; }

//        [JsonProperty("applicantsCount")]
//        public int ApplicantsCount { get; set; }

//        [JsonProperty("positionsCount")]
//        public int PositionsCount { get; set; }

//        [JsonProperty("skills")]
//        public object Skills { get; set; }

//        [JsonProperty("appUserJobs")]
//        public object AppUserJobs { get; set; }
//    }
//}
