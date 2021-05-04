using ERP.Entities.DbContext;
using ERP.Entities.DBModel.PurchaseOrders;
using ERP.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Repository.PurchaseOrders
{
    public class PurchaseOrderRepository : GenericRepository<PurchaseOrder>
    {
        private IGenericRepository<PurchaseOrder> _currentTransactionRepository;
        private HAFoodDbContext __context;
        public PurchaseOrderRepository(HAFoodDbContext _context) : base(_context)
        {
            __context = _context;
            _currentTransactionRepository = new GenericRepository<PurchaseOrder>(_context);
        }

        public PurchaseOrder SavePurchaseOrder(PurchaseOrder purchaseOrder)
        {
            try
            {
                PurchaseOrder pOrder = null;
                var currentOrder = _context.Set<PurchaseOrder>();
                var currentOrderDetails = _context.Set<PurchaseOrderItem>();
                var currentPayment = _context.Set<Entities.DBModel.Payments.Payment>();
                var order = currentOrder.Include("PurchaseOrderItems").Where(x => x.Id == purchaseOrder.Id).FirstOrDefault();

                if (order != null)
                    _context.Entry(order).CurrentValues.SetValues(purchaseOrder);
                else
                    pOrder = currentOrder.Add(purchaseOrder);

                _context.SaveChanges();



                //var feeIds = purchaseOrder.retailTransactionDetail.Select(s => s.id).ToList();

                //dbOrder.currentTransactionDetail.Where(f => !feeIds.Contains(f.id))
                //    .ToList()
                //    .ForEach(i => _context.Entry(i).State = EntityState.Deleted);

                //var paymentIds = retailTransaction.retailTransactionTaxes.Select(s => s.id).ToList();

                //dbOrder.retailTransactionTaxes.Where(f => !paymentIds.Contains(f.id))
                //    .ToList()
                //    .ForEach(i => _context.Entry(i).State = EntityState.Deleted);


                // add retail pOrder items
                foreach (var item in purchaseOrder.PurchaseOrderItems.Distinct())
                {
                    //item.RetailOrder = null;
                    //if (pOrder != null)
                    //else
                    //    item.CurrentTransactionId = purchaseOrder.Id;

                    var dbItem = currentOrderDetails.Where(x => x.Id == item.Id).FirstOrDefault();

                    if (dbItem != null)
                    {
                        item.PurchaseOrderId = purchaseOrder.Id;
                        _context.Entry(dbItem).CurrentValues.SetValues(item);
                    }
                    else
                    {
                        item.PurchaseOrderId = purchaseOrder.Id;
                        order.PurchaseOrderItems.Add(item);
                    }
                }

                _context.SaveChanges();
                return purchaseOrder;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public PurchaseOrder GetOrder(string Orderno)
        {
            var retailOrderTable = _context.Set<PurchaseOrder>();
            var order = retailOrderTable.Include("PurchaseOrderItems").Where(x => x.OrderNo == Orderno).FirstOrDefault();
            return order;
        }
    }
}
