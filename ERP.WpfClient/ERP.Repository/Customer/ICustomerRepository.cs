using System.Collections.Generic;

namespace ERP.Repository.Customer
{
    public interface ICustomerRepository
    {
        List<Entities.DBModel.Customer> Get();
        void SaveCustomer(Entities.DBModel.Customer customer);
        void UpdateCustomer(Entities.DBModel.Customer customer);
        Entities.DBModel.Customer GetById(int id);
        void DeleteCustomer(int id);
    }
}
