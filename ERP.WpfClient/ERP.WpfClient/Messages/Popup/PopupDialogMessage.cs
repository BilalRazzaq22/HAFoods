using GalaSoft.MvvmLight.Messaging;
using System.Windows;

namespace ERP.WpfClient.Messages.Popup
{
    public class PopupDialogMessage : MessageBase
    {
        public string Title { get; set; }
        public bool Show { get; set; }
        public FrameworkElement ControlToDisplay { get; set; }
        public bool ShowCloseButton { get; set; }
        public bool UseMessageBox { get; set; }
        public bool UseSecondPopup { get; set; }
        public bool UpdateHeadingOnly { get; set; }
        public bool ShowLastDialog { get; set; }
        public bool HideOnly { get; set; }
    }
}
