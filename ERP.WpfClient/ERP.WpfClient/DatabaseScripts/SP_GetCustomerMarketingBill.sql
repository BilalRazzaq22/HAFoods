Create OR ALTER proc [dbo].[SP_GetCustomerMarketingBill] --3,1
@CustomerMarketingBillId int = NULL,
@CustomerId int = NULL
as
begin
select (c.FirstName + ' ' + c.LastName) as CustomerName,stk.ItemName,cmi.Quantity,cmi.Price,cmi.Discount,cmi.TotalPrice,cmo.OrderNo,
cmo.TotalPrice as GrandTotalPrice,cmo.TotalDiscount,cmo.GrandTotal,Sum(co.TotalAmount) as TotalAmount,Sum(co.AmountPaid) as AmountPaid,
ISNULL((select Balance from CustomerMarketingOrderAmounts where CreatedDate in (select MAX(CreatedDate) from CustomerMarketingOrderAmounts)),0) as RemainingAmount,
p.PaymentType,cmo.CreatedDate,ISNULL((select Count(CustomerId) from CustomerMarketingOrders cmo where cmo.CustomerId = @CustomerId),0) as TotalSuppliers,
(cmo.GrandTotal - cmo.TotalPrice) as PreviousBalance from CustomerMarketingOrders cmo
left join CustomerMarketingOrderItems cmi on cmi.CustomerMarketingOrderId = cmo.Id
left join Stocks stk on stk.Id = cmi.StockId
left join Payments p on p.Id = cmo.PaymentId
left join CustomerMarketingOrderAmounts co on co.CustomerId = cmo.CustomerId
left join Customers c on c.Id = cmo.CustomerId
where (@CustomerMarketingBillId IS NULL OR @CustomerMarketingBillId = cmo.Id) 
group by c.FirstName,c.LastName,stk.ItemName,cmi.Quantity,cmi.Price,cmi.Discount,cmi.TotalPrice,cmo.OrderNo,
cmo.TotalPrice,cmo.TotalDiscount,cmo.GrandTotal,p.PaymentType,cmo.CreatedDate
end