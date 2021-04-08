using ERP.Entities.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Repository.Transaction
{
    public class CurrentTransactionDetailRepository
    {
        public void SaveDetail(CurrentTransaction currentTransaction)
        {
            try
            {
                var retailOrderTable = DBInstance.Instance.Set<CurrentTransaction>();
                var retailOrderItemsTable = DBInstance.Instance.Set<CurrentTransactionDetail>();

                var result = retailOrderTable.Add(currentTransaction);
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

                    item.CurrentTransactionId = result.Id;

                    var dbItem = retailOrderItemsTable.Where(x => x.Id == item.Id).FirstOrDefault();

                    if (dbItem != null)
                    {
                        DBInstance.Instance.Entry(dbItem).CurrentValues.SetValues(item);
                    }
                    else
                    {
                        currentTransaction.CurrentTransactionDetails.Add(item);
                    }
                }
                DBInstance.Instance.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
