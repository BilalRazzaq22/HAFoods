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
    public class CustomerMarketingBillRepository : GenericRepository<CustomerMarketingOrder>
    {
        private IGenericRepository<CustomerMarketingOrder> _currentTransactionRepository;
        private HAFoodDbContext __context;
        public CustomerMarketingBillRepository(HAFoodDbContext _context) : base(_context)
        {
            __context = _context;
            _currentTransactionRepository = new GenericRepository<CustomerMarketingOrder>(_context);
        }

        public CustomerMarketingOrder SaveCustomerOrder(CustomerMarketingOrder customerMarketingOrder)
        {
            try
            {
                CustomerMarketingOrder transaction = null;
                var currentOrder = _context.Set<CustomerMarketingOrder>();
                var currentOrderDetails = _context.Set<CustomerMarketingOrderItem>();
                var currentPayment = _context.Set<Entities.DBModel.Payments.Payment>();
                var order = currentOrder.Include("CustomerMarketingOrderItems").Where(x => x.Id == customerMarketingOrder.Id).FirstOrDefault();

                if (order != null)
                    _context.Entry(order).CurrentValues.SetValues(customerMarketingOrder);
                else
                    transaction = currentOrder.Add(customerMarketingOrder);

                _context.SaveChanges();



                //var feeIds = customerMarketingOrder.retailTransactionDetail.Select(s => s.id).ToList();

                //dbOrder.currentTransactionDetail.Where(f => !feeIds.Contains(f.id))
                //    .ToList()
                //    .ForEach(i => _context.Entry(i).State = EntityState.Deleted);

                //var paymentIds = retailTransaction.retailTransactionTaxes.Select(s => s.id).ToList();

                //dbOrder.retailTransactionTaxes.Where(f => !paymentIds.Contains(f.id))
                //    .ToList()
                //    .ForEach(i => _context.Entry(i).State = EntityState.Deleted);


                // add retail transaction items
                foreach (var item in customerMarketingOrder.CustomerMarketingOrderItems.Distinct())
                {
                    //item.RetailOrder = null;
                    //if (transaction != null)
                    //else
                    //    item.CurrentTransactionId = customerMarketingOrder.Id;

                    var dbItem = currentOrderDetails.Where(x => x.Id == item.Id).FirstOrDefault();

                    if (dbItem != null)
                    {
                        item.CustomerMarketingOrderId = customerMarketingOrder.Id;
                        _context.Entry(dbItem).CurrentValues.SetValues(item);
                    }
                    else
                    {
                        item.CustomerMarketingOrderId = customerMarketingOrder.Id;
                        order.CustomerMarketingOrderItems.Add(item);
                    }
                }

                _context.SaveChanges();
                return customerMarketingOrder;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CustomerMarketingOrder GetOrder(string Orderno)
        {
            var retailOrderTable = _context.Set<CustomerMarketingOrder>();
            var order = retailOrderTable.Include("CustomerMarketingOrderItems").Where(x => x.OrderNo == Orderno).FirstOrDefault();
            return order;
        }
    }
}
