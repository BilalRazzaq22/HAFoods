using ERP.Common.NotifyProperty;
using ERP.Entities.DbContext;
using ERP.WpfClient.Controls.Helpers;
using ERP.WpfClient.View.Popups.Database;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ERP.WpfClient.ViewModel.Database
{
    public class DatabaseViewModel : ViewModelBase
    {
        #region Fields

        private string _filePath;

        #endregion

        #region Constructor
        public DatabaseViewModel()
        {
            DatabaseCommand = new RelayCommand<string>(ExecuteDatabaseCommand);
        }

        #endregion

        #region Properties

        public RelayCommand<string> DatabaseCommand { get; set; }

        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; RaisePropertyChanged("FilePath"); }
        }

        #endregion

        #region Methods

        

        public void ExecuteDatabaseCommand(string str)
        {
            switch (str)
            {
                case "DatabaseBackup":
                    BackupDatabase();
                    break;
                case "RestoreDatabase":
                    //ApplicationManager.Instance.ShowDialog("Restore Database", new RestoreDatabasePopup(this));
                    break;
                default:
                    break;
            }
        }

        

        public void BackupDatabase()
        {
            var bw = new BackgroundWorker();
            bw.DoWork += (sender, args) =>
            {
                try
                {
                    ApplicationManager.Instance.ShowBusyInidicator("Backup Database ...!");
                    var destination = @"C:\HAFood Database Backup";
                    var fileName = "HAFOODDB_" + DateTime.Now.ToString("dd-MM-yyyy") + "_" + DateTime.Now.ToString("hh-mm-ss") + ".bak";
                    using (var db = new HAFoodDbContext())
                    {
                        string backupQuery = @"BACKUP DATABASE ""{0}"" TO DISK = N'{1}' WITH FORMAT, MEDIANAME = 'SQLServerBackups', NAME = 'Full Backup of SQLTestDB'";
                        backupQuery = string.Format(backupQuery, "HAFoodDB", destination + @"\" + fileName);
                        db.Database.SqlQuery<object>(backupQuery).ToList().FirstOrDefault();
                    }

                    string fff = Path.GetFileName(destination + @"\" + fileName).ToString();

                    File.Copy(fff, @"https://drive.google.com/drive/folders/1FkbViuprU0xBxRFnDN0srzhgbPp-szYH", true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "HA Foods");
                }
            };

            bw.RunWorkerCompleted += async (sender, args) =>
            {
                ApplicationManager.Instance.HideBusyInidicator();
            };
            bw.RunWorkerAsync();
        }

        

        #endregion
    }
}
