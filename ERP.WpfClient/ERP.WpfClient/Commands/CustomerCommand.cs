using ERP.WpfClient.ViewModel;
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
            switch (parameter as string)
            {
                case "SaveCustomer":
                    CustomerViewModel.SaveCustomer();
                    break;
                default:
                    break;
            }
        }
    }
}
