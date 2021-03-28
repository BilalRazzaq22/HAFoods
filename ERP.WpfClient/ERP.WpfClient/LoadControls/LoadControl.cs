using ERP.WpfClient.View;

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
