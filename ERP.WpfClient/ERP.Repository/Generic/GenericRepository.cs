using ERP.Entities.DbContext;
using ERP.Entities.DBModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Repository.Generic
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected HAFoodDbContext _context;
        DbSet<T> table;

        public GenericRepository()
        {
            _context = new HAFoodDbContext();
            table = _context.Set<T>();
        }

        public GenericRepository(HAFoodDbContext context)
        {
            _context = context;
            table = _context.Set<T>();
        }

        public List<T> Get()
        {
            return table.ToList();
        }

        public T Add(T t)
        {
            table.Add(t);
            Save();
            return t;
        }

        public void Update(T t, int id)
        {
            if (t != null)
            {
                T exist = table.Find(id);
                if (exist != null)
                {
                    _context.Entry(exist).CurrentValues.SetValues(t);
                    Save();
                }
            }
        }

        public T GetById(int? id)
        {
            return table.Find(id);
        }

        public void Delete(int id)
        {
            T existing = table.Find(id);
            table.Remove(existing);
            Save();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
