using ERP.WpfClient.View;
using ERP.WpfClient.View.Customers;

namespace ERP.WpfClient.LoadControls
{
    public interface ILoadControl
    {
        Customer CustomerForm { get; }
    }
}
