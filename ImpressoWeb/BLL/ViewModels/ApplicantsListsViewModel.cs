using System;
using System.Collections.Generic;

namespace BLL.ViewModels
{
    public class ApplicantsListsViewModel
    {
        public ApplicantsListsViewModel(IEnumerable<ApplicantViewModel> applicants)
        {
            var now = DateTime.Now;
            var firstDayOfCurrentWeek = now.Date.AddDays(-GetDayOfWeek(now));

            TodayApplicants = new List<ApplicantViewModel>();
            ThisWeekApplicants = new List<ApplicantViewModel>();
            EarlierApplicants = new List<ApplicantViewModel>();

            foreach (var applicant in applicants)
            {
                var firstDayOfDateOfPostWeek = applicant.DateOfPost.Date.AddDays(-GetDayOfWeek(applicant.DateOfPost));
                if (applicant.DateOfPost.ToString("dd/MM/yyyy") == now.ToString("dd/MM/yyyy"))
                {
                    TodayApplicants.Add(applicant);
                }
                else if (firstDayOfDateOfPostWeek == firstDayOfCurrentWeek)
                {
                    ThisWeekApplicants.Add(applicant);
                }
                else
                {
                    EarlierApplicants.Add(applicant);
                }
            }
        }

        private int GetDayOfWeek(DateTime date)
        {
            var calendar = System.Globalization.DateTimeFormatInfo.CurrentInfo.Calendar;
            int dayOfWeek = (int) calendar.GetDayOfWeek(date) - 1;
            // monday is first day of week, but no sunday
            return dayOfWeek == -1 ? 6 : dayOfWeek;
        }

        public List<ApplicantViewModel> TodayApplicants { get; set; }
        public List<ApplicantViewModel> ThisWeekApplicants { get; set; }
        public List<ApplicantViewModel> EarlierApplicants { get; set; }
    }
}