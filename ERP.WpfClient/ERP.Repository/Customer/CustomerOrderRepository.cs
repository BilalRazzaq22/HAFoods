using ERP.Entities.DbContext;
using ERP.Entities.DBModel.Customers;
using ERP.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Repository.Customer
{
    public class CustomerOrderRepository : GenericRepository<CustomerOrder>
    {
        private IGenericRepository<CustomerOrder> _customerOrderRepository;
        private HAFoodDbContext __context;
        public CustomerOrderRepository(HAFoodDbContext _context) : base(_context)
        {
            __context = _context;
            _customerOrderRepository = new GenericRepository<CustomerOrder>(_context);
        }

        public void SaveCustomerOrderAmount(CustomerOrder customerOrder)
        {
            var customer = _context.Set<CustomerOrder>();
            var currentCustomer = customer.FirstOrDefault(x => x.CustomerId == customerOrder.CustomerId);

            if (currentCustomer == null)
            {
                _customerOrderRepository.Add(customerOrder);
            }
            else
            {
                customerOrder.Id = currentCustomer.Id;
                _customerOrderRepository.Update(customerOrder, customerOrder.Id);
            }
            _customerOrderRepository.Save();
        }
    }
}
