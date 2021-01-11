using Autofac;
using ERP.Entities.DbContext;
using ERP.Repository.Generic;
using ERP.WpfClient.Controls.Helpers;
using ERP.WpfClient.Mapper;
using ERP.WpfClient.View.AutoLock;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;

namespace ERP.WpfClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IContainer Container { get; private set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MapperProfile.InitializeMappers();
            InitializeServices();
            CheckDisability();
            ApplicationManager.Instance.AutoDiableStorePopUp();
        }

        private void InitializeServices()
        {
            var builder = new ContainerBuilder();
            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>));
            Container = builder.Build();
            InitializeDB();
        }

        private void CheckDisability()
        {
            var todayDate = "2021-04-11";
            DateTime dt3 = new DateTime(2021, 04, 11, 18, 30, 20);
            var d = System.Diagnostics.Stopwatch.GetTimestamp();
            var utc = DateTime.UtcNow;
            var now = DateTime.Now;
            var date = Convert.ToDateTime(todayDate);
            var dgd = GetNow();
            var dsdd = ApplicationManager.Instance.GetCurrentTime();

        }

        [DllImport("Kernel32.dll", CallingConvention = CallingConvention.Winapi)]
        static extern void GetSystemTimePreciseAsFileTime(out long filetime);

        public DateTimeOffset GetNow()
        {
            long fileTime;
            GetSystemTimePreciseAsFileTime(out fileTime);
            return DateTimeOffset.FromFileTime(fileTime);
        }

 



        private void InitializeDB()
        {
            CheckReset();

            if (!Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "HAFood")))
            {
                Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "HAFood"));
            }

            var spFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "HAFood");
            if (!File.Exists(Path.Combine(spFolderPath, "HAFoodDB.mdf")))
            {
                Console.WriteLine("{0} Initializing DB", DateTime.Now);

                string filePath = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\HAFood\HAFoodDB.mdf; Integrated Security = True;";
                HAFoodDbContext HaFoodDbContext = new HAFoodDbContext();
                HaFoodDbContext.Init();

                var filePathSetting = Path.Combine(spFolderPath, "Settings.dat");

                Console.WriteLine("{0} DB Initialized", DateTime.Now);
            }
        }

        static void CheckReset()
        {
            try
            {
                var spFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "HAFood");
                //var filePath = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "HAFood"), "construct.json");
                if (Directory.Exists(spFolderPath))
                {
                    //if (File.ReadAllText(filePath).Equals("{'Reset':'True'}"))
                    //{
                        File.Delete(Path.Combine(spFolderPath, "HAFoodDB.mdf"));
                        File.Delete(Path.Combine(spFolderPath, "HAFoodDB_log.ldf"));
                        //File.Delete(spFolderPath);
                    //}
                }
            }
            catch (Exception ex)
            {

            }

        }

        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }
    }
}
