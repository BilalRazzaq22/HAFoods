using ERP.Entities.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Repository.Transaction
{
    public class CurrentTransactionRepository : ICurrentTransactionRepository
    {
        public void SaveDetail(CurrentTransaction currentTransaction)
        {
            try
            {
                CurrentTransaction transaction = null;
                var currentOrder = DBInstance.Instance.Set<CurrentTransaction>();
                var currentOrderDetails = DBInstance.Instance.Set<CurrentTransactionDetail>();
                var currentPayment = DBInstance.Instance.Set<Payment>();
                var order = currentOrder.Include("CurrentTransactionDetails").Where(x => x.Id == currentTransaction.Id).FirstOrDefault();

                if (order != null)
                    DBInstance.Instance.Entry(order).CurrentValues.SetValues(currentTransaction);
                else
                    transaction = currentOrder.Add(currentTransaction);

                DBInstance.Instance.SaveChanges();



                //var feeIds = currentTransaction.retailTransactionDetail.Select(s => s.id).ToList();

                //dbOrder.currentTransactionDetail.Where(f => !feeIds.Contains(f.id))
                //    .ToList()
                //    .ForEach(i => DBInstance.Instance.Entry(i).State = EntityState.Deleted);

                //var paymentIds = retailTransaction.retailTransactionTaxes.Select(s => s.id).ToList();

                //dbOrder.retailTransactionTaxes.Where(f => !paymentIds.Contains(f.id))
                //    .ToList()
                //    .ForEach(i => DBInstance.Instance.Entry(i).State = EntityState.Deleted);


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
                        DBInstance.Instance.Entry(dbItem).CurrentValues.SetValues(item);
                    }
                    else
                    {
                        item.CurrentTransactionId = currentTransaction.Id;
                        order.CurrentTransactionDetails.Add(item);
                    }
                }

                DBInstance.Instance.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CurrentTransaction GetOrder(string Orderno)
        {
            var retailOrderTable = DBInstance.Instance.Set<CurrentTransaction>();
            var order = retailOrderTable.Include("CurrentTransactionDetails").Where(x => x.OrderNo == Orderno).FirstOrDefault();
            return order;
        }
    }
}
