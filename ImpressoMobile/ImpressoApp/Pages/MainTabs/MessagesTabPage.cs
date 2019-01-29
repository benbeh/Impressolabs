using System;

using Xamarin.Forms;

namespace ImpressoApp.Pages.MainTabs
{
    public class MessagesTabPage : ContentPage
    {
        public MessagesTabPage()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Hello ContentPage" }
                }
            };
        }
    }
}

