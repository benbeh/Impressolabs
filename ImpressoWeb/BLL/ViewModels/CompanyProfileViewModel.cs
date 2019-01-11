using System;
using System.Collections.Generic;
using System.Linq;
using BLL.ViewModels.API;
using Core.Enum;

namespace BLL.ViewModels
{
    public class CompanyProfileViewModel
    {
        public string Photo { get; set; }
        public string Name { get; set; }
        public IndustryEnum Industry { get; set; }

        public IEnumerable<ProjectViewModel> Projects { get; set; }
        public IEnumerable<JobViewModel> Jobs { get; set; }
        public IEnumerable<TokenLogViewModel> Tokens { get; set; }

        public int TotalTokens
        {
            get
            {
                if (Tokens == null || !Tokens.Any())
                    return 0;
                return Tokens.ToList().Sum(token => token.Count);
            }
        }
    }
}