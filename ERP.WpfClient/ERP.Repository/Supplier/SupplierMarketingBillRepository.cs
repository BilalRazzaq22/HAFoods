using ERP.Entities.DbContext;
using ERP.Entities.DBModel.Suppliers;
using ERP.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Repository.Supplier
{
    public class SupplierMarketingBillRepository : GenericRepository<SupplierMarketingOrder>
    {
        private IGenericRepository<SupplierMarketingOrder> _currentTransactionRepository;
        private HAFoodDbContext __context;
        public SupplierMarketingBillRepository(HAFoodDbContext _context) : base(_context)
        {
            __context = _context;
            _currentTransactionRepository = new GenericRepository<SupplierMarketingOrder>(_context);
        }

        public SupplierMarketingOrder SaveSupplierMarketingOrder(SupplierMarketingOrder supplierMarketingOrder)
        {
            try
            {
                SupplierMarketingOrder transaction = null;
                var currentOrder = _context.Set<SupplierMarketingOrder>();
                var currentOrderDetails = _context.Set<SupplierMarketingOrderItem>();
                var currentPayment = _context.Set<Entities.DBModel.Payments.Payment>();
                var order = currentOrder.Include("SupplierMarketingOrderItems").Where(x => x.Id == supplierMarketingOrder.Id).FirstOrDefault();

                if (order != null)
                    _context.Entry(order).CurrentValues.SetValues(supplierMarketingOrder);
                else
                    transaction = currentOrder.Add(supplierMarketingOrder);

                _context.SaveChanges();



                //var feeIds = supplierMarketingOrder.retailTransactionDetail.Select(s => s.id).ToList();

                //dbOrder.currentTransactionDetail.Where(f => !feeIds.Contains(f.id))
                //    .ToList()
                //    .ForEach(i => _context.Entry(i).State = EntityState.Deleted);

                //var paymentIds = retailTransaction.retailTransactionTaxes.Select(s => s.id).ToList();

                //dbOrder.retailTransactionTaxes.Where(f => !paymentIds.Contains(f.id))
                //    .ToList()
                //    .ForEach(i => _context.Entry(i).State = EntityState.Deleted);


                // add retail transaction items
                foreach (var item in supplierMarketingOrder.SupplierMarketingOrderItems.Distinct())
                {
                    //item.RetailOrder = null;
                    //if (transaction != null)
                    //else
                    //    item.CurrentTransactionId = supplierMarketingOrder.Id;

                    var dbItem = currentOrderDetails.Where(x => x.Id == item.Id).FirstOrDefault();

                    if (dbItem != null)
                    {
                        item.SupplierMarketingOrderId = supplierMarketingOrder.Id;
                        _context.Entry(dbItem).CurrentValues.SetValues(item);
                    }
                    else
                    {
                        item.SupplierMarketingOrderId = supplierMarketingOrder.Id;
                        order.SupplierMarketingOrderItems.Add(item);
                    }
                }

                _context.SaveChanges();
                return supplierMarketingOrder;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public SupplierMarketingOrder GetOrder(string Orderno)
        {
            var retailOrderTable = _context.Set<SupplierMarketingOrder>();
            var order = retailOrderTable.Include("SupplierMarketingOrderItems").Where(x => x.OrderNo == Orderno).FirstOrDefault();
            return order;
        }
    }
}
