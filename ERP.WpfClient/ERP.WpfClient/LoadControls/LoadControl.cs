using ERP.WpfClient.View;
using ERP.WpfClient.View.Customers;

namespace ERP.WpfClient.LoadControls
{
    public class LoadControl : ILoadControl
    {
        private Customer _customer;
        public Customer CustomerForm
        {
            get
            {
                if (_customer == null)
                    _customer = new Customer();
                return _customer;
            }
        }
    }
}
