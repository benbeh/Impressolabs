using System;
namespace ImpressoApp.Models.Event
{
    public class SetAsInterestedRequestModel
    {
        public string EventId { get; set; }

        public bool IsInterested { get; set; }
    }
}
