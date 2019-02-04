using System;
using System.Collections;
using Xamarin.Forms;
using System.Collections.ObjectModel;
namespace ImpressoApp.Controls
{
    public class SpecialGridView : Grid
    {
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(
                nameof(ItemsSource),
                typeof(ICollection),
                typeof(SpecialGridView),
                null,
                propertyChanged: ItemsChanged);

        public static readonly BindableProperty ItemTemplateProperty =
            BindableProperty.Create(
                nameof(ItemTemplate),
                typeof(DataTemplate),
                typeof(SpecialGridView),
                null);

        public ICollection ItemsSource
        {
            get => (ICollection)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        public void BuildUI()
        {
            int index = 0;

            foreach (var item in ItemsSource)
            {
                RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                var itemView = GetItemView(item);
                Children.Add(itemView);
                SetRow(itemView, index);

                index++;
            }

            InvalidateMeasure();
        }

        private View GetItemView(object model)
        {
            var view = (View)ItemTemplate?.CreateContent();
            view.BindingContext = model;

            return view;
        }

        private static void ItemsChanged(object bindable, object oldValue, object newValue)
        {
            if (bindable is SpecialGridView gridView && gridView.ItemsSource != null)
            {
                gridView?.BuildUI();
            }
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            var res = base.OnMeasure(widthConstraint, heightConstraint);

            var h = Children.Count > 0 ? this.Children[0].HeightRequest : 30;

            return new SizeRequest(new Size(res.Request.Width, (ItemsSource?.Count ?? 0) * h));
        }
    }
}
