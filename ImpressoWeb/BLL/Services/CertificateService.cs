using BLL.Interfaces;
using BLL.ViewModels;
using Core.Entities;
using DAL.Interfaces;

namespace BLL.Services
{
    public class CertificateService : Service<Certificate, CertificateViewModel>, ICertificateService
    {
        public CertificateService(IUnitOfWork unitOfWork) : base(unitOfWork, unitOfWork.Certificates)
        {
        }
    }
}