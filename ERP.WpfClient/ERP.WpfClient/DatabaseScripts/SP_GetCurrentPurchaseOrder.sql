Create OR ALTER proc [dbo].[SP_GetCurrentPurchaseOrder] --3,1
@PurchaseOrderId int = NULL,
@SupplierId int = NULL
as
begin
select (s.FirstName + ' ' + s.LastName) as SupplierName,stk.ItemName,poi.PurchaseQuantity,poi.BuyPrice,stk.Packing,(poi.BuyPrice * stk.Packing) as PerCotton,poi.Discount,poi.TotalPrice,po.OrderNo,
po.TotalPrice as GrandTotalPrice,po.TotalDiscount,po.GrandTotal,Sum(so.TotalAmount) as TotalAmount,Sum(so.AmountPaid) as AmountPaid,
ISNULL((select Balance from SupplierOrders where CreatedDate in (select MAX(CreatedDate) from SupplierOrders)),0) as RemainingAmount,
p.PaymentType,po.CreatedDate,ISNULL((select Count(SupplierId) from PurchaseOrders po where po.SupplierId = @SupplierId),0) as TotalSuppliers,
(so.Balance) as PreviousBalance from PurchaseOrders po
left join PurchaseOrderItems poi on poi.PurchaseOrderId = po.Id
left join Stocks stk on stk.Id = poi.StockId
left join Payments p on p.Id = po.PaymentId
left join SupplierOrders so on so.SupplierId = po.SupplierId
left join Suppliers s on s.Id = po.SupplierId
where (@PurchaseOrderId IS NULL OR @PurchaseOrderId = po.Id) 
group by s.FirstName,s.LastName,stk.ItemName,poi.PurchaseQuantity,poi.BuyPrice,poi.Discount,poi.TotalPrice,po.OrderNo,
po.TotalPrice,po.TotalDiscount,po.GrandTotal,p.PaymentType,po.CreatedDate,so.Balance,stk.Packing
end