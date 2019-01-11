using Core.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities
{
    public class ConnectedUser
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public string ConnectedAppUserId { get; set; }
        public AppUser ConnectedAppUser { get; set; }

        public string Reason { get; set; }
    }
}
