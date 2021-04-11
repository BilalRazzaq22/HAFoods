using Autofac;
using ERP.Entities.DbContext;
using ERP.Repository.Generic;
using ERP.WpfClient.Mapper;
using System;
using System.IO;
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
        }

        private void InitializeServices()
        {
            var builder = new ContainerBuilder();
            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>));
            Container = builder.Build();
            InitializeDB();
        }

        private void InitializeDB()
        {
            CheckReset();

            try
            {
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
            catch (Exception ex)
            {

                throw;
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
