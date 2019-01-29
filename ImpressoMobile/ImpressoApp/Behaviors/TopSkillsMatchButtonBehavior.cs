using System;
using Xamarin.Forms;
using ImpressoApp.Controls;
namespace ImpressoApp.Behaviors
{
    public class TopSkillsMatchButtonBehavior : Behavior<Button>
    {
        protected override void OnAttachedTo(BindableObject bindable)
        {
            base.OnAttachedTo(bindable);

            bindable.PropertyChanged += Bindable_PropertyChanged;
        }

        protected override void OnDetachingFrom(Button bindable)
        {
            base.OnDetachingFrom(bindable);

            bindable.PropertyChanged -= Bindable_PropertyChanged;
        }

        void Bindable_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == VisualElement.IsEnabledProperty.PropertyName)
            {
                if(sender is Button button)
                {
                    button.BackgroundColor = button.IsEnabled ?
                        (Color)Application.Current.Resources["MagentaColor"] :
                        Color.White;

                    button.BorderColor = button.IsEnabled ?
                        (Color)Application.Current.Resources["MagentaColor"] :
                        (Color)Application.Current.Resources["GrayColor"];
                }
            }
        }

    }
}
