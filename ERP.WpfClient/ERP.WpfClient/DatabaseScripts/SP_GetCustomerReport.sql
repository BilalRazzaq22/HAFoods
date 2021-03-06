Create OR ALTER proc [dbo].[SP_GetCustomerReport] --1
@CustomerId int = NULL
as
begin
select (c.FirstName + ' ' + c.LastName) as CustomerName,c.ContactNo,
ISNULL((select SUM(co.AmountPaid) from CustomerOrders co where co.CustomerId = c.Id),0) as Debit,
ISNULL((select SUM(co.RemainingAmount) from CustomerOrders co where co.CustomerId = c.Id),0) as Credit,
(ISNULL((select SUM(co.TotalAmount) from CustomerOrders co where co.CustomerId = c.Id),0) - ISNULL((select SUM(co.AmountPaid) from CustomerOrders co where co.CustomerId = c.Id),0)) as Balance from Customers c
left join CustomerOrders co on c.Id = co.CustomerId
where (@CustomerId IS NULL OR @CustomerId = co.CustomerId)
group by c.Id,c.FirstName,c.LastName,c.ContactNo
end