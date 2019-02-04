using BaseMvvmToolkit.ViewModels;
using System.Windows.Input;
using ImpressoApp.Models.Job;

namespace ImpressoApp.ViewModels.Feeds
{
    public class ApplyJobDialogViewModel : DialogViewModel
    {
        private JobModel jobModel;
        public JobModel JobModel
        {
            get => jobModel;
            set
            {
                jobModel = value;
                ApplyFor = string.Format("as {0} in {1} in {2}", jobModel.Title, jobModel.CompanyName, jobModel.Location);
            }
        }
        public string ApplyFor { get; private set; }

        public ICommand CancelCommand { get; set; }
        public ICommand ApplyWithYourProfileCommand { get; set; }
    }
}
