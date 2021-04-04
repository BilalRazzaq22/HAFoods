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
        DbSet<T> table;
        public GenericRepository()
        {
            table = DBInstance.Instance.Set<T>();
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
                T exist = DBInstance.Instance.Set<T>().Find(id);
                if (exist != null)
                {
                    DBInstance.Instance.Entry(exist).CurrentValues.SetValues(t);
                    Save();
                }
            }
            //table.Attach(t);
            //DBInstance.Instance.Entry(t).State = EntityState.Modified;
        }

        public T GetById(int id)
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
            DBInstance.Instance.SaveChanges();
        }
    }
}
