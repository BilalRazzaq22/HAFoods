using Autofac;
using ERP.Entities.DbContext;
using ERP.Entities.DBModel.AppSettings;
using ERP.Entities.DBModel.Payments;
using ERP.Entities.DBModel.Users;
using ERP.Repository.Generic;
using ERP.WpfClient.Controls.Helpers;
using ERP.WpfClient.Mapper;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Timers;
using System.Windows;

namespace ERP.WpfClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IContainer Container { get; private set; }
        private bool IsCreateSetup = false;
        private IGenericRepository<AppSetting> _appSettingRepository;
        private IGenericRepository<User> _userRepository;
        private IGenericRepository<Payment> _paymentRepository;
        Timer _timer = new Timer();

        protected override void OnStartup(StartupEventArgs e)
        {
            //bool createdNew;
            //System.Threading.Mutex m_Mutex = new System.Threading.Mutex(true, "ERP.WpfClient", out createdNew);
            //if (!(createdNew))
            //{
            //    if (MessageBoxResult.OK == MessageBox.Show("HA Foods is already open on your computer.", "HA Foods"))
            //    {
            //        //Application.Current.Shutdown();
            //    }
            //}

            //IsCreateSetup = true;
            try
            {
                _timer.Interval = 60 * 60 * 1000; // One Hour
                                                  //_timer.Tick += _timer_Tick;
                _timer.Elapsed += _timer_Elapsed;
                base.OnStartup(e);
                MapperProfile.InitializeMappers();
                InitializeServices();
            }
            catch (Exception ex)
            {
                MessageBox.Show("On StartUp" + ex.Message);
            }
        }

        private void InitializeServices()
        {
            try
            {
                var builder = new ContainerBuilder();
                builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>));
                Container = builder.Build();
                _appSettingRepository = Resolve<IGenericRepository<AppSetting>>();
                _userRepository = Resolve<IGenericRepository<User>>();
                _paymentRepository = Resolve<IGenericRepository<Payment>>();
                InitializeDB();
            }
            catch (Exception ex)
            {
                MessageBox.Show("InitializeServices" + ex.Message);
            }
        }

        private void InitializeDB()
        {
            try
            {
                //AppSetting appSetting = _appSettingRepository.Get().FirstOrDefault(x => x.IsDBCreated == true);

                //if (appSetting == null)
                //{
                if (!Directory.Exists(Path.Combine(@"C:\", "HAFood Database Backup")))
                {
                    Directory.CreateDirectory(Path.Combine(@"C:\", "HAFood Database Backup"));
                }

                if (!Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "HAFood")))
                {
                    Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "HAFood"));
                }

                var spFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "HAFood");
                if (!File.Exists(Path.Combine(spFolderPath, "HAFoodDB.mdf")))
                {
                    try
                    {
                        //CheckReset();
                        //string path = @"C:\Users\bilal\projects\HAFood DB Backup";
                        string path = @"C:\Program Files (x86)\HAFoods Setup\HA Foods";
                        string filePath = @"Data Source = (LocalDB)\MSSQLLocalDB; Initial Catalog=HAFoodDB; AttachDbFilename = " + path + @"\HAFoodDB.mdf; Integrated Security = True;";
                        HAFoodDbContext HaFoodDbContext = new HAFoodDbContext();
                        HaFoodDbContext.Init();
                        RunSP();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("App.xaml.cs " + ex.Message);
                    }

                    AppSetting appSetting = _appSettingRepository.Get().FirstOrDefault();
                    if (appSetting == null)
                    {
                        appSetting = new AppSetting
                        {
                            AppVersion = "1.0.0.0",
                            IsDBCreated = true,
                            AppStartDate = DateTime.Now,
                            AppEndDate = DateTime.Now.AddDays(15)
                        };
                        _appSettingRepository.Add(appSetting);
                    }

                    User user = new User
                    {
                        Username = "admin",
                        Email = "admin@hafoods.com",
                        Password = "admin123",
                        UserGroup = "Admin"
                    };
                    _userRepository.Add(user);

                    Payment payment = new Payment();
                    payment.PaymentType = "Cash";

                    _paymentRepository.Add(payment);

                    payment = new Payment();
                    payment.PaymentType = "Credit";

                    _paymentRepository.Add(payment);
                }

                //    _appSettingRepository.Update(new AppSetting() { IsDBCreated = true }, 1);
                //}


                //var spFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "HAFood");
                //if (!File.Exists(Path.Combine(spFolderPath, "HAFoodDB.mdf")))
                //{
                //    Console.WriteLine("{0} Initializing DB", DateTime.Now);
                //    string filePath = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\HAFood\HAFoodDB.mdf; Integrated Security = True;";
                //    HAFoodDbContext HaFoodDbContext = new HAFoodDbContext();
                //    HaFoodDbContext.Init();

                //    var filePathSetting = Path.Combine(spFolderPath, "Settings.dat");

                //    Console.WriteLine("{0} DB Initialized", DateTime.Now);
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        static void CheckReset()
        {
            try
            {
                if (!Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "HAFood")))
                {
                    Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "HAFood"));
                }

                var spFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "HAFood");
                //var filePath = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "HAFood"), "construct.json");
                if (Directory.Exists(spFolderPath))
                {
                    //if (File.ReadAllText(filePath).Equals("{'Reset':'True'}"))
                    //{
                    File.Delete(Path.Combine(spFolderPath, "HAFoodDB.mdf"));
                    File.Delete(Path.Combine(spFolderPath, "HAFoodDB_log.ldf"));
                    //string path = ApplicationManager.Instance.Path();
                    readfiles();
                    //File.Copy(Path.Combine(spFolderPath, "HAFoodDB.mdf"));
                    //File.Copy(Path.Combine(spFolderPath, "HAFoodDB_log.ldf"));
                    //File.Delete(spFolderPath);
                    //}
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        static void readfiles()
        {
            var spFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "HAFood");

            //string path = ApplicationManager.Instance.Path();

            //string path = @"C:\Program Files (x86)\HAFoods Setup\HA Foods\Database";
            string path = @"C:\Users\bilal\projects\HAFoods\ERP.WpfClient\ERP.WpfClient\bin\Debug\Database";

            //string path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);HAFoods Setup\HA Foods

            //string fullpath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location).Remove(path.Length - 10) + @"\Database";

            string[] filePaths = Directory.GetFiles(path);
            foreach (var filename in filePaths)
            {
                string file = Path.GetFileName(filename).ToString();
                string dest = Path.Combine(spFolderPath, file);
                //Do your job with "file"  
                //string str = file.ToString().Replace(spFolderPath,path);
                File.Copy(filename, dest, true);
            }
        }

        private void ExecuteSQLStmt(string sql)
        {
            string constr = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\HAFood\HAFoodDB.mdf; Integrated Security = True;";

            //if (conn.State == ConnectionState.Open)
            //    conn.Close();
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            //ConnectionString = "Integrated Security=SSPI;" +
            //"Initial Catalog=mydb;" +
            //"Data Source=localhost;";
            //conn.ConnectionString = ConnectionString;
            //conn.Open();
            SqlCommand cmd = new SqlCommand(sql, con);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ae)
            {
                //MessageBox.Show(ae.Message.ToString());
            }
        }

        private void RunSP()
        {
            string[] filePaths;
            string startupPath = Environment.CurrentDirectory;
            if (Directory.Exists(System.IO.Path.Combine(startupPath, @"DatabaseScripts")))
            {
                filePaths = Directory.GetFiles(System.IO.Path.Combine(startupPath, @"DatabaseScripts"));
            }

            string path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            //string fullpath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location).Remove(path.Length - 10);
            string fullpath = @"C:\Program Files (x86)\HAFoods Setup\HA Foods";
            filePaths = Directory.GetFiles(System.IO.Path.Combine(fullpath, @"DatabaseScripts"));
            if (filePaths != null)
            {
                if (filePaths.Length > 0)
                {
                    foreach (string p in filePaths)
                    {
                        //Console.WriteLine(p);
                        //p.Replace("FridayRetailDb", databaseName);
                        string qp = File.ReadAllText(p);//.Replace("FridayRetailDb", databaseName);
                        //migrationBuilder.Sql(qp);
                        ExecuteSQLStmt(qp);
                    }
                }
            }
        }

        public void BackupDatabase()
        {
            var bw = new System.ComponentModel.BackgroundWorker();
            bw.DoWork += (sender, args) =>
            {
                try
                {
                    //ApplicationManager.Instance.ShowBusyInidicator("Backup Database ...!");
                    var destination = @"C:\HAFood Database Backup";
                    using (var db = new HAFoodDbContext())
                    {
                        string backupQuery = @"BACKUP DATABASE ""{0}"" TO DISK = N'{1}' WITH FORMAT, MEDIANAME = 'SQLServerBackups', NAME = 'Full Backup of SQLTestDB'";
                        backupQuery = string.Format(backupQuery, "HAFoodDB", destination + @"\HAFOODDB_" + DateTime.Now.ToString("dd-MM-yyyy") + "_" + DateTime.Now.ToString("hh-mm-ss") + ".bak");
                        db.Database.SqlQuery<object>(backupQuery).ToList().FirstOrDefault();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "HA Foods");
                }
            };

            bw.RunWorkerCompleted += async (sender, args) =>
            {
                //ApplicationManager.Instance.HideBusyInidicator();
            };
            bw.RunWorkerAsync();
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();
            BackupDatabase();
        }

        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }
    }
}
