using System;
using System.Collections.Generic;
using BLL.AutoMapper;
using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Repositories;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public abstract class Service<TEntity, TViewModel> : IService<TEntity, TViewModel> where TEntity : class where TViewModel : class
    {
        public IUnitOfWork Database { get; set; }
        private readonly IRepository<TEntity> _repository;

        public Service(IUnitOfWork unitOfWork, IRepository<TEntity> repository)
        {
            Database = unitOfWork;
            _repository = repository;
        }

        public IEnumerable<TViewModel> GetAll()
        {
            return Mapping.Map<IEnumerable<TEntity>, IEnumerable<TViewModel>>(_repository.GetAll());
        }

        public IEnumerable<TViewModel> GetAllAsNoTracking()
        {
            return Mapping.Map<IEnumerable<TEntity>, IEnumerable<TViewModel>>(_repository.GetAll().AsNoTracking());
        }

        public TViewModel Get(int? id)
        {
            if (id == null)
                throw new Exception("Id wasn't set");
            var item = _repository.Get(id.Value);
            if (item == null)
                throw new Exception(typeof(TEntity) + " wasn't found");
            return Mapping.Map<TEntity, TViewModel>(item);
        }

        public TViewModel Add(TViewModel T)
        {
            if (T == null)
                throw new Exception(typeof(TEntity) + " wasn't set");
            var result = _repository.Add(Mapping.Map<TViewModel, TEntity>(T));
            Database.Save();
            return Mapping.Map<TEntity, TViewModel>(result);
        }

        public void Update(TViewModel T)
        {
            if (T == null)
                throw new Exception(typeof(TEntity) + " wasn't set");
            _repository.Update(Mapping.Map<TViewModel, TEntity>(T));
            Database.Save();
        }

        public void Delete(int? id)
        {
            if (id == null)
                throw new Exception("Id wasn't found");

            _repository.Delete(id.Value);
            Database.Save();
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public void SaveChanges()
        {
            Database.Save();
        }

        public void DetachAllEntities()
        {
            Database.DetachAllEntities();
        }
    }
}