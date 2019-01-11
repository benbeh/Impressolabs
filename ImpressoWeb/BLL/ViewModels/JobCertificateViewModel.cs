using BLL.ViewModels.API;

namespace BLL.ViewModels
{
    public class JobCertificateViewModel
    {
        public int JobId { get; set; }
        public JobViewModel Job { get; set; }

        public int CertificateId { get; set; }
        public CertificateViewModel Certificate { get; set; }
    }
}