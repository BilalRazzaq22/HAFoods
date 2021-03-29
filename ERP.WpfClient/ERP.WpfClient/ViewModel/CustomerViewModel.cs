using ERP.Common.NotifyProperty;
using ERP.Entities.DBModel;
using ERP.WpfClient.Commands;
using ERP.WpfClient.Mapper;
using ERP.WpfClient.Model;

namespace ERP.WpfClient.ViewModel
{
    public class CustomerViewModel : ViewModelBase
    {
        #region Fields

        //private CustomerRepository _customerRepository;
        private CustomerModel _customerModel;

        #endregion

        #region Ctor

        public CustomerViewModel()
        {
            this.CustomerCommands = new CustomerCommand(this);
            //_customerRepository = new CustomerRepository();
            CustomerModel = new CustomerModel();
        }

        #endregion

        #region Properties
        public CustomerCommand CustomerCommands { get; set; }

        public CustomerModel CustomerModel
        {
            get { return _customerModel; }
            set { _customerModel = value; RaisePropertyChanged("CustomerModel"); }
        }

        #endregion

        #region Methods

        public void SaveCustomer()
        {
            //_customerRepository.SaveCustomer(MapperProfile.iMapper.Map<Customer>(CustomerModel));
        }

        #endregion
    }
}
