using ERP.Common;
using ERP.Common.NotifyProperty;
using ERP.WpfClient.Messages.BusyIndicator;
using ERP.WpfClient.Messages.Popup;
using ERP.WpfClient.Messages.View;
using ERP.WpfClient.View.Popups;
using ERP.WpfClient.ViewModel.Popup;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;

namespace ERP.WpfClient.Controls.Helpers
{
    public class ApplicationManager : ViewModelBase
    {
        #region Fields
        private static ApplicationManager _instance;
        private static object padLock = new Object();
        //private readonly AppSettingsEntity _appSettings;
        //private readonly RadDesktopAlertManager alertManager;
        private string _machineMacAddress;
        private bool _isBusy;
        private static List<Stream> m_streams;
        private static int m_currentPageIndex = 0;
        public static bool IsCustomerBillOpen = false;
        public static bool IsSupplierBillOpen = false;
        public Stack<ViewTypes> NavigationHistory { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Singleton pattern implementation with thread safety
        /// </summary>
        static ApplicationManager()
        {
            if (_instance == null)
            {
                lock (padLock)
                {
                    if (_instance == null)
                    {
                        _instance = new ApplicationManager();
                    }
                }
            }
        }
        private ApplicationManager()
        {
            //alertManager = new RadDesktopAlertManager();
        }
        #endregion

        #region Properties

        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; RaisePropertyChanged("IsBusy"); }
        }
        public static ApplicationManager Instance
        {
            get { return _instance; }
        }
        public string MachineMacAddress
        {
            get { return _machineMacAddress; }
            set { _machineMacAddress = value; RaisePropertyChanged("MachineMacAddress"); }
        }
        //public AppSettingsEntity AppSettings
        //{
        //    get
        //    {
        //        var settings = _appSettings;

        //        if (settings == null)
        //        {
        //            //     settings = _appSettings = _applicationService.GetApplicationSettings(MachineMacAddress);
        //        }
        //        return settings;
        //    }
        //    set { RaisePropertyChanged("AppSettings"); }
        //}
        #endregion

        #region Methods

        public string Path()
        {
            return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
        }

        /// <summary>
        /// Creates the Login History of the Users
        /// </summary>
        /// <param name="type">Type of which login has been done</param>
        //public void CreateLoginHistory(LoginType type)
        //{
        //    try
        //    {
        //        //if (ActiveShift == null)
        //        //    ActiveShift = _shiftService.GetActiveShift(MachineMacAddress);

        //        //if (ActiveShift != null)
        //        //{
        //        //    if (type == LoginType.SignIn)
        //        //    {

        //        //        var loginHistory = new LoginHistory()
        //        //        {
        //        //            loggedIn_at = DateTime.Now,
        //        //            userName = CurrentUser.userName,
        //        //            user_id = CurrentUser.id,
        //        //            logType = (int)type,
        //        //            logTypeName = type.ToString(),
        //        //            isSyncronized = false,
        //        //            id = Guid.NewGuid(),
        //        //        };
        //        //        _loginHistoryRepository.AddAsyn(loginHistory);
        //        //        _loginHistoryRepository.SaveAsync();
        //        //    }
        //        //    else if (type == LoginType.SignOut)
        //        //    {
        //        //        LoginHistory latestLoginhistory = _loginHistoryRepository.GetAll().OrderByDescending(x => x.loggedIn_at).FirstOrDefault();
        //        //        var loginHistory = new LoginHistory()
        //        //        {
        //        //            loggedIn_at = latestLoginhistory.loggedIn_at,
        //        //            loggedOut_at = DateTime.Now,
        //        //            userName = latestLoginhistory.userName,
        //        //            user_id = latestLoginhistory.user_id,
        //        //            logType = (int)type,
        //        //            logTypeName = type.ToString(),
        //        //            isSyncronized = false,
        //        //            id = Guid.NewGuid(),
        //        //        };
        //        //        _loginHistoryRepository.AddAsyn(loginHistory);
        //        //        _loginHistoryRepository.SaveAsync();
        //        //    }
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error("Errror Saving Shift Login History " + ex.Message, ex);
        //    }

        //}



        /// <summary>
        /// Exists from the Application
        /// </summary>
        //public void ExitApplication()
        //{
        //    ShowConfirmDialog(MessageResources.ConfirmatoryMessage, () =>
        //    {
        //        CreateLoginHistory(LoginType.SignOut);
        //        Application.Current.Shutdown();
        //    });

        //}

        /// <summary>
        /// Background worker to handle logic without freezing UI
        /// </summary>
        /// <param name="execute">action to be done</param>
        /// <param name="completionAction">action to be done after completion of first action</param>
        /// <param name="showBusyIndicator">show busy indicator</param>
        /// <param name="busyIndicatorMsg">message to be displayed on busy indicator</param>
        public void ExecuteAsync(Action execute, Action completionAction, bool showBusyIndicator = false,
     string busyIndicatorMsg = "")
        {
            var bw = new BackgroundWorker();
            bw.DoWork += (sender, args) => execute();
            bw.RunWorkerCompleted += (sender, args) =>
            {
                if (showBusyIndicator)
                    HideBusyInidicator();
                if (completionAction != null)
                {
                    completionAction();
                }
            };
            if (showBusyIndicator)
                ShowBusyInidicator(busyIndicatorMsg);
            bw.RunWorkerAsync();
        }

        public void ShowMessageBox(string message, string okButtonText = "OKAY")
        {
            Messenger.Default.Send<PopupDialogMessage>(new PopupDialogMessage()
            {
                Show = true,
                Title = "",
                ControlToDisplay = new MessageBoxPopup(message, okButtonText),
                ShowCloseButton = false,
                UseMessageBox = true,
            });
        }

        public void ShowDialog(string title, FrameworkElement element, bool showCloseButton = true,
           bool useMessageBox = false)
        {
            Messenger.Default.Send<PopupDialogMessage>(new PopupDialogMessage()
            {
                Show = true,
                Title = title,
                ControlToDisplay = element,
                ShowCloseButton = showCloseButton,
                UseMessageBox = useMessageBox
            });
        }


        public void HideDialog(bool useSecondDialog = false, bool hideOnly = false)
        {
            Messenger.Default.Send<PopupDialogMessage>(new PopupDialogMessage()
            {
                Show = false,
                UseSecondPopup = useSecondDialog,
                HideOnly = hideOnly
            });
        }

        public void ShowConfirmDialog(string message, Action successAction, Action cancelAction = null,
   bool useYesNo = false)
        {
            Messenger.Default.Send<PopupDialogMessage>(new PopupDialogMessage()
            {
                Show = true,
                Title = "",
                ControlToDisplay =
                    new ConfirmationPopup(new ConfirmPopupViewModel
                    {
                        Message = message,
                        YesAction = successAction,
                        CancelAction = cancelAction,
                        UseYesNo = useYesNo
                    }),
                ShowCloseButton = false,
                UseMessageBox = true,
            });
        }
        public void ExecuteOnMainThread(Action action)
        {
            Application.Current.Dispatcher.Invoke(action);
        }

        internal void ShowLastView(Action executeAction = null, object primaryKey = null)
        {
            if (NavigationHistory.Count > 0)
            {
                var viewtoDisplay = NavigationHistory.Pop();
                Messenger.Default.Send<ShowViewMessage>(new ShowViewMessage
                {
                    ViewName = viewtoDisplay.ToString(),
                    ActionToExecute = executeAction,
                    PrimaryKey = primaryKey,
                });
                NavigationHistory.Clear();
            }
        }

        public void ShowBusyInidicator(string message = null)
        {
            SendBusyIndicatorMessage(true, busyNoticeDetails: message);
        }

        internal void HideMessageBox()
        {
            Messenger.Default.Send<PopupDialogMessage>(new PopupDialogMessage() { Show = false, UseMessageBox = true });
        }
        public void HideBusyInidicator()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                IsBusy = false;
                SendBusyIndicatorMessage(false, false, false, "");
            }));


        }

        private void SendBusyIndicatorMessage(bool isBusy = false, bool isBusyNotice = false, bool isErrorNotice = false,
           string busyNoticeDetails = null)
        {
            var busyMessage = new BusyIndicatorMessage();
            busyMessage.IsBusy = isBusy;
            busyMessage.IsBusyNotice = isBusyNotice;
            busyMessage.IsErrorNotice = isErrorNotice;
            if (!String.IsNullOrEmpty(busyNoticeDetails))
            {
                busyMessage.BusyNoticeDetails = busyNoticeDetails;
            }
            IsBusy = true;
            ExecuteOnMainThread(() => Messenger.Default.Send(busyMessage));
        }

        public void PrintReport(object query, string reportName, string dataset, string file)
        {
            //string path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            //string fullpath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location).Remove(path.Length - 10) + reportName + ".rdlc";

            string fullpath = @"C:\Program Files (x86)\HAFoods Setup\HA Foods" + reportName + ".rdlc";
            //string fullpath = path + @"\Reports\" + reportName + ".rdlc";

            string deviceInfo =
             @"<DeviceInfo>
                        <OutputFormat>EMF</OutputFormat>
                        <MarginTop>0in</MarginTop>
                        <MarginLeft>0.1in</MarginLeft>
                        <MarginRight>0.1in</MarginRight>
                        <MarginBottom>0in</MarginBottom>
                    </DeviceInfo>";
            string[] streamIds;
            Warning[] warnings;

            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.LocalReport.ReportPath = fullpath;
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource(dataset, query));
            var bytes = reportViewer.LocalReport.Render("PDF", deviceInfo, out mimeType, out encoding, out extension, out streamIds, out warnings);
            string fileName = @"D:\" + file + ".pdf";
            File.WriteAllBytes(fileName, bytes);
            Process.Start(fileName);
        }

        //public bool IsUserLoggedIn()
        //{
        //    return User != null;
        //}

        /// <summary>
        /// This method uploads the excel data after showing alert and shows completion box once the method has been completed
        /// </summary>
        //public void UploadExcelData()
        //{
        //    bool result = false;
        //    var alert = new RadDesktopAlert
        //    {
        //        Header = "Seeding Excel Data",
        //        Content = "You will be notified once data has seeded!",
        //        ShowDuration = 5000,
        //    };
        //    alertManager.ShowAlert(alert);
        //    ExecuteAsync(() =>
        //    {
        //        result = RunSeeder.SeedData.Seeds();
        //    }, () =>
        //    {
        //        if (result)
        //        {
        //            MessageBox.Show(MessageResources.SeedingSuccess, ApplicationConstants.ApplicationName);
        //        }
        //        else
        //        {
        //            MessageBox.Show(ExceptionResources.SeedingFailed, ApplicationConstants.ApplicationName);
        //        }
        //    }
        //    , false, "");
        //}

        //internal void PrintToPrinter(LocalReport report)
        //{
        //     Export(report);
        //}

        //public static void Export(LocalReport report, bool print = true)
        //{
        //    string deviceInfo =
        //     @"<DeviceInfo>
        //        <OutputFormat>EMF</OutputFormat>
        //        <MarginTop>0in</MarginTop>
        //        <MarginLeft>0.1in</MarginLeft>
        //        <MarginRight>0.1in</MarginRight>
        //        <MarginBottom>0in</MarginBottom>
        //    </DeviceInfo>";
        //    Warning[] warnings;
        //    m_streams = new List<Stream>();
        //    report.Render("Image", deviceInfo, CreateStream, out warnings);
        //    foreach (Stream stream in m_streams)
        //        stream.Position = 0;

        //    if (print)
        //    {
        //        Print();
        //    }
        //}

        //public static void Print()
        //{
        //    if (m_streams == null || m_streams.Count == 0)
        //        throw new Exception("Error: no stream to print.");
        //    using (PrintDocument printDoc = new PrintDocument())
        //    {
        //        PrintController printController = new StandardPrintController();
        //        printDoc.PrintController = printController;
        //        if (!printDoc.PrinterSettings.IsValid)
        //        {
        //            throw new Exception("Error: cannot find the default printer.");
        //        }
        //        else
        //        {
        //            printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
        //            m_currentPageIndex = 0;
        //            printDoc.Print();
        //        }
        //    }
        //}

        //public static Stream CreateStream(string name, string fileNameExtension, Encoding encoding, string mimeType, bool willSeek)
        //{
        //    Stream stream = new MemoryStream();
        //    m_streams.Add(stream);
        //    return stream;
        //}

        //public static void PrintPage(object sender, PrintPageEventArgs ev)
        //{
        //    Metafile pageImage = new
        //       Metafile(m_streams[m_currentPageIndex]);

        //    // Adjust rectangular area with printer margins.
        //    Rectangle adjustedRect = new Rectangle(
        //        ev.PageBounds.Left - (int)ev.PageSettings.HardMarginX,
        //        ev.PageBounds.Top - (int)ev.PageSettings.HardMarginY,
        //        ev.PageBounds.Width,
        //        ev.PageBounds.Height);

        //    // Draw a white background for the report
        //    ev.Graphics.FillRectangle(System.Drawing.Brushes.White,
        //                              adjustedRect);

        //    // Draw the report content
        //    ev.Graphics.DrawImage(pageImage, adjustedRect);

        //    // Prepare for the next page. Make sure we haven't hit the end.
        //    m_currentPageIndex++;
        //    ev.HasMorePages = (m_currentPageIndex < m_streams.Count);
        //}
    }



    #endregion

}

