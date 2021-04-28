using AutoMapper;
using ERP.Entities.DBModel;
using ERP.Entities.DBModel.CashBook;
using ERP.Entities.DBModel.Customers;
using ERP.Entities.DBModel.Payments;
using ERP.Entities.DBModel.Stocks;
using ERP.Entities.DBModel.Suppliers;
using ERP.Entities.DBModel.Transactions;
using ERP.Entities.DBModel.Users;
using ERP.WpfClient.Model;
using ERP.WpfClient.Model.CashBooks;
using ERP.WpfClient.Model.Payment;
using ERP.WpfClient.Model.Stock;
using ERP.WpfClient.Model.Supplier;
using ERP.WpfClient.Model.Transaction;
using ERP.WpfClient.Model.User;
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
                cfg.CreateMap<Payment, PaymentModel>().ReverseMap();
                cfg.CreateMap<User, UserModel>().ReverseMap();
                cfg.CreateMap<CashBookOne, CashBookOneModel>().ReverseMap();
            });

            iMapper = config.CreateMapper();
        }
    }
}
