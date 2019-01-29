using System;
using Newtonsoft.Json;

namespace ImpressoApp.Models.Token
{
    public class TokenServerModel
    {
        public int Id { get; set; }
        public DateTime DepartureDate { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public int Count { get; set; }
        public string Message { get; set; }


        [JsonIgnoreAttribute]
        public bool IsExpanded { get; set; }

        [JsonIgnore]
        public string Title { get => $"{Count} IMP"; }

    }
}
