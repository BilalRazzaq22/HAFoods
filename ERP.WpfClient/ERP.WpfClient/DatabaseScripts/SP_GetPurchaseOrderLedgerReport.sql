Create proc [dbo].[SP_GetPurchaseOrderLedgerReport] --'2021-04-25' ,'2021-04-25',1,2,'Credit'
@FromDate datetime = NULL,
@ToDate datetime = NULL,
@SupplierId int = NULL
as
begin
select CAST(po.CreatedDate as Date) as [Date],po.OrderNo as [Description],0 as Debit, 0 as Credit, 0 as Balance 
from PurchaseOrders po
left join Payments p on p.Id = po.PaymentId
WHERE ((@FromDate IS NULL OR CAST(@FromDate as Date) >= CAST(po.CreatedDate as Date))
OR (@ToDate IS NULL OR CAST(@ToDate as Date) <= CAST(po.CreatedDate as Date)))
AND (@SupplierId IS NULL OR @SupplierId = po.SupplierId)
end

