using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class AppUserCertificate
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public int CertificateId { get; set; }
        public Certificate Certificate { get; set; }
    }
}
