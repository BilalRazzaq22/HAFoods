using Autofac;
using ERP.Entities.DbContext;
using ERP.Repository.Generic;
using ERP.WpfClient.Controls.Helpers;
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
        private bool IsCreateSetup = false;

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
            InitializeDB();
        }

        private void InitializeDB()
        {
            if (IsCreateSetup)
                CheckReset();

            try
            {

                string filePath = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\HAFood\HAFoodDB.mdf; Integrated Security = True;";
                HAFoodDbContext HaFoodDbContext = new HAFoodDbContext();
                HaFoodDbContext.Init();
                //var spFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "HAFood");
                //if (!File.Exists(Path.Combine(spFolderPath, "HAFoodDB.mdf")))
                //{
                //    Console.WriteLine("{0} Initializing DB", DateTime.Now);


                //    var filePathSetting = Path.Combine(spFolderPath, "Settings.dat");

                //    Console.WriteLine("{0} DB Initialized", DateTime.Now);
                //}
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

            }
        }

        static void readfiles()
        {
            var spFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "HAFood");
            string path = ApplicationManager.Instance.Path();
            string[] filePaths = Directory.GetFiles("C:\\Users\\bilal\\projects\\HAFoods\\ERP.WpfClient\\ERP.WpfClient\\bin\\Debug\\Database");
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
