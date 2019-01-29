using System;
using PropertyChanged;
using ImpressoApp.Enums;
using System.Collections.ObjectModel;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using ImpressoApp.Models.User;

namespace ImpressoApp.Models.Job
{
    [AddINotifyPropertyChangedInterfaceAttribute]
    public class JobModel
    {
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime PostedTime { get; set; }
        public bool TopSkilsMatch { get; set; }
        public bool IsBookmarked { get; set; }
        public string HirePeriodTime { get; set; }
        public ExperienceType Level { get; set; }
        public int? CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyLogoSource { get; set; }
        public int ApplicantsCount { get; set; }
        public string JobText { get; set; }
        public TypeOfWork TypeOfWork { get; set; }
        public List<string> Skills { get; set; }
        public bool IsApplied { get; set; }
    }
}
