CREATE OR ALTER proc [dbo].[SP_GetCustomerSaleOrder] --'2021-05-23'--,'1002'
@Date datetime = NULL,
@OrderNo nvarchar(50) = NULL
as
begin
select ct.OrderNo,(c.FirstName + ' ' + c.LastName) as CustomerName,stk.ItemName,ctd.Quantity,ctd.Price,stk.Packing,(ctd.Price * stk.Packing) as PerCarton,ctd.Discount,ctd.TotalPrice,
ct.TotalPrice as GrandTotalPrice,ct.TotalDiscount,ct.GrandTotal,
ISNULL((select co.TotalAmount from CustomerOrders co where co.CustomerId = ct.CustomerId and co.OrderNo = ct.OrderNo),0) as TotalAmount,
ISNULL((select SUM(co.AmountPaid) from CustomerOrders co where co.CustomerId = ct.CustomerId and co.OrderNo = ct.OrderNo),0) as AmountPaid,
ISNULL((select Balance from CustomerOrders co where co.OrderNo = ct.OrderNo),0) as RemainingAmount,
ISNULL((select co.Balance from CustomerOrders co where co.CustomerId = ct.CustomerId and co.OrderNo = ct.OrderNo),0) as PreviousBalance,CAST(ct.CreatedDate as Date) as TransactionDate from CurrentTransactions ct
left join CurrentTransactionDetails ctd on ctd.CurrentTransactionId = ct.Id
left join Stocks stk on stk.Id = ctd.StockId
--left join Payments p on p.Id = ct.PaymentId
--left join CustomerOrders so on so.CustomerId = ct.CustomerId
left join Customers c on c.Id = ct.CustomerId
WHERE (CAST(ct.CreatedDate as Date) = CAST(@Date as Date)) 
--AND (@OrderNo = ct.OrderNo)
--group by c.FirstName,c.LastName,stk.ItemName,ctd.Quantity,ctd.Price,ctd.Discount,ctd.TotalPrice,ct.OrderNo,
--ct.TotalPrice,ct.TotalDiscount,ct.GrandTotal,p.PaymentType,ct.CreatedDate,stk.Packing,so.Balance
end