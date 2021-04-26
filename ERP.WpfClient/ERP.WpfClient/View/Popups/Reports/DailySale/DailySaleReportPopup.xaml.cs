﻿using ERP.WpfClient.ViewModel.Popup.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ERP.WpfClient.View.Popups.Reports.DailySale
{
    /// <summary>
    /// Interaction logic for DailySaleReportPopup.xaml
    /// </summary>
    public partial class DailySaleReportPopup : UserControl
    {
        public DailySaleReportPopup()
        {
            InitializeComponent();
        }

        public DailySaleReportPopup(AllReportsViewModel allReportsViewModel)
        {
            InitializeComponent();
            DataContext = allReportsViewModel;
        }
    }
}
