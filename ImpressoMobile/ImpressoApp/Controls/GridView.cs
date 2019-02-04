using System;
using System.Collections;
using Xamarin.Forms;
using System.Collections.ObjectModel;
namespace ImpressoApp.Controls
{
    public class GridView : Grid
    {
        public static readonly BindableProperty ItemsSourceProperty = 
            BindableProperty.Create(
                nameof(ItemsSource), 
                typeof(ICollection), 
                typeof(GridView), 
                null,
                propertyChanged: ItemsChanged);

        public static readonly BindableProperty ItemTemplateProperty =
            BindableProperty.Create(
                nameof(ItemTemplate),
                typeof(DataTemplate),
                typeof(GridView),
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

            foreach(var item in ItemsSource)
            {
                RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                var itemView = GetItemView(item);
                Children.Add(itemView);
                SetRow(itemView, index);

                index++;
            }
        }

        private View GetItemView(object model)
        {
            var view = (View)ItemTemplate?.CreateContent();
            view.BindingContext = model;

            return view;
        }

        private static void ItemsChanged(object bindable, object oldValue, object newValue)
        {
            if (bindable is GridView gridView && gridView.ItemsSource != null)
            {
                gridView?.BuildUI();
            }
        }
    }
}
