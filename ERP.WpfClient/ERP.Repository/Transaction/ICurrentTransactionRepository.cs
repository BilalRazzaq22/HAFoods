using ERP.Entities.DBModel;
using ERP.Entities.DBModel.Transactions;

namespace ERP.Repository.Transaction
{
    public interface ICurrentTransactionRepository
    {
        void SaveDetail(CurrentTransaction currentTransaction);
        CurrentTransaction GetOrder(string Orderno);
    }
}
