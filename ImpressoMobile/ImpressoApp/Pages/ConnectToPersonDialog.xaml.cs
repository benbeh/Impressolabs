using System.Threading.Tasks;
using BaseMvvmToolkit;
using Xamarin.Forms;

namespace ImpressoApp.Pages
{
    public partial class ConnectToPersonDialog : SlideCustomDialog
    {
        public ConnectToPersonDialog()
        {
            InitializeComponent();

            customEntry.Focused += async (object sender, Xamarin.Forms.FocusEventArgs e) =>
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    await Task.Delay(1);
                    await scrollView.ScrollToAsync(0, (customEntry.Parent as VisualElement).Bounds.Y, true);
                }
            };
        }
    }
}
