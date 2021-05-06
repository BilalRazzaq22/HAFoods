Create OR ALTER proc [dbo].[SP_GetPurchaseOrderLedgerReport] --'2021-04-25' ,'2021-04-25',1,2,'Credit'
@FromDate datetime = NULL,
@ToDate datetime = NULL,
@SupplierId int = NULL
as
begin
select CAST(so.CreatedDate as Date) as [Date],so.OrderNo,so.AmountPaid as Debit, so.Balance as Credit, so.Balance from SupplierOrders so
WHERE ((@FromDate IS NULL OR CAST(@FromDate as Date) >= CAST(so.CreatedDate as Date))
OR (@ToDate IS NULL OR CAST(@ToDate as Date) <= CAST(so.CreatedDate as Date)))
AND (@SupplierId IS NULL OR @SupplierId = so.SupplierId)
end
