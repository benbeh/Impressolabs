using System.Collections.Generic;

namespace BLL.Interfaces
{
    public interface IService<TEntity, TViewModel> where TEntity : class where TViewModel : class
    {
        IEnumerable<TViewModel> GetAll();

        IEnumerable<TViewModel> GetAllAsNoTracking();

        TViewModel Get(int? id);

        TViewModel Add(TViewModel T);

        void Update(TViewModel T);

        void Delete(int? id);

        void Dispose();

        void SaveChanges();

        void DetachAllEntities();
    }
}