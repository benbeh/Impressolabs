namespace Core.Entities
{
    public class CompanyCertificate
    {
        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public int CertificateId { get; set; }
        public Certificate Certificate { get; set; }
    }
}