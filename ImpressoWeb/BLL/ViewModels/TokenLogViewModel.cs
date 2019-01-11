using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.ViewModels
{
    public class TokenLogViewModel
    {
        public int Id { get; set; }        
        public DateTime DepartureDate { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public int Count { get; set; }
        public string Message { get; set; }
    }
}
