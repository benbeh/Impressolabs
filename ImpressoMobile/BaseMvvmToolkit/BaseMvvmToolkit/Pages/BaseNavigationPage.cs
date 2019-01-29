using Xamarin.Forms;

namespace BaseMvvmToolkit.Pages
{
    public class BaseNavigationPage : NavigationPage
    {
        public BaseNavigationPage(Page root, bool presentToolbar = true) : base(root)
        {
            BarTextColor = Color.White;

            if (!presentToolbar)
            {
                SetHasNavigationBar(root, false);
            }
        }

        protected override void OnAppearing()
        {
            Popped += Page_Popped;
            base.OnAppearing();
        }

        public void Page_Popped(object sender, NavigationEventArgs e)
        {
            var page = e.Page;

            if (page is IBasePage)
            {
                ((IBasePage)page).OnPageClosing();
            }
        }

        protected override void OnDisappearing()
        {
            Popped -= Page_Popped;
            base.OnDisappearing();
        }
    }
}
