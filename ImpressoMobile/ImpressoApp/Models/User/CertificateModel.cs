using System;
using Newtonsoft.Json;
namespace ImpressoApp.Models.User
{
    public class CertificateModel
    {
        public int Id { get; set; }

        public bool IsVerified { get; set; }

        public string Title { get; set; }

        [JsonIgnore]
        public string VerifyText 
        {
            get => IsVerified ? "Verified" : "Not verified";
        }
    }
}
