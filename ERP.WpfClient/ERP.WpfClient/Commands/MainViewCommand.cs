using ERP.WpfClient.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ERP.WpfClient.Commands
{
    public class MainViewCommand : ICommand
    {
        public MainViewModel MainViewModel { get; set; }

        public MainViewCommand(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            MainViewModel.LoadViewAsync("Customer");
        }
    }
}
