using ERP.Entities.DBModel;

namespace ERP.Repository.Transaction
{
    public interface ICurrentTransactionRepository
    {
        void SaveDetail(CurrentTransaction currentTransaction);
        CurrentTransaction GetOrder(string Orderno);
    }
}
