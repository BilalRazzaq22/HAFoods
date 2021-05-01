Create proc [dbo].[SP_GetCurrentTransactionLedgerReport] --'2021-04-25' ,'2021-04-25',1,2,'Credit'
@FromDate datetime = NULL,
@ToDate datetime = NULL,
@CustomerId int = NULL
as
begin
select CAST(ct.CreatedDate as Date) as [Date],ct.OrderNo as [Description],co.AmountPaid as Debit, co.RemainingAmount as Credit, (co.TotalAmount - co.AmountPaid) as Balance from CurrentTransactions ct
left join CustomerOrders co on co.CustomerId = ct.CustomerId
left join Payments p on p.Id = ct.PaymentId
WHERE ((@FromDate IS NULL OR CAST(@FromDate as Date) >= CAST(ct.CreatedDate as Date))
OR (@ToDate IS NULL OR CAST(@ToDate as Date) <= CAST(ct.CreatedDate as Date)))
AND (@CustomerId IS NULL OR @CustomerId = ct.CustomerId)
end

