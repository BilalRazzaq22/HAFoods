Create OR ALTER proc [dbo].[SP_GetPurchaseOrderLedgerReport] --'2021-05-10' ,'2021-05-10',1
@FromDate datetime = NULL,
@ToDate datetime = NULL,
@SupplierId int = NULL
as
begin
select CAST(so.CreatedDate as Date) as [Date],so.OrderNo,so.AmountPaid as Debit, so.RemainingAmount as Credit, so.RemainingAmount as Balance
,(SUM(so.AmountPaid) + ISNULL((select SUM(cb.Amount) from CashBookOnes cb where cb.SupplierId = @SupplierId),0)) as TotalDebit,
((ISNULL((select SUM(so.RemainingAmount) from SupplierOrders so),0)) - (ISNULL((select SUM(cb.Amount) from CashBookOnes cb where cb.SupplierId = @SupplierId),0))) as TotalCredit,
((ISNULL((select SUM(so.RemainingAmount) from SupplierOrders so),0)) - (SUM(so.AmountPaid) + ISNULL((select SUM(cb.Amount) from CashBookOnes cb where cb.SupplierId = @SupplierId),0))) as TotalBalance
--(SUM(so.AmountPaid) + SUM(cbo.Amount)) as TotalDebit, (SUM(so.RemainingAmount) - SUM(cbo.Amount)) as TotalCredit,
--ISNULL((select Balance from SupplierOrders where CreatedDate in (select Max(CreatedDate) from SupplierOrders)),0) as TotalBalance
from SupplierOrders so
WHERE ((@FromDate IS NULL OR CAST(@FromDate as Date) >= CAST(so.CreatedDate as Date))
OR (@ToDate IS NULL OR CAST(@ToDate as Date) <= CAST(so.CreatedDate as Date)))
AND (@SupplierId IS NULL OR @SupplierId = so.SupplierId)
group by so.CreatedDate,so.OrderNo,so.AmountPaid,so.RemainingAmount
end