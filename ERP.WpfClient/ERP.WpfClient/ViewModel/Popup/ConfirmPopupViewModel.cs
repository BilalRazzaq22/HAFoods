using ERP.WpfClient.ViewModels.Data;
using GalaSoft.MvvmLight.Command;
using System;

namespace ERP.WpfClient.ViewModel.Popup
{
    public class ConfirmPopupViewModel : DataViewModelBase
    {
        private string _message;

        #region Properties
        public bool UseYesNo { get; set; }
        public Action CancelAction { get; set; }
        public Action YesAction { get; set; }

        public string Message
        {
            get { return _message; }
            set { _message = value; RaisePropertyChanged("Message"); }
        }

        #endregion

        #region Constructor()

        public ConfirmPopupViewModel()
        {
            ActionCommand = new RelayCommand<string>(ExeccuteAction);
        }
        #endregion

        #region Relay Commands
        public RelayCommand<string> ActionCommand { get; private set; }
        #endregion

        #region Methods

        private void ExeccuteAction(string action)
        {
            switch (action)
            {
                case "Cancel":
                    ApplicationManager.HideMessageBox();

                    if (CancelAction != null)
                    {
                        CancelAction();
                    }

                    break;
                case "Yes":
                    ApplicationManager.HideMessageBox();

                    if (YesAction != null)
                    {
                        YesAction();
                    }

                    break;
            }
        }
        #endregion
    }
}
