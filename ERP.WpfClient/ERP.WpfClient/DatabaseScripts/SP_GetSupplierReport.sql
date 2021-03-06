Create OR ALTER proc [dbo].[SP_GetSupplierReport] --6
@SupplierId int = NULL
as
begin
select (s.FirstName + ' ' + s.LastName) as SupplierName,s.ContactNo,
ISNULL((select SUM(so.AmountPaid) from SupplierOrders so where so.SupplierId = s.Id),0) as Debit,
ISNULL((select SUM(so.RemainingAmount) from SupplierOrders so where so.SupplierId = s.Id),0) as Credit,
(ISNULL((select SUM(so.TotalAmount) from SupplierOrders so where so.SupplierId = s.Id),0) - ISNULL((select SUM(so.AmountPaid) from SupplierOrders so where so.SupplierId = s.Id),0)) as Balance from Suppliers s
left join SupplierOrders so on s.Id = so.SupplierId
where (@SupplierId IS NULL OR @SupplierId = so.SupplierId)
group by s.Id,s.FirstName,s.LastName,s.ContactNo
end