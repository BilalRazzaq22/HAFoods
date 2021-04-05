using ERP.Common;
using ERP.Common.NotifyProperty;
using ERP.Repository.Generic;
using ERP.WpfClient.Controls.Helpers;
using ERP.WpfClient.Mapper;
using ERP.WpfClient.Model.Transaction;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ERP.WpfClient.ViewModel.Transaction
{
    public class CurrentTransactionViewModel : ViewModelBase, INotifyOnBringIntoView
    {
        #region Fields

        private readonly IGenericRepository<Entities.DBModel.CurrentTransaction> _currentTransactionRepository;
        private CurrentTransactionModel _currentTransactionModel;
        private ObservableCollection<CurrentTransactionModel> _currentTransactionList;
        private string _currentTransactionButton;
        private string _currentTransactionParameter;

        #endregion

        #region Ctor

        public CurrentTransactionViewModel()
        {
            CurrentTransactionCommands = new RelayCommand<object>(ExecuteCurrentTransactionCommand);
            DeleteCurrentTransactionCommand = new RelayCommand<object>(ExecuteDeleteCurrentTransactionCommand);
            //this.CurrentTransactionCommands = new CustomerCommand(this);
            _currentTransactionRepository = App.Resolve<IGenericRepository<Entities.DBModel.CurrentTransaction>>();
            CurrentTransactionModel = new CurrentTransactionModel();
            CurrentTransactionList = new ObservableCollection<CurrentTransactionModel>();
            CurrentTransactionButton = "Save";
            CurrentTransactionParameter = "SaveCurrentTransaction";
        }

        #endregion

        #region Properties
        public RelayCommand<object> CurrentTransactionCommands { get; set; }
        public RelayCommand<object> DeleteCurrentTransactionCommand { get; set; }

        //public CustomerCommand CurrentTransactionCommands { get; set; }

        public CurrentTransactionModel CurrentTransactionModel
        {
            get { return _currentTransactionModel; }
            set { _currentTransactionModel = value; RaisePropertyChanged("CurrentTransactionModel"); }
        }

        public ObservableCollection<CurrentTransactionModel> CurrentTransactionList
        {
            get { return _currentTransactionList; }
            set { _currentTransactionList = value; RaisePropertyChanged("CurrentTransactionList"); }
        }

        public string CurrentTransactionParameter
        {
            get { return _currentTransactionParameter; }
            set { _currentTransactionParameter = value; RaisePropertyChanged("CurrentTransactionParameter"); }
        }

        public string CurrentTransactionButton
        {
            get { return _currentTransactionButton; }
            set { _currentTransactionButton = value; RaisePropertyChanged("CurrentTransactionButton"); }
        }

        #endregion

        #region Methods

        private void ExecuteCurrentTransactionCommand(object str)
        {
            if (str as string == "SaveCurrentTransaction")
            {
                SaveCurrentTransaction();
            }
            else if (str as string == "UpdateCurrentTransaction")
            {
                UpdateCurrentTransaction();
            }
            else if (str as string == "Clear")
            {
                Reset();
            }
            else if (str != null)
            {
                EditStock(str as CurrentTransactionModel);
            }
        }

        private void Reset()
        {
            CurrentTransactionModel = new CurrentTransactionModel();
            CurrentTransactionButton = "Save";
            CurrentTransactionParameter = "SaveCurrentTransaction";
        }

        private void ExecuteDeleteCurrentTransactionCommand(object obj)
        {
            if (obj != null)
            {
                ApplicationManager.Instance.ShowConfirmDialog("Are you sure you want to Delete the delete?", () =>
                {
                    DeleteStock(obj as CurrentTransactionModel);
                    ApplicationManager.Instance.HideDialog();
                }, () => ApplicationManager.Instance.HideMessageBox(), useYesNo: true);
            }
        }

        public void SaveCurrentTransaction()
        {
            var model = _currentTransactionRepository.Add(MapperProfile.iMapper.Map<Entities.DBModel.CurrentTransaction>(CurrentTransactionModel));
            CurrentTransactionModel.Id = model.Id;
            CurrentTransactionList.Add(CurrentTransactionModel);
            Reset();
        }

        public void EditStock(CurrentTransactionModel currentTransactionModel)
        {
            CurrentTransactionButton = "Update";
            CurrentTransactionParameter = "UpdateCurrentTransaction";
            CurrentTransactionModel.Id = currentTransactionModel.Id;
            //CurrentTransactionModel.ItemName = currentTransactionModel.ItemName;
            //CurrentTransactionModel.UrduName = currentTransactionModel.UrduName;
            //CurrentTransactionModel.BuyPrice = currentTransactionModel.BuyPrice;
            //CurrentTransactionModel.SalePrice = currentTransactionModel.SalePrice;
            //CurrentTransactionModel.Quantity = currentTransactionModel.Quantity;
            //CurrentTransactionModel.Category = currentTransactionModel.Category;
            //CurrentTransactionModel.Packing = currentTransactionModel.Packing;
            //CurrentTransactionModel.Remarks = currentTransactionModel.Remarks;

        }

        public void UpdateCurrentTransaction()
        {
            _currentTransactionRepository.Update(MapperProfile.iMapper.Map<Entities.DBModel.CurrentTransaction>(CurrentTransactionModel), CurrentTransactionModel.Id);
            Reset();
        }

        public void DeleteStock(CurrentTransactionModel currentTransactionModel)
        {
            _currentTransactionRepository.Delete(currentTransactionModel.Id);
            CurrentTransactionList.Remove(currentTransactionModel);
        }

        private void Init()
        {
            var bw = new BackgroundWorker();
            List<Entities.DBModel.CurrentTransaction> currentTransaction = null;
            bw.DoWork += (sender, args) =>
            {
                try
                {
                    ApplicationManager.Instance.ShowBusyInidicator("Loading Data... !");
                    currentTransaction = _currentTransactionRepository.Get();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Post Error\nMessage: " + ex.Message, "HA Foods");
                }
            };

            bw.RunWorkerCompleted += async (sender, args) =>
            {
                await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    CurrentTransactionList = MapperProfile.iMapper.Map<ObservableCollection<CurrentTransactionModel>>(currentTransaction);
                }));
                ApplicationManager.Instance.HideBusyInidicator();
            };

            bw.RunWorkerAsync();
        }

        public void OnBringIntoView()
        {
            Init();
        }

        #endregion
    }
}
