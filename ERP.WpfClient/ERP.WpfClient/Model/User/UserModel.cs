using ERP.Common.NotifyProperty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.WpfClient.Model.User
{
    public class UserModel : ViewModelBase
    {
        private int _id;
        private string _username;
        private string _email;
        private string _password;
        private string _userGroup;

        public int Id
        {
            get { return _id; }
            set { _id = value; RaisePropertyChanged("Id"); }
        }

        public string Username
        {
            get { return _username; }
            set { _username = value; RaisePropertyChanged("Username"); }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; RaisePropertyChanged("Email"); }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; RaisePropertyChanged("Password"); }
        }

        public string UserGroup
        {
            get { return _userGroup; }
            set { _userGroup = value; RaisePropertyChanged("UserGroup"); }
        }
    }
}
