using ERP.Common;
using ERP.Common.NotifyProperty;
using ERP.Entities.DbContext;
using ERP.Repository.Generic;
using ERP.WpfClient.Controls.Helpers;
using ERP.WpfClient.Mapper;
using ERP.WpfClient.Model.Stock;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ERP.WpfClient.ViewModel.Stock
{
    public class StockViewModel : ViewModelBase, INotifyOnBringIntoView
    {
        #region Fields

        private readonly IGenericRepository<Entities.DBModel.Stocks.Stock> _stockRepository;
        private StockModel _stockModel;
        private ObservableCollection<StockModel> _stockList;
        private string _stockButton;
        private string _stockParameter;
        private string _previousQuantity;
        private string _newQuantity;
        private string _isUpdateStock;

        #endregion

        #region Ctor

        public StockViewModel()
        {
            StockCommands = new RelayCommand<object>(ExecuteStockCommand);
            DeleteStockCommand = new RelayCommand<object>(ExecuteDeleteStockCommand);
            //this.StockCommands = new CustomerCommand(this);
            _stockRepository = new GenericRepository<Entities.DBModel.Stocks.Stock>(new HAFoodDbContext());
            StockModel = new StockModel();
            StockList = new ObservableCollection<StockModel>();
            StockButton = "Save";
            StockParameter = "SaveStock";
        }

        #endregion

        #region Properties
        public RelayCommand<object> StockCommands { get; set; }
        public RelayCommand<object> DeleteStockCommand { get; set; }

        //public CustomerCommand StockCommands { get; set; }

        public StockModel StockModel
        {
            get { return _stockModel; }
            set { _stockModel = value; RaisePropertyChanged("StockModel"); }
        }

        public ObservableCollection<StockModel> StockList
        {
            get { return _stockList; }
            set { _stockList = value; RaisePropertyChanged("StockList"); }
        }

        public string StockParameter
        {
            get { return _stockParameter; }
            set { _stockParameter = value; RaisePropertyChanged("StockParameter"); }
        }

        public string StockButton
        {
            get { return _stockButton; }
            set { _stockButton = value; RaisePropertyChanged("StockButton"); }
        }

        public string PreviousQuantity
        {
            get { return _previousQuantity; }
            set { _previousQuantity = value; RaisePropertyChanged("PreviousQuantity"); }
        }

        public string NewQuantity
        {
            get { return _newQuantity; }
            set { _newQuantity = value; RaisePropertyChanged("NewQuantity"); }
        }

        public string IsUpdateStock
        {
            get { return _isUpdateStock; }
            set { _isUpdateStock = value; RaisePropertyChanged("IsUpdateStock"); }
        }

        private string _isEnabled;

        public string IsEnabled
        {
            get { return _isEnabled; }
            set { _isEnabled = value; RaisePropertyChanged("IsEnabled"); }
        }

        #endregion

        #region Methods

        private void ExecuteStockCommand(object str)
        {
            if (str as string == "SaveStock")
            {
                SaveStock();
            }
            else if (str as string == "UpdateStock")
            {
                UpdateStock();
            }
            else if (str as string == "Clear")
            {
                Reset();
            }
            else if (str != null)
            {
                EditStock(str as StockModel);
            }
        }

        private void Reset()
        {
            StockModel = new StockModel();
            StockButton = "Save";
            StockParameter = "SaveStock";
            IsUpdateStock = Visibility.Collapsed.ToString();
            PreviousQuantity = "Quantity:";
            IsEnabled = "True";
        }

        private void ExecuteDeleteStockCommand(object obj)
        {
            if (obj != null)
            {
                ApplicationManager.Instance.ShowConfirmDialog("Are you sure you want to Delete the stock?", () =>
                {
                    DeleteStock(obj as StockModel);
                    ApplicationManager.Instance.HideDialog();
                }, () => ApplicationManager.Instance.HideMessageBox(), useYesNo: true);
            }
        }

        public void SaveStock()
        {
            //StockModel.CurrentQuantity = StockModel.NewQuantity;
            var model = _stockRepository.Add(MapperProfile.iMapper.Map<Entities.DBModel.Stocks.Stock>(StockModel));
            StockModel.Id = model.Id;
            StockList.Add(StockModel);
            Reset();
        }

        public void EditStock(StockModel stockModel)
        {
            IsUpdateStock = Visibility.Visible.ToString();
            PreviousQuantity = "Previous Quantity:";
            NewQuantity = "New Quantity:";
            IsEnabled = "False";
            StockButton = "Update";
            StockParameter = "UpdateStock";
            StockModel.Id = stockModel.Id;
            StockModel.ItemName = stockModel.ItemName;
            StockModel.UrduName = stockModel.UrduName;
            StockModel.BuyPrice = stockModel.BuyPrice;
            StockModel.SalePrice = stockModel.SalePrice;
            StockModel.Quantity = stockModel.Quantity;
            StockModel.NewQuantity = 0;
            StockModel.Category = stockModel.Category;
            StockModel.Packing = stockModel.Packing;
            StockModel.Remarks = stockModel.Remarks;
        }

        public void UpdateStock()
        {
            StockModel.Quantity = StockModel.NewQuantity + StockModel.Quantity;
            _stockRepository.Update(MapperProfile.iMapper.Map<Entities.DBModel.Stocks.Stock>(StockModel), StockModel.Id);
            Reset();
        }

        public void DeleteStock(StockModel stockModel)
        {
            _stockRepository.Delete(stockModel.Id);
            StockList.Remove(stockModel);
        }

        private void Init()
        {
            var bw = new BackgroundWorker();
            List<Entities.DBModel.Stocks.Stock> stock = null;
            bw.DoWork += (sender, args) =>
            {
                try
                {
                    ApplicationManager.Instance.ShowBusyInidicator("Loading Data... !");
                    stock = _stockRepository.Get();
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
                    StockList = MapperProfile.iMapper.Map<ObservableCollection<StockModel>>(stock);
                }));
                ApplicationManager.Instance.HideBusyInidicator();
            };

            bw.RunWorkerAsync();
        }

        public void OnBringIntoView()
        {
            Init();
            Reset();
        }

        #endregion
    }
}
