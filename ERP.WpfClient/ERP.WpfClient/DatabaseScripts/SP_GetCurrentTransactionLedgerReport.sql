Create OR ALTER proc [dbo].[SP_GetCurrentTransactionLedgerReport] --'2021-05-01' ,'2021-05-10',1
@FromDate datetime = NULL,
@ToDate datetime = NULL,
@CustomerId int = NULL
as
begin
select (c.FirstName + ' ' + c.LastName) as CustomerName,CAST(co.CreatedDate as Date) as [Date],co.OrderNo,co.AmountPaid as Debit, co.RemainingAmount as Credit, co.RemainingAmount as Balance
,(SUM(co.AmountPaid) + ISNULL((select SUM(cb.Amount) from CashBookOnes cb where cb.CustomerId = @CustomerId),0)) as TotalDebit,
((ISNULL((select SUM(co.RemainingAmount) from CustomerOrders co),0)) - (ISNULL((select SUM(cb.Amount) from CashBookOnes cb where cb.CustomerId = @CustomerId),0))) as TotalCredit,
((ISNULL((select SUM(co.RemainingAmount) from CustomerOrders co),0)) - (SUM(co.AmountPaid) + ISNULL((select SUM(cb.Amount) from CashBookOnes cb where cb.CustomerId = @CustomerId),0))) as TotalBalance
from CustomerOrders co
left join Customers c on c.Id = co.CustomerId
WHERE ((@FromDate IS NULL OR CAST(co.CreatedDate as Date) >= CAST(@FromDate as Date)) 
AND (@ToDate IS NULL OR CAST(co.CreatedDate as Date) <= CAST(@ToDate as Date)))
AND (@CustomerId IS NULL OR @CustomerId = co.CustomerId)
group by c.FirstName , c.LastName,co.CreatedDate,co.OrderNo,co.AmountPaid,co.RemainingAmount
end

