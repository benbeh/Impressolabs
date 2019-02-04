using System;
using System.Collections.ObjectModel;
using BaseMvvmToolkit.ViewModels;
using ImpressoApp.Services.Project;
using System.Linq;
using Xamarin.Forms;
using Autofac;
using System.Windows.Input;
using BaseMvvmToolkit.Services;
using ImpressoApp.Models.Job;
using ImpressoApp.Services.User;
using System.Collections.Generic;
using ImpressoApp.Models.Project;

namespace ImpressoApp.ViewModels.Profile
{
    public class CreateJobDialogViewModel : DialogViewModel
    {
        private INavigationService navigationService;

        public ICommand RowSelected { get; set; }
        public ICommand ContinueCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        private readonly IUserService userService;
        private List<ProjectModel> projects;

        public CreateJobDialogViewModel()
        {
            this.navigationService = (Application.Current as App)?.Container.Resolve<INavigationService>();
            this.userService = (Application.Current as App)?.Container.Resolve<IUserService>();
            RowSelected = new Command(RowSelectedExecute);
            ContinueCommand = new Command(ContinueExecute);
            CancelCommand = new Command(CancelExecute);
        }

        public async override void Init(object arg)
        {
            base.Init(arg);

            projects = (await userService.GetCurrentCompanyInfo()).Projects;
            Items = new ObservableCollection<string>(projects.Select(item => item.Name));
            CurrentItem = Items[0];
        }

        public string JobName { get; set; }

        public string CurrentItem { get; set; }

        public ObservableCollection<string> Items { get; private set; } = new ObservableCollection<string>();

        private async void RowSelectedExecute(object obj)
        {
            var value = (obj as SelectedItemChangedEventArgs).SelectedItem as string;
            var selectedItem = value;
            Items = new ObservableCollection<string>(Items.ToList());
        }

        private async void ContinueExecute(object obj)
        {
            navigationService.HideDialog();
            await navigationService.NavigateToAsync<CreateJobViewModel>(new JobModel { Title = JobName, ProjectId = projects.FirstOrDefault(pr => pr.Name == CurrentItem)?.Id });
        }

        private async void CancelExecute(object obj)
        {
            navigationService.HideDialog();
        }
    }
}
