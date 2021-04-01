using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ERP.WpfClient.View.Search
{
    /// <summary>
    /// Interaction logic for SearchBox.xaml
    /// </summary>
    public partial class SearchBox : UserControl
    {
        List<PropertyInfo> _propertyInfo = new List<PropertyInfo>();
        Timer _timer = new Timer();
        private DataGrid grid;
        string prevKey = string.Empty;
        private string _selectedcol;
        List<string> list = new List<string>();

        public string SelectedCol
        {
            get { return _selectedcol; }
            set { _selectedcol = value; }
        }

        public DataGrid Grid
        {
            get { return grid; }
            set { grid = value; }
        }
        public SearchBox()
        {
            InitializeComponent();
            _timer.Interval = 100;//TimeSpan.FromMilliseconds(100);
            //_timer.Tick += _timer_Tick;
            _timer.Elapsed += _timer_Elapsed;
            prevKey = _txtSearch.Text;
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();
            if (View != null && _propertyInfo.Count > 0)
            {
                Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    View.Filter = null;
                    View.Filter = (object item) =>
                    {

                        // get search text
                        var srch = _txtSearch.Text;

                        // no text? show all items
                        if (string.IsNullOrEmpty(srch))
                        {
                            return true;
                        }
                        // show items that contain the text in any of the specified properties
                        var collection = Grid.ItemsSource;
                        Type collectionType = collection.GetType();
                        Type itemType = collectionType.GetGenericArguments().Single();

                        foreach (PropertyInfo pi in _propertyInfo)
                        {
                            var value = Convert.ToString(pi.GetValue(item, null));

                            if (value != null && value.IndexOf(srch, StringComparison.OrdinalIgnoreCase) > -1 && srch.IndexOf(value, StringComparison.OrdinalIgnoreCase) < 1
                            ) //1
                            {
                                return true;
                            }
                            else return false;

                        }
                        return false;
                    };
                }, System.Windows.Threading.DispatcherPriority.Background);
            }
        }

        public ICollectionView View { get; set; }
        public List<PropertyInfo> FilterProperties
        {
            get { return _propertyInfo; }
        }
        void _txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            _propertyInfo.Clear();
            if (prevKey == "Search" && string.IsNullOrEmpty(_txtSearch.Text)) return;
            //if (_imgClear != null)
            //{
            // update clear button visibility
            //_imgClear.Visibility = string.IsNullOrEmpty(_txtSearch.Text)
            //? Visibility.Collapsed
            //: Visibility.Visible;
            if (Grid != null)
            {
                var collection = Grid.ItemsSource;
                Type collectionType = collection.GetType();

                Type itemType = collectionType.GetGenericArguments().Single();
                View = CollectionViewSource.GetDefaultView(collection);
                if (View.SourceCollection != null)
                {
                    var props = FilterProperties;
                    props.Add(itemType.GetProperty(SelectedCol));
                    prevKey = _txtSearch.Text;
                    // start ticking
                    _timer.Stop();
                    _timer.Start();
                }
            }
            //}
        }
        void _imgClear_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _propertyInfo.Clear();
            _txtSearch.Text = string.Empty;
        }

        //void _timer_Tick(object sender, EventArgs e)
        //{
        //    _timer.Stop();
        //    if (View != null && _propertyInfo.Count > 0)
        //    {
        //        View.Filter = null;
        //        View.Filter = (object item) =>
        //        {
        //            // get search text
        //            var srch = _txtSearch.Text;

        //            // no text? show all items
        //            if (string.IsNullOrEmpty(srch))
        //            {
        //                if (PMCode.StartsWith(((Position)item).PortfolioCode, StringComparison.OrdinalIgnoreCase) == true
        //                //if (PMCode.IndexOf(((Position)item).PortfolioCode, StringComparison.OrdinalIgnoreCase) > -1
        //                    || (((Position)item).qty == 0 && ((Position)item).DayPnL == 0 && IsClose == false) //1
        //                   )
        //                {
        //                    return false;
        //                }
        //                else
        //                {
        //                    return true;
        //                }
        //            }

        //            // show items that contain the text in any of the specified properties
        //            foreach (PropertyInfo pi in _propertyInfo)
        //            {
        //                var value = pi.GetValue(item, null) as string;

        //                //if (value != null && value.IndexOf(srch, StringComparison.OrdinalIgnoreCase) > -1 &&
        //                //    PMCode.IndexOf(((Position)item).PortfolioCode, StringComparison.OrdinalIgnoreCase) < 0
        //                //    && (((Position)item).qty != 0 || ((Position)item).DayPnL != 0 || IsClose == true) //1
        //                //   )

        //                if (value != null && value.StartsWith(srch, StringComparison.OrdinalIgnoreCase) == true &&
        //                   PMCode.IndexOf(((Position)item).PortfolioCode, StringComparison.OrdinalIgnoreCase) < 0
        //                   && (((Position)item).qty != 0 || ((Position)item).DayPnL != 0 || IsClose == true) //1
        //                  )
        //                {
        //                    return true;
        //                }
        //            }
        //            // exclude this item...
        //            return false;
        //        };
        //    }

        //}

        private void _txtSearch_MouseEnter(object sender, MouseEventArgs e)
        {
            if (_txtSearch.Text == "Search")
            {
                //_propertyInfo.Clear();
                _txtSearch.Text = string.Empty;
            }
        }

        public void PopulateComboBox()
        {
            if (Grid == null) return;
            if (Grid.ItemsSource == null) return;

            //_filterSearch.Items.Clear();
            //ComboBoxSymbol.Clear();
            var lstcol = Grid.Columns.Where(x => x.Visibility == Visibility.Visible).OrderBy(x => x.DisplayIndex).ToList();
            lstcol = lstcol.Where(x => !string.IsNullOrEmpty(x.Header.ToString().Trim())).ToList();
            //var collection = Grid.ItemsSource;

            //Type collectionType = collection.GetType();
            //Type itemType = collectionType.GetGenericArguments().Single();


            foreach (var col in lstcol)
            {
                string HeaderName = col.SortMemberPath.ToString();
                if (!string.IsNullOrEmpty(HeaderName))
                {
                    list.Add(HeaderName);
                }
            }

            SelectedCol = list.FirstOrDefault();
            //_filterSearch.SelectedItem = "Ticker";
            //_imgClear.Visibility = string.IsNullOrEmpty(_txtSearch.Text)
            //        ? Visibility.Collapsed
            //        : Visibility.Visible;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            PopulateComboBox();
        }
    }
}
