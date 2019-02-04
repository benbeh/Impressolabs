using System;
using System.Threading.Tasks;
using System.Windows.Input;
using BaseMvvmToolkit.Services;
using BaseMvvmToolkit.ViewModels;
using ImpressoApp.Models.Job;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Collections.Generic;
using ImpressoApp.Enums;
using ImpressoApp.Models.User;
using ImpressoApp.Services.Job;

namespace ImpressoApp.ViewModels.Profile
{
    public class CreateJobViewModel : BaseViewModel
    {
        private IJobService jobService;

        public CreateJobViewModel(INavigationService navigationService, IJobService jobService) : base(navigationService)
        {
            Title = "Create Job";
            AddSkillCommand = new Command(AddSkillCommandExecute);
            SaveCommand = new Command(SaveExecute);
            this.jobService = jobService;
        }

        public ObservableCollection<TypeOfWork> JobTypes => new ObservableCollection<TypeOfWork> { TypeOfWork.None, TypeOfWork.PartTime, TypeOfWork.FullTime };
        public ObservableCollection<ExperienceType> JobLevels => new ObservableCollection<ExperienceType> { ExperienceType.None, ExperienceType.Beginner, ExperienceType.Intermediate, ExperienceType.Edvanced };

        public string NewSkillText { get; set; }

        public JobModel JobModel { get; private set; }

        public ICommand AddSkillCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }

        public override Task Init(object args)
        {
            JobModel = (JobModel)args;
            return base.Init(args);
        }

        private async void SaveExecute()
        {
            IsBusy = true;
            try
            {
                JobModel.PostedTime = DateTime.Now;
                await jobService.CreateJob(JobModel);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                IsBusy = false;
            }
            await NavigationService.PopAsync();
        }

        private void AddSkillCommandExecute(object obj)
        {
            if (string.IsNullOrWhiteSpace(NewSkillText))
            {
                return;
            }

            if (JobModel.Skills == null)
            {
                JobModel.Skills = new List<string>();
            }

            JobModel.Skills.Add(NewSkillText);

            var newCollection = new List<string>(JobModel.Skills);

            JobModel.Skills = newCollection;

            NewSkillText = string.Empty;
        }
    }
}
