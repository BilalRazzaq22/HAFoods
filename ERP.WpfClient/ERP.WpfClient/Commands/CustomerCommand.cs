using ERP.WpfClient.Model;
using ERP.WpfClient.ViewModel.Customer;
using System;
using System.Windows.Input;

namespace ERP.WpfClient.Commands
{
    public class CustomerCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public CustomerViewModel CustomerViewModel { get; set; }

        public CustomerCommand(CustomerViewModel customerViewModel)
        {
            CustomerViewModel = customerViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if(parameter as string == "SaveCustomer")
            {
                CustomerViewModel.SaveCustomer();
            }
            else if (parameter as string == "UpdateCustomer")
            {
                CustomerViewModel.UpdateCustomer();
            }
            else if(parameter != null)
            {
                CustomerViewModel.EditCustomer(parameter as CustomerModel);
            }
        }
    }
}
