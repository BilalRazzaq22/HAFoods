Create OR ALTER proc [dbo].[SP_GetCurrentTransactionOrder] --4
@CurrentTransactionId int = null,
@CustomerId int = NULL
as
begin
select stk.ItemName,ctd.Quantity,ctd.Price,ctd.Discount,ctd.TotalPrice,ct.OrderNo,
ct.TotalPrice as GrandTotalPrice,ct.TotalDiscount,ct.GrandTotal,Sum(co.TotalAmount) as TotalAmount,Sum(co.AmountPaid) as AmountPaid,
ISNULL((select Balance from CustomerOrders where CreatedDate in (select MAX(CreatedDate) from CustomerOrders)),0) as RemainingAmount,
p.PaymentType,ct.CreatedDate,ISNULL((select Count(CustomerId) from CurrentTransactions ct where ct.CustomerId = @CustomerId),0) as TotalCustomerOrders from CurrentTransactions ct
left join CurrentTransactionDetails ctd on ctd.CurrentTransactionId = ct.Id
left join Stocks stk on stk.Id = ctd.StockId
left join Payments p on p.Id = ct.PaymentId
left join CustomerOrders co on co.CustomerId = ct.CustomerId
where (@CurrentTransactionId IS NULL OR @CurrentTransactionId = ct.Id) 
group by stk.ItemName,ctd.Quantity,ctd.Price,ctd.Discount,ctd.TotalPrice,ct.OrderNo,
ct.TotalPrice,ct.TotalDiscount,ct.GrandTotal,p.PaymentType,ct.CreatedDate
end