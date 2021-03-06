Create OR ALTER proc [dbo].[SP_GetPurchaseOrderLedgerReport] --'2021-05-01' ,'2021-05-31',1
@FromDate datetime = NULL,
@ToDate datetime = NULL,
@SupplierId int = NULL
as
begin
select (s.FirstName + ' ' + s.LastName) as SupplierName,CAST(so.CreatedDate as Date) as [Date],so.OrderNo as [Description],so.AmountPaid as Debit, so.RemainingAmount as Credit, so.RemainingAmount as Balance
from SupplierOrders so
left join Suppliers s on s.Id = so.SupplierId
WHERE ((CAST(so.CreatedDate as Date) >= CAST(@FromDate as Date)) 
AND (CAST(so.CreatedDate as Date) <= CAST(@ToDate as Date)))
AND (@SupplierId = so.SupplierId)
group by s.FirstName , s.LastName,so.CreatedDate,so.OrderNo,so.AmountPaid,so.RemainingAmount

UNION ALL

select t.*,(t.Credit - t.Debit) as Balance from (
select NULL as SupplierName,CAST(cb.CashBookOneDate as Date) as [Date],cb.[Description],
case when (p.PaymentType = 'Cash') then cb.Amount else '0.00' end as Debit,
case when (p.PaymentType = 'Credit') then cb.Amount else '0.00' end as Credit
from CashBookOnes cb
left join Suppliers s on s.Id = cb.SupplierId
left join Payments p on p.Id = cb.PaymentId
WHERE ((CAST(cb.CashBookOneDate as Date) >= CAST(@FromDate as Date)) 
AND (CAST(cb.CashBookOneDate as Date) <= CAST(@ToDate as Date)))
AND (@SupplierId = cb.SupplierId))t

UNION ALL

select t.*,(t.Credit - t.Debit) as Balance from (
select NULL as CustomerName,CAST(cbt.CashBookTwoDate as Date) as [Date],cbt.DebiterDescription as [Description],
case when (cbt.DebiterType = 'Supplier') then cbt.DebiterAmount else '0.00' end as Debit,
case when (cbt.CrediterType = 'Supplier') then cbt.DebiterAmount else '0.00' end as Credit
from CashBookTwoes cbt
left join Customers c on c.Id = cbt.DebiterSupplierId OR c.Id = cbt.CrediterSupplierId
WHERE ((CAST(cbt.CashBookTwoDate as Date) >= CAST(@FromDate as Date)) 
AND (CAST(cbt.CashBookTwoDate as Date) <= CAST(@ToDate as Date)))
AND ((@SupplierId IS NOT NULL AND @SupplierId = cbt.CrediterSupplierId) OR (@SupplierId IS NOT NULL AND @SupplierId = cbt.DebiterSupplierId)))t

end