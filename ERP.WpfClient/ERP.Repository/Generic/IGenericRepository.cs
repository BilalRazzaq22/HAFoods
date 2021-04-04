using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Repository.Generic
{
    public interface IGenericRepository<T> where T : class
    {
        List<T> Get();
        T Add(T t);
        void Update(T t, int id);
        T GetById(int id);
        void Delete(int id);
        void Save();
    }
}
