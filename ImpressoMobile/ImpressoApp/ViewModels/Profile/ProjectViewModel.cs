using System;
using System.Threading.Tasks;
using BaseMvvmToolkit.Services;
using BaseMvvmToolkit.ViewModels;

namespace ImpressoApp.ViewModels.Profile
{
    public class ProjectViewModel : BaseViewModel
    {
        public string Name { get; set; }

        public ProjectViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        public override Task Init(object args)
        {
            Name = args as string;

            return base.Init(args);
        }
    }
}
