using AutoMapper;
using ERP.Entities.DBModel;
using ERP.WpfClient.Model;
using ERP.WpfClient.Model.Stock;
using ERP.WpfClient.Model.Supplier;
using ERP.WpfClient.Model.Transaction;
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
                cfg.CreateMap<Supplier, SupplierModel>().ReverseMap();
                cfg.CreateMap<Stock, StockModel>().ReverseMap();
                cfg.CreateMap<CurrentTransaction, CurrentTransactionModel>().ReverseMap();
                cfg.CreateMap<CurrentTransactionDetail, CurrentTransactionDetailModel>().ReverseMap();
            });

            iMapper = config.CreateMapper();
        }
    }
}
