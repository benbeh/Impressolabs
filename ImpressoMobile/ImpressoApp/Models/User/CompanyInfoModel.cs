using System;
using System.Collections.Generic;
using ImpressoApp.Enums;
using ImpressoApp.Models.Job;
using ImpressoApp.Models.Project;
using ImpressoApp.Models.Token;

namespace ImpressoApp.Models.User
{
    public class CompanyInfoModel
    {
        public string Photo { get; set; }
        public string Name { get; set; }
        public List<JobModel> Jobs { get; set; }
        public List<ProjectModel> Projects { get; set; }
        public Industry Industry { get; set; }
        public List<TokenModel> Tokens { get; set; }
        public int TotalTokens { get; set; }
    }
}
