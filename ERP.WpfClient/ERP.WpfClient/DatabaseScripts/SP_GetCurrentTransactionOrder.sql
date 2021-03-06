Create OR ALTER proc [dbo].[SP_GetCurrentTransactionOrder] --9,1
@CurrentTransactionId int = null,
@CustomerId int = NULL
as
begin
select (c.FirstName + ' ' + c.LastName) as CustomerName,stk.ItemName,ctd.Quantity,ctd.Price,stk.Packing,(ctd.Price * stk.Packing) as PerCotton,ctd.Discount,ctd.TotalPrice,ct.OrderNo,
ct.TotalPrice as GrandTotalPrice,ct.TotalDiscount,ct.GrandTotal,Sum(so.TotalAmount) as TotalAmount,Sum(so.AmountPaid) as AmountPaid,
ISNULL((select Balance from CustomerOrders where CreatedDate in (select MAX(CreatedDate) from CustomerOrders)),0) as RemainingAmount,
p.PaymentType,ct.CreatedDate,ISNULL((select Count(CustomerId) from CurrentTransactions ct where ct.CustomerId = @CustomerId),0) as TotalSuppliers,
(so.Balance) as PreviousBalance from CurrentTransactions ct
left join CurrentTransactionDetails ctd on ctd.CurrentTransactionId = ct.Id
left join Stocks stk on stk.Id = ctd.StockId
left join Payments p on p.Id = ct.PaymentId
left join CustomerOrders so on so.CustomerId = ct.CustomerId
left join Customers c on c.Id = ct.CustomerId
where (@CurrentTransactionId IS NULL OR @CurrentTransactionId = ct.Id) 
group by c.FirstName,c.LastName,stk.ItemName,ctd.Quantity,ctd.Price,ctd.Discount,ctd.TotalPrice,ct.OrderNo,
ct.TotalPrice,ct.TotalDiscount,ct.GrandTotal,p.PaymentType,ct.CreatedDate,stk.Packing,so.Balance
end