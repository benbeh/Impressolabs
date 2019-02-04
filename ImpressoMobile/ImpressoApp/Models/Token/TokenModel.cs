using System;
using ImpressoApp.Enums;
using Android.Text;
using Newtonsoft.Json;
using PropertyChanged;
namespace ImpressoApp.Models.Token
{
    [AddINotifyPropertyChangedInterfaceAttribute]
    public class TokenModel
    {
        public SendReceiveTokenType SendReceiveTokenType { get; set; }
        public DateTime Timestamp { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }

        [JsonIgnoreAttribute]
        public bool IsExpanded { get; set; }

        [JsonIgnoreAttribute]
        public string Title 
        { 
            get { return string.Format("{0} {1} IMP", SendReceiveTokenType.ToString(), Amount); }
        }
    }
}
