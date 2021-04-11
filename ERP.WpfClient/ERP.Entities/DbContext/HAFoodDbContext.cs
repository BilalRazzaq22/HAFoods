﻿using ERP.Entities.DBModel;
using ERP.Entities.DBModel.Customers;
using ERP.Entities.DBModel.Payments;
using ERP.Entities.DBModel.Stocks;
using ERP.Entities.DBModel.Suppliers;
using ERP.Entities.DBModel.Transactions;
using System;
using System.Data.Entity;

namespace ERP.Entities.DbContext
{
    public class HAFoodDbContext : System.Data.Entity.DbContext
    {
        // Commented Code is intentionally placed
        //string  connectionString = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = |DataDirectory|\FridayRetail\FridayRetail.mdf; Integrated Security = True;";
        //(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Avais-pc\AppData\Local\FridayRetail\FridayRetail.mdf;Integrated Security = True;")
        public HAFoodDbContext() : base(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\HAFood\HAFoodDB.mdf; Integrated Security = True;")
        {
            //@"Server=175.10.21.13,1433; Database=FridayRetailDb;  Database=FridayRetailPOS; User=sa; Password=techverx@123;MultipleActiveResultSets=true"
        }
        public void Init()
        {
            this.Database.Initialize(true);
        }

        public DbSet<CurrentTransaction> CurrentTransactions { get; set; }
        public DbSet<CurrentTransactionDetail> CurrentTransactionDetails { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
    }
}