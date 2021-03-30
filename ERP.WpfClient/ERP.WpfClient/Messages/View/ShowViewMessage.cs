using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.WpfClient.Messages.View
{
    public class ShowViewMessage : MessageBase
    {
        public string ViewName { get; set; }
        public Action ActionToExecute { get; set; }
        public object PrimaryKey { get; set; }
        public bool UseAdminVerification { get; set; }
        public object DataContext { get; set; }
    }
}
