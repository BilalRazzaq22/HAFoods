Create OR ALTER proc [dbo].[SP_GetSupplierReport] --6
@SupplierId int = NULL
as
begin
select so.OrderNo,(s.FirstName + ' ' + s.LastName) as SupplierName,s.ContactNo,so.AmountPaid as Debit,so.RemainingAmount as Credit,
(so.TotalAmount - so.AmountPaid) as Balance from Suppliers s
left join SupplierOrders so on s.Id = so.SupplierId
where (@SupplierId IS NULL OR @SupplierId = so.SupplierId)
end