using ERP.Entities.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.Common;

namespace ERP.Repository.CustomerRepo
{
    public class CustomerRepository
    {
        private HAFoods_DBEntities db = new HAFoods_DBEntities();

        public void SaveCustomer(Customer customer)
        {
            db.Customers.Add(customer);
            db.SaveChanges();
        }
    }
}
