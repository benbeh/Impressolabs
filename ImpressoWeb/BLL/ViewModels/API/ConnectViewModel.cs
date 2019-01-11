using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.ViewModels.API
{
    public class ConnectViewModel
    {
        public IEnumerable<PeopleConnectViewModel> People { get; set; }
        public IEnumerable<EventViewModel> Events { get; set; }
    }
}
