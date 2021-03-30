using ERP.Entities.DBModel;
using System.Collections.Generic;
using System.Linq;

namespace ERP.Repository.Customer
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly HAFoods_DBEntities db = new HAFoods_DBEntities();
       
        public List<Entities.DBModel.Customer> Get()
        {
            return db.Customers.ToList();
        }

        public void SaveCustomer(Entities.DBModel.Customer customer)
        {
            db.Customers.Add(customer);
            db.SaveChanges();
        }

        public void UpdateCustomer(Entities.DBModel.Customer customer)
        {
            Entities.DBModel.Customer cust = GetById(customer.Id);
            if(cust != null)
            {
                cust.FirstName = customer.FirstName;
                cust.LastName = customer.LastName;
                cust.ContactNo = customer.ContactNo;
                cust.Address = customer.Address;
            }
            db.SaveChanges();
        }

        public Entities.DBModel.Customer GetById(int id)
        {
            return db.Customers.Find(id);
        }
        
        public void DeleteCustomer(int id)
        {
            Entities.DBModel.Customer cust = GetById(id);
            if (cust != null)
            {
                db.Customers.Remove(cust);
            }
            db.SaveChanges();
        }
    }
}
