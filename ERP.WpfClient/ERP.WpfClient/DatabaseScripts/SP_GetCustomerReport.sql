Create OR ALTER proc [dbo].[SP_GetCustomerReport] --1
@CustomerId int = NULL
as
begin
select co.OrderNo,(c.FirstName + ' ' + c.LastName) as CustomerName,c.ContactNo,co.AmountPaid as Debit,co.RemainingAmount as Credit,
(co.TotalAmount - co.AmountPaid) as Balance from Customers c
left join CustomerOrders co on c.Id = co.CustomerId
where (@CustomerId IS NULL OR @CustomerId = co.CustomerId)
end