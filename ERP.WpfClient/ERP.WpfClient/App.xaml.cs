using Autofac;
using ERP.Entities.DbContext;
using ERP.Entities.DBModel.AppSettings;
using ERP.Entities.DBModel.Payments;
using ERP.Entities.DBModel.Users;
using ERP.Repository.Generic;
using ERP.WpfClient.Controls.Helpers;
using ERP.WpfClient.Mapper;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
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

        protected override void OnStartup(StartupEventArgs e)
        {
            //IsCreateSetup = true;
            base.OnStartup(e);
            MapperProfile.InitializeMappers();
            InitializeServices();
        }

        private void InitializeServices()
        {
            var builder = new ContainerBuilder();
            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>));
            Container = builder.Build();
            _appSettingRepository = Resolve<IGenericRepository<AppSetting>>();
            _userRepository = Resolve<IGenericRepository<User>>();
            _paymentRepository = Resolve<IGenericRepository<Payment>>();
            InitializeDB();
        }

        private void InitializeDB()
        {
            try
            {
                //AppSetting appSetting = _appSettingRepository.Get().FirstOrDefault(x => x.IsDBCreated == true);

                //if (appSetting == null)
                //{
                if (!Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "HAFood")))
                {
                    Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "HAFood"));
                }

                var spFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "HAFood");
                if (!File.Exists(Path.Combine(spFolderPath, "HAFoodDB.mdf")))
                {
                    CheckReset();
                    string filePath = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\HAFood\HAFoodDB.mdf; Integrated Security = True;";
                    HAFoodDbContext HaFoodDbContext = new HAFoodDbContext();
                    HaFoodDbContext.Init();

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

                //if (ex.HResult == -2146233079)
                //{
                //    CheckReset();
                //    //string filePath = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\HAFood\HAFoodDB.mdf; Integrated Security = True;";
                //    HAFoodDbContext HaFoodDbContext = new HAFoodDbContext();
                //    HaFoodDbContext.Init();

                //    //AppSetting appSetting = _appSettingRepository.Get().FirstOrDefault(x => x.IsDBCreated == false);

                //    //appSetting.IsDBCreated = true;
                //    //_appSettingRepository.Update(appSetting, appSetting.Id);
                //}
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

            string path = @"C:\Program Files (x86)\HAFoods Setup\HA Foods\Database";

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

        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }
    }
}
