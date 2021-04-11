using ERP.Entities.DbContext;
using ERP.Entities.DBModel.Transactions;
using ERP.Repository.Generic;
using System;
using System.Linq;

namespace ERP.Repository.Transaction
{
    public class CurrentTransactionRepository : GenericRepository<CurrentTransaction>, ICurrentTransactionRepository
    {
        private IGenericRepository<CurrentTransaction> _currentTransactionRepository;
        private HAFoodDbContext __context;
        public CurrentTransactionRepository(HAFoodDbContext _context) : base (_context)
        {
            __context = _context;
            _currentTransactionRepository = new GenericRepository<CurrentTransaction>(_context);
        }

        public void SaveDetail(CurrentTransaction currentTransaction)
        {
            try
            {
                CurrentTransaction transaction = null;
                var currentOrder = _context.Set<CurrentTransaction>();
                var currentOrderDetails = _context.Set<CurrentTransactionDetail>();
                var currentPayment = _context.Set<Entities.DBModel.Payments.Payment>();
                var order = currentOrder.Include("CurrentTransactionDetails").Where(x => x.Id == currentTransaction.Id).FirstOrDefault();

                if (order != null)
                    _context.Entry(order).CurrentValues.SetValues(currentTransaction);
                else
                    transaction = currentOrder.Add(currentTransaction);

                _context.SaveChanges();



                //var feeIds = currentTransaction.retailTransactionDetail.Select(s => s.id).ToList();

                //dbOrder.currentTransactionDetail.Where(f => !feeIds.Contains(f.id))
                //    .ToList()
                //    .ForEach(i => _context.Entry(i).State = EntityState.Deleted);

                //var paymentIds = retailTransaction.retailTransactionTaxes.Select(s => s.id).ToList();

                //dbOrder.retailTransactionTaxes.Where(f => !paymentIds.Contains(f.id))
                //    .ToList()
                //    .ForEach(i => _context.Entry(i).State = EntityState.Deleted);


                // add retail transaction items
                foreach (var item in currentTransaction.CurrentTransactionDetails.Distinct())
                {
                    //item.RetailOrder = null;
                    //if (transaction != null)
                    //else
                    //    item.CurrentTransactionId = currentTransaction.Id;

                    var dbItem = currentOrderDetails.Where(x => x.Id == item.Id).FirstOrDefault();

                    if (dbItem != null)
                    {
                        item.CurrentTransactionId = currentTransaction.Id;
                        _context.Entry(dbItem).CurrentValues.SetValues(item);
                    }
                    else
                    {
                        item.CurrentTransactionId = currentTransaction.Id;
                        order.CurrentTransactionDetails.Add(item);
                    }
                }

                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CurrentTransaction GetOrder(string Orderno)
        {
            var retailOrderTable = _context.Set<CurrentTransaction>();
            var order = retailOrderTable.Include("CurrentTransactionDetails").Where(x => x.OrderNo == Orderno).FirstOrDefault();
            return order;
        }
    }
}
