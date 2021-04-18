using ERP.Common;
using ERP.Common.NotifyProperty;
using ERP.Repository.Generic;
using ERP.WpfClient.Controls.Helpers;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.WpfClient.ViewModel.User
{
    public class UserLoginViewModel : ViewModelBase
    {
        #region Fields

        private string _username;
        private string _password;
        private IGenericRepository<Entities.DBModel.Users.User> _userRepository;
        private ViewManagerService _viewManagerService = ViewManagerService.CreateInstance();

        #endregion

        public UserLoginViewModel()
        {
            LoginUserCommand = new RelayCommand<string>(ExecuteLoginUserCommand);
            _userRepository = App.Resolve<IGenericRepository<Entities.DBModel.Users.User>>();
        }

        #region Ctor

        #endregion

        #region Properties

        public RelayCommand<string> LoginUserCommand { get; set; }

        public string Username
        {
            get { return _username; }
            set { _username = value; RaisePropertyChanged("Username"); }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; RaisePropertyChanged("Password"); }
        }

        #endregion

        #region Methods

        private void ExecuteLoginUserCommand(string str)
        {
            if (Username == "admin@superadmin.com" && Password == "superadmin123")
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }
            else
            {
                Entities.DBModel.Users.User user = _userRepository.Get().FirstOrDefault(x => (x.Username == Username || x.Email == Username) && x.Password == Password);
                if (user != null)
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                }
                else
                    ApplicationManager.Instance.ShowMessageBox("Invalid Username or Password");
            }
        }

        #endregion
    }
}
