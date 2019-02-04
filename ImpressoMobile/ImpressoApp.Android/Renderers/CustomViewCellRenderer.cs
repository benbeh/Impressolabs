using System;
using System.ComponentModel;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Views;
using ImpressoApp.Controls;
using ImpressoApp.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using View = Android.Views.View;

[assembly: ExportRenderer(typeof(CustomViewCell), typeof(CustomViewCellRenderer))]
namespace ImpressoApp.Droid.Renderers
{
    class CustomViewCellRenderer : ViewCellRenderer
    {
        private View cellCore;
        private Drawable unselectedBackground;
        private bool selected;

        protected override View GetCellCore(Cell item, View convertView, ViewGroup parent, Context context)
        {
            cellCore = base.GetCellCore(item, convertView, parent, context);

            selected = false;
            unselectedBackground = cellCore.Background;

            return cellCore;
        }

        protected override void OnCellPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            base.OnCellPropertyChanged(sender, args);

            if (args.PropertyName == "IsSelected")
            {
                selected = !selected;

                if (selected)
                {
                    var customViewCell = sender as CustomViewCell;
                    cellCore.SetBackgroundColor(customViewCell.SelectedBackgroundColor.ToAndroid());
                }
                else
                {
                    cellCore.SetBackground(unselectedBackground);
                }
            }
        }
    }
}
