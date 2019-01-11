using BLL.Interfaces;
using BLL.ViewModels;
using DAL.Interfaces;
using Core.Entities;

namespace BLL.Services
{
    public class SkillService : Service<Skill, SkillViewModel>, ISkillService
    {
        public SkillService(IUnitOfWork unitOfWork) : base(unitOfWork, unitOfWork.Skills)
        {
        }
    }
}