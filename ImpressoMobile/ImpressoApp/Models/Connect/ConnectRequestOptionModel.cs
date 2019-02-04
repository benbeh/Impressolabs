using System;
using PropertyChanged;
namespace ImpressoApp.Models.Connect
{
    [AddINotifyPropertyChangedInterface]
    public class ConnectRequestOptionModel
    {
        public string Name { get; set; }

        public bool IsSelected { get; set; }
    }
}
