namespace Core.Entities
{
    public class JobCertificate
    {
        public int JobId { get; set; }
        public Job Job { get; set; }

        public int CertificateId { get; set; }
        public Certificate Certificate { get; set; }
    }
}
