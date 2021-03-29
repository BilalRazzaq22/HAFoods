using AutoMapper;
using ERP.Entities.DBModel;
using ERP.WpfClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.WpfClient.Mapper
{
    public class MapperProfile
    {
        public static IMapper iMapper { get; set; }
        public static void InitializeMappers()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Customer, CustomerModel>().ReverseMap();
            });

            iMapper = config.CreateMapper();
        }
    }
}
