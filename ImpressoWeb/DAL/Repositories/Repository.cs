using System.Collections.Generic;
using System.Linq;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbSet<T> _items;

        public Repository(DbSet<T> items)
        {
            _items = items;
        }

        public IQueryable<T> GetAll()
        {
            return _items;
        }

        public T Get(int id)
        {
            return _items.Find(id);
        }

        public T Add(T item)
        {
            return _items.Add(item).Entity;
        }

        public void Update(T item)
        {
            _items.Update(item);
        }

        public void Delete(int id)
        {
            T item = _items.Find(id);
            if (item != null)
                _items.Remove(item);
        }

        public void Delete(T item)
        {
            if (item != null)
                _items.Remove(item);
        }

        public void Delete(IEnumerable<T> items)
        {
            if (items != null && items.Count() != 0)
                _items.RemoveRange(items);
        }
    }
}