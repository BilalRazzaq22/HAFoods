using System;

namespace ERP.WpfClient.Messages.BusyIndicator
{
    public class BusyIndicatorMessage
    {
        public bool IsBusy { get; set; }
        public bool IsBusyNotice { get; set; }
        public bool IsErrorNotice { get; set; }
        public String BusyNoticeDetails { get; set; }
        public Action Action { get; set; }
    }
}
