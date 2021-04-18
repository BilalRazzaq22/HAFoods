using ERP.Common.NotifyProperty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.WpfClient.Model.User
{
    public class UserGroupModel : ViewModelBase
    {
        private int _id;
        private string _groupName;

        public int Id
        {
            get { return _id; }
            set { _id = value; RaisePropertyChanged("Id"); }
        }

        public string GroupName
        {
            get { return _groupName; }
            set { _groupName = value; RaisePropertyChanged("GroupName"); }
        }
    }
}
