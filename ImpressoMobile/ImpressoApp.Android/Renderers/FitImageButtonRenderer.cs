using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Widget;
using ImpressoApp.Droid.Renderers;
using ImpressoApp.Controls;
using Android.Content;

[assembly: ExportRenderer(typeof(FitImageButton), typeof(FitImageButtonRenderer))]
namespace ImpressoApp.Droid.Renderers
{
    public class FitImageButtonRenderer : ViewRenderer<FitImageButton, ImageButton>
    {
        private FitImageButton FitImageButton => Element as FitImageButton;

        public FitImageButtonRenderer(Context context) : base(context)
        {

        }

        private ImageButton ImageButton { get; set; }

        protected override void OnElementChanged(ElementChangedEventArgs<FitImageButton> e)
        {
            base.OnElementChanged(e);

            ImageButton = new ImageButton(Context);

            ImageButton.Click += (o, e1) =>             {                 Element.Command?.Execute(this);             }; 
            var fileName = FitImageButton.Image.File;
            var id = Resources.GetIdentifier(fileName.ToLower().Replace(".png", string.Empty), "drawable", Context.PackageName);
            ImageButton.SetImageResource(id);
            ImageButton.SetScaleType(ImageView.ScaleType.FitCenter);
            ImageButton.SetBackgroundColor(Android.Graphics.Color.Transparent);
            ImageButton.SetPadding(2.ToPx(), 2.ToPx(), 2.ToPx(), 2.ToPx());

            SetNativeControl(ImageButton);
        }
    }
}
