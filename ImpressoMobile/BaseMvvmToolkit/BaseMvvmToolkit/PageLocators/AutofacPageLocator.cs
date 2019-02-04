using System;
using Autofac;
using BaseMvvmToolkit.Pages;
using BaseMvvmToolkit.ViewModels;

namespace BaseMvvmToolkit.PageLocators
{
    public class AutofacPageLocator : PageLocator
    {
        private readonly ILifetimeScope _container;

        public AutofacPageLocator(ILifetimeScope container)
        {
            _container = container;
        }

        protected override IBasePage CreatePage(Type pageType)
        {
            return _container.Resolve(pageType) as IBasePage;
        }

        protected override IBaseViewModel CreateViewModel(Type viewModelType)
        {
            try
            {
                return _container.Resolve(viewModelType) as IBaseViewModel;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
