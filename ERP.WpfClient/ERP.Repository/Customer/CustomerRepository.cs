using ERP.Entities.DBModel;
using System.Collections.Generic;
using System.Linq;

namespace ERP.Repository.Customer
{
    public class CustomerRepository : ICustomerRepository
    {
        //private readonly HAFoods_DBEntities DBInstance.Instance = new HAFoods_DBEntities();
       
        public List<Entities.DBModel.Customer> Get()
        {
            return DBInstance.Instance.Customers.ToList();
        }

        public void SaveCustomer(Entities.DBModel.Customer customer)
        {
            DBInstance.Instance.Customers.Add(customer);
            DBInstance.Instance.SaveChanges();
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
            DBInstance.Instance.SaveChanges();
        }

        public Entities.DBModel.Customer GetById(int id)
        {
            return DBInstance.Instance.Customers.Find(id);
        }
        
        public void DeleteCustomer(int id)
        {
            Entities.DBModel.Customer cust = GetById(id);
            if (cust != null)
            {
                DBInstance.Instance.Customers.Remove(cust);
            }
            DBInstance.Instance.SaveChanges();
        }
    }
}
