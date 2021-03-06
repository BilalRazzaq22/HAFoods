Create OR ALTER proc [dbo].[SP_GetCurrentTransactionLedgerReport] --'2021-05-01' ,'2021-05-30',2
@FromDate datetime = NULL,
@ToDate datetime = NULL,
@CustomerId int = NULL
as
begin
select (c.FirstName + ' ' + c.LastName) as CustomerName,CAST(co.CreatedDate as Date) as [Date],co.OrderNo as [Description],co.AmountPaid as Debit, co.RemainingAmount as Credit, co.RemainingAmount as Balance
from CustomerOrders co
left join Customers c on c.Id = co.CustomerId
WHERE ((CAST(co.CreatedDate as Date) >= CAST(@FromDate as Date)) 
OR (CAST(co.CreatedDate as Date) <= CAST(@ToDate as Date)))
AND (@CustomerId = co.CustomerId)
group by c.FirstName , c.LastName,co.CreatedDate,co.OrderNo,co.AmountPaid,co.RemainingAmount

UNION ALL

select t.*,(t.Credit - t.Debit) as Balance from (
select NULL as CustomerName,CAST(cb.CashBookOneDate as Date) as [Date],cb.[Description],
case when (p.PaymentType = 'Cash') then cb.Amount else '0.00' end as Debit,
case when (p.PaymentType = 'Credit') then cb.Amount else '0.00' end as Credit
from CashBookOnes cb
left join Customers c on c.Id = cb.CustomerId
left join Payments p on p.Id = cb.PaymentId
WHERE ((CAST(cb.CashBookOneDate as Date) >= CAST(@FromDate as Date)) 
AND (CAST(cb.CashBookOneDate as Date) <= CAST(@ToDate as Date)))
AND (@CustomerId = cb.CustomerId))t

UNION ALL

select t.*,(t.Credit - t.Debit) as Balance from (
select NULL as CustomerName,CAST(cbt.CashBookTwoDate as Date) as [Date],cbt.DebiterDescription as [Description],
case when (cbt.DebiterType = 'Customer') then cbt.DebiterAmount else '0.00' end as Debit,
case when (cbt.CrediterType = 'Customer') then cbt.DebiterAmount else '0.00' end as Credit
from CashBookTwoes cbt
left join Customers c on c.Id = cbt.DebiterCustomerId
WHERE ((CAST(cbt.CashBookTwoDate as Date) >= CAST(@FromDate as Date)) 
AND (CAST(cbt.CashBookTwoDate as Date) <= CAST(@ToDate as Date)))
AND ((@CustomerId IS NOT NULL AND @CustomerId = cbt.CrediterCustomerId) OR (@CustomerId IS NOT NULL AND @CustomerId = cbt.DebiterCustomerId)))t

end