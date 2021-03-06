Create OR ALTER proc [dbo].[SP_GetDailySalesReport] --'2021-04-25' ,'2021-04-25',1,2,'Credit'
@FromDate datetime = NULL,
@ToDate datetime = NULL,
@CustomerId int = NULL,
@StockId int = NULL,
@PaymentType nvarchar(50) = NULL
as
begin
select ct.OrderNo as InvoiceNo, stk.ItemName,stk.SalePrice as UnitPrice,stk.Packing,ctd.Quantity,ctd.TotalPrice,CAST(ct.CreatedDate as Date) as [Date],
(c.FirstName + ' ' + c.LastName) as CustomerName from CurrentTransactions ct
left join CurrentTransactionDetails ctd on ctd.CurrentTransactionId = ct.Id
left join Stocks stk on stk.Id = ctd.StockId
left join Customers c on ct.CustomerId = c.Id
left join Payments p on p.Id = ct.PaymentId
WHERE ((@FromDate IS NULL OR CAST(@FromDate as Date) >= CAST(ct.CreatedDate as Date))
OR (@ToDate IS NULL OR CAST(@ToDate as Date) <= CAST(ct.CreatedDate as Date)))
AND (@CustomerId IS NULL OR @CustomerId = ct.CustomerId)
AND (@StockId IS NULL OR @StockId = ctd.StockId)
AND (@PaymentType IS NULL OR @PaymentType = p.PaymentType)
end