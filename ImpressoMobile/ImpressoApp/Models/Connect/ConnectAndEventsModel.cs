using System;
using System.Collections.Generic;
using ImpressoApp.Models.Feeds;
namespace ImpressoApp.Models.Connect
{
    public class ConnectAndEventsModel : BaseResponseModel
    { 
        public List<ConnectPeopleModel> People { get; set; }

        public List<ConnectEventModel> Events { get; set; }
     }
}
