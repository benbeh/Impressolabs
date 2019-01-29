using System;
using BottomBar.XamarinForms;
using BaseMvvmToolkit.Pages;
using ImpressoApp.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using BottomBar.Droid.Renderers;
using System.Reflection;

[assembly: ExportRenderer(typeof(BaseTabbedPage), typeof(BaseTabbedPageRenderer))]
namespace ImpressoApp.Droid.Renderers
{
    public class BaseTabbedPageRenderer : BottomBarPageRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<BottomBarPage> e)
        {
            if (e.NewElement == null)
            {
                base.OnElementChanged(e);
                return;
            }

            base.OnElementChanged(e);

            if (e.NewElement is BaseTabbedPage tabbedPage)
            {
                tabbedPage.BarTextColor = tabbedPage.TintColor;
            }

            var bottomBar = GetBottomBar();
            bottomBar.ItemContainer.SetBackgroundColor(e.NewElement.BarBackgroundColor.ToAndroid());
        }

        private BottomNavigationBar.BottomBar GetBottomBar()
        {
            const string bottomBarFieldName = "_bottomBar";

            Type baseRendererType = typeof(BottomBarPageRenderer);

            // Look-up the field the bottom bar should be stored in.
            FieldInfo bottomBarField = baseRendererType.GetField(bottomBarFieldName,
                BindingFlags.Instance | BindingFlags.NonPublic);

            if (bottomBarField == null)
            {
                throw new MissingFieldException(typeof(BottomBarPageRenderer).Name,
                    bottomBarFieldName);
            }

            if (bottomBarField.FieldType != typeof(BottomNavigationBar.BottomBar))
            {
                throw new FormatException(
                    $"The type of bottom bar is not that which was expected: {bottomBarField.FieldType.Name}");
            }

            return bottomBarField.GetValue(this) as BottomNavigationBar.BottomBar;
        }

    }
}
