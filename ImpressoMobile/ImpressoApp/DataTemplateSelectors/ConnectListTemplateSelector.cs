using System;
using Xamarin.Forms;
using ImpressoApp.Models.Feeds;
using ImpressoApp.UserControls.Feeds;

namespace ImpressoApp.DataTemplateSelectors
{
    public class ConnectListTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ConnectPeopleViewTemplate { get; set; }
        public DataTemplate ConnectEventViewTemplate { get; set; }

        public ConnectListTemplateSelector()
        {
            ConnectPeopleViewTemplate = new DataTemplate(typeof(ConnectPeopleItemView));
            ConnectEventViewTemplate = new DataTemplate(typeof(ConnectEventItemView));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if(item is ConnectEventModel)
            {
                return ConnectEventViewTemplate;
            }

            return ConnectPeopleViewTemplate;
        }
    }
}
