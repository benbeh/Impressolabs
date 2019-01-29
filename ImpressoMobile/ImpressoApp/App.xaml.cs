using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Autofac;
using BaseMvvmToolkit.Extensions;
using System.Reflection;
using BaseMvvmToolkit.Services;
using ImpressoApp.ViewModels;
using ImpressoApp.Services.RequestProvider;
using ImpressoApp.Services.AuthenticationService;
using ImpressoApp.Services.Media;
using ImpressoApp.Services.Feeds;
using ImpressoApp.Services.User;
using ImpressoApp.Services.Profile;
using ImpressoApp.Services.Job;
using ImpressoApp.Services.Company;
using ImpressoApp.Services.Search;
using ImpressoApp.Services.Event;
using ImpressoApp.Services.Token;
using ImpressoApp.Services.People;
using ImpressoApp.Services.Testimonials;
using ImpressoApp.Services.Facebook;
using ImpressoApp.Services.Project;
using ImpressoApp.Services.Connect;
using FFImageLoading;
using ImpressoApp.ViewModels.Authentication;
using ImpressoApp.Services;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace ImpressoApp
{
    public partial class App : Application
    {
        public IContainer Container { get; private set; }

        public App()
        {
            InitializeComponent();
            SetupDependencies();
            SetupStartPage();
        }

        private void SetupDependencies()
        {
            if (Container != null)
            {
                return;
            }

            var builder = new ContainerBuilder();
            builder.RegisterMvvmComponents(typeof(App).GetTypeInfo().Assembly);
            builder.RegisterType<NavigationService>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterXamDependency<IFacebookService>();
            builder.RegisterXamDependency<IPlatformService>();
            builder.RegisterXamDependency<IImageProcessService>();
            builder.RegisterType<RequestProvider>().As<IRequestProvider>().SingleInstance();
            builder.RegisterType<AuthenticationService>().As<IAuthenticationService>();
            builder.RegisterType<MediaPickerService>().As<IMediaPickerService>();
            builder.RegisterType<FeedsService>().As<IFeedsService>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<ProfileService>().As<IProfileService>();
            builder.RegisterType<ProjectService>().As<IProjectService>();
            builder.RegisterType<JobService>().As<IJobService>();
            builder.RegisterType<PeopleService>().As<IPeopleService>();
            builder.RegisterType<CompanyService>().As<ICompanyService>();
            builder.RegisterType<SearchService>().As<ISearchService>().SingleInstance();
            builder.RegisterType<EventService>().As<IEventService>();
            builder.RegisterType<TokenService>().As<ITokenService>();
            builder.RegisterType<TestimonialsService>().As<ITestimonialsService>();
            builder.RegisterType<ConnectService>().As<IConnectService>();
            Container = builder.Build();
        }

        private async void SetupStartPage()
        {
            var navigationService = Container.Resolve<INavigationService>();

            if (navigationService != null)
            {
                navigationService.SetMainViewModel<WelcomeViewModel>();
            }
        }
    }
}
