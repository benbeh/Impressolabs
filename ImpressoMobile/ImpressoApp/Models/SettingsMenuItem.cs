using System;
using System.Windows.Input;
namespace ImpressoApp.Models
{
    public class MenuItemModel
    {
        public string Title { get; set; }
        public int NotificationsCount { get; set; }
        public ICommand TapCommand { get; set; }
    }
}
