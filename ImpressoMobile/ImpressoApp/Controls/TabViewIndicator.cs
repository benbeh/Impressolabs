using System;
using System.Collections.Generic;
using Xamarin.Forms;
using CarouselView.FormsPlugin.Abstractions;
using System.Windows.Input;
using System.Collections;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ImpressoApp.Controls
{
    public class TabViewIndicator : Grid
    {
        public static BindableProperty TabItemsSourceProperty = BindableProperty.Create(nameof(TabItemsSource), typeof(ICollection), typeof(TabViewIndicator));
        public static BindableProperty TabBackgroundColorProperty = BindableProperty.Create(nameof(TabBackgroundColor), typeof(Color), typeof(TabViewIndicator), Color.Default);
        public static BindableProperty TabSelectedBackgroundColorProperty = BindableProperty.Create(nameof(TabSelectedBackgroundColor), typeof(Color), typeof(TabViewIndicator), Color.Default);
        public static BindableProperty TabSelectedTextColorProperty = BindableProperty.Create(nameof(TabSelectedBackgroundColor), typeof(Color), typeof(TabViewIndicator), Color.Default);
        public static BindableProperty TabTextColorProperty = BindableProperty.Create(nameof(TabBackgroundColor), typeof(Color), typeof(TabViewIndicator), Color.Black);
        public static BindableProperty TabFontSizeProperty = BindableProperty.Create(nameof(TabFontSize), typeof(double), typeof(TabViewIndicator), 14.0);
        public static BindableProperty TabHeightProperty = BindableProperty.Create(nameof(TabHeight), typeof(double), typeof(TabViewIndicator), 40.0);
        public static BindableProperty SelectedTabIndexProperty = BindableProperty.Create(nameof(SelectedTabIndex), typeof(int), typeof(TabViewIndicator), 0);
        public static BindableProperty HasBottomLineProperty = BindableProperty.Create(nameof(HasBottomLine), typeof(bool), typeof(TabViewIndicator), true);
        public static BindableProperty HasFillIndicatorProperty = BindableProperty.Create(nameof(HasFillIndicator), typeof(bool), typeof(TabViewIndicator), false);
        public static BindableProperty TabSelectedCommandProperty = BindableProperty.Create(nameof(TabSelectedCommand), typeof(ICommand), typeof(TabViewIndicator));
        public static BindableProperty ViewBackgroundColorProperty = BindableProperty.Create(nameof(ViewBackgroundColor), typeof(Color), typeof(TabViewIndicator), Color.White);
        public static BindableProperty TabCornerRadiusProperty = BindableProperty.Create(nameof(TabCornerRadius), typeof(int), typeof(TabViewIndicator), 0);
        public static BindableProperty TabMarginProperty = BindableProperty.Create(nameof(TabMargin), typeof(Thickness), typeof(TabViewIndicator), default(Thickness));


        public ICollection TabItemsSource 
        {
            get => (ICollection)GetValue(TabItemsSourceProperty);
            set => SetValue(TabItemsSourceProperty, value);
        }

        public Color TabBackgroundColor
        {
            get => (Color)GetValue(TabBackgroundColorProperty);
            set => SetValue(TabBackgroundColorProperty, value);
        }

        public Color ViewBackgroundColor
        {
            get => (Color)GetValue(ViewBackgroundColorProperty);
            set => SetValue(ViewBackgroundColorProperty, value);
        }

        public Color TabSelectedBackgroundColor
        {
            get => (Color)GetValue(TabSelectedBackgroundColorProperty);
            set => SetValue(TabSelectedBackgroundColorProperty, value);
        }

        public Color TabSelectedTextColor
        {
            get => (Color)GetValue(TabSelectedTextColorProperty);
            set => SetValue(TabSelectedTextColorProperty, value);
        }

        public Color TabTextColor
        {
            get => (Color)GetValue(TabTextColorProperty);
            set => SetValue(TabTextColorProperty, value);
        }

        public int TabCornerRadius
        {
            get => (int)GetValue(TabCornerRadiusProperty);
            set => SetValue(TabCornerRadiusProperty, value);
        }

        public double TabFontSize
        {
            get => (double)GetValue(TabFontSizeProperty);
            set => SetValue(TabFontSizeProperty, value);
        }

        public double TabHeight
        {
            get => (double)GetValue(TabHeightProperty);
            set => SetValue(TabHeightProperty, value);
        }

        public int SelectedTabIndex
        {
            get => (int)GetValue(SelectedTabIndexProperty);
            set => SetValue(SelectedTabIndexProperty, value);
        }

        public bool HasBottomLine
        {
            get => (bool)GetValue(HasBottomLineProperty);
            set => SetValue(HasBottomLineProperty, value);
        }

        public bool HasFillIndicator
        {
            get => (bool)GetValue(HasFillIndicatorProperty);
            set => SetValue(HasFillIndicatorProperty, value);
        }

        public Thickness TabMargin
        {
            get => (Thickness)GetValue(TabMarginProperty);
            set => SetValue(TabMarginProperty, value);
        }

        public Command TabSelectedCommand
        {
            get => (Command)GetValue(TabSelectedCommandProperty);
            set => SetValue(TabSelectedCommandProperty, value);
        }

        private List<Button> buttons;
        private List<BoxView> indicators;
        private List<TabIndicatorModel> tabIndicatorModels;
        private int lastSelectedTabIndex;

        private ICommand TabPressCommand { get; set; }


        public TabViewIndicator()
        {
            TabPressCommand = new Command(TabPressCommandExecute);
        }

        private void TabPressCommandExecute(object obj)
        {
            var index = (int)obj;

            if(index == lastSelectedTabIndex)
            {
                return;
            }

            buttons[index].TextColor = TabSelectedTextColor;
            buttons[index].FontAttributes = FontAttributes.Bold;
            buttons[lastSelectedTabIndex].TextColor = TabTextColor;
            buttons[lastSelectedTabIndex].FontAttributes = FontAttributes.None;

            if(tabIndicatorModels != null && tabIndicatorModels.FirstOrDefault().IconSource != null)
            {
                buttons[index].Image = tabIndicatorModels[index].SelectedIconSource;
                buttons[lastSelectedTabIndex].Image = tabIndicatorModels[lastSelectedTabIndex].IconSource;
            }

            if (HasBottomLine)
            {
                indicators[index].BackgroundColor = TabSelectedTextColor;
                indicators[lastSelectedTabIndex].BackgroundColor = Color.Transparent;
            }

            if(HasFillIndicator)
            {
                buttons[index].BackgroundColor = TabSelectedBackgroundColor;
                buttons[lastSelectedTabIndex].BackgroundColor = TabBackgroundColor;
            }

            if(TabSelectedCommand != null && TabSelectedCommand.CanExecute(null))
            {
                TabSelectedCommand.Execute(index);
            }

            lastSelectedTabIndex = index;
        }

        private void Init ()
        {
            if(TabItemsSource == null)
            {
                return;
            }

            Padding = new Thickness(0);
            ColumnSpacing = 0;

            buttons = new List<Button>();
            indicators = new List<BoxView>();

            tabIndicatorModels = new List<TabIndicatorModel>();

            foreach(var item in TabItemsSource)
            {
                tabIndicatorModels.Add((TabIndicatorModel)item);
            }

            if (tabIndicatorModels == null || tabIndicatorModels.Count <= 0)
            {
                return;
            }

            var frame = new Frame
            {
                HeightRequest = TabHeight,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = ViewBackgroundColor,
                CornerRadius = TabCornerRadius,
                HasShadow = false
            };

            Children.Add(frame);
            Grid.SetColumnSpan(frame, tabIndicatorModels.Count);

            for (int i = 0; i < tabIndicatorModels.Count; i++)
            {
                ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

                var tabButton = new Button
                {
                    HeightRequest = TabHeight,
                    BackgroundColor = TabBackgroundColor,
                    HorizontalOptions = LayoutOptions.Fill,
                    TextColor = SelectedTabIndex == i ? TabSelectedTextColor : TabTextColor,
                    Image = SelectedTabIndex == i ? tabIndicatorModels[i].SelectedIconSource : tabIndicatorModels[i].IconSource,
                    Text = tabIndicatorModels[i].Title,
                    CornerRadius = TabCornerRadius,
                    Command = TabPressCommand,
                    CommandParameter = i,
                    Margin = TabMargin
                };

                if(HasFillIndicator)
                {
                    tabButton.BackgroundColor = SelectedTabIndex == i ? TabSelectedBackgroundColor : TabBackgroundColor;
                }


                if (HasBottomLine)
                {
                    var bottomIndicator = new BoxView
                    {
                        HeightRequest = 4,
                        WidthRequest = 100,
                        Margin = new Thickness(0, 0, 0, -2),
                        BackgroundColor = SelectedTabIndex == i ? TabSelectedTextColor : Color.Transparent,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.End
                    };

                    indicators.Add(bottomIndicator);
                    Children.Add(indicators[i], i, 0);
                }

                buttons.Add(tabButton);
                Children.Add(buttons[i], i, 0);
            }
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            Init();
        }
    }
}
