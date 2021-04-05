using Autofac;
using ERP.Repository.Generic;
using ERP.WpfClient.Mapper;
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
        }

        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }
    }
}
