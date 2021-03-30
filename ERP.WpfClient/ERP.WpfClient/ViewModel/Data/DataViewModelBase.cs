using ERP.Common;
using ERP.WpfClient.Controls.Helpers;
using ERP.WpfClient.Messages.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace ERP.WpfClient.ViewModels.Data
{
    public class DataViewModelBase : ViewModelBase, IViewClient
    {
        private readonly ViewManagerService _viewManagerService;
        private NavigateKey _objNavigateKey;

        protected DataViewModelBase()
        {
            _viewManagerService = ViewManagerService.CreateInstance();
            CancelCommand = new RelayCommand(Cancel);
            ShowViewCommand = new RelayCommand<string>(ShowView);
            ShowLastViewCommand = new RelayCommand(ShowLastView);
        }

        private void ShowLastView()
        {
            ApplicationManager.ShowLastView();
        }

        public ViewManagerService ViewManagerService
        {
            get { return _viewManagerService; }
        }
        public ApplicationManager ApplicationManager
        {
            get { return ApplicationManager.Instance; }
        }




        public void Cancel()
        {
            ViewManagerService.Select().Remove(NavigateKey);
        }



        public NavigateKey NavigateKey
        {
            get { return _objNavigateKey; }
            set
            {
                _objNavigateKey = value;
                RaisePropertyChanged("NavigateKey");
            }
        }

        public virtual RelayCommand<string> ShowViewCommand { get; protected set; }

        public RelayCommand ShowLastViewCommand { get; protected set; }

        public RelayCommand CancelCommand { get; private set; }

        public virtual void ShowView(string view)
        {
            if (view.Equals("BackToHome"))
            {
                //Custom logic here
                //                ApplicationManager.ShowView(ViewTypes.RetailWelcomeScreen.ToString());
                return;
            }

            Messenger.Default.Send(new ShowViewMessage { ViewName = view });
        }
    }
}
