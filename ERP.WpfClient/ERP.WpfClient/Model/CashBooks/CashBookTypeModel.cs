using ERP.Common.NotifyProperty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.WpfClient.Model.CashBooks
{
    public class CashBookTypeModel : ViewModelBase
    {
        private string _type;

        public string Type
        {
            get { return _type; }
            set { _type = value; RaisePropertyChanged("Type"); }
        }
    }
}
