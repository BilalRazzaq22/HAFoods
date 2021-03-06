CREATE OR ALTER proc [dbo].[SP_GetTopOrders]
as
begin
select top 10 ct.OrderNo, (c.FirstName + ' ' + c.LastName) as CustomerName,ct.GrandTotal,p.PaymentType from CurrentTransactions ct
left join Customers c on c.Id = ct.CustomerId
left join Payments p on p.Id = ct.PaymentId
end