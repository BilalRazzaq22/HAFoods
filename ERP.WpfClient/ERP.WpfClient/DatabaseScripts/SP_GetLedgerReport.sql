Create OR ALTER proc [dbo].[SP_GetLedgerReport] --'2021-04-25' ,'2021-04-25',1,2,'Credit'
@FromDate datetime = NULL,
@ToDate datetime = NULL,
@CustomerId int = NULL,
@SupplierId int = NULL
as
begin
select CAST(ct.CreatedDate as Date) as [Date],ct.OrderNo as [Description],ct.TotalPrice as Debit, 0 as Credit from CurrentTransactions ct
left join Payments p on p.Id = ct.PaymentId
WHERE ((@FromDate IS NULL OR CAST(@FromDate as Date) >= CAST(ct.CreatedDate as Date))
OR (@ToDate IS NULL OR CAST(@ToDate as Date) <= CAST(ct.CreatedDate as Date)))
AND (@CustomerId IS NULL OR @CustomerId = ct.CustomerId)
AND p.PaymentType = 'Cash'

Union All

select CAST(ct.CreatedDate as Date) as [Date],ct.OrderNo as [Description], 0 as Debit, ct.TotalPrice as Credit from CurrentTransactions ct
left join Payments p on p.Id = ct.PaymentId
WHERE ((@FromDate IS NULL OR CAST(@FromDate as Date) >= CAST(ct.CreatedDate as Date))
OR (@ToDate IS NULL OR CAST(@ToDate as Date) <= CAST(ct.CreatedDate as Date)))
AND (@CustomerId IS NULL OR @CustomerId = ct.CustomerId)
AND p.PaymentType = 'Credit'

end

