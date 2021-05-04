Create proc [dbo].[SP_GetCurrentPurchaseOrder] --8
@PurchaseOrderId int = null
as
begin
select stk.ItemName,poi.PurchaseQuantity,poi.BuyPrice,poi.Discount,poi.TotalPrice,po.OrderNo,
po.TotalPrice as GrandTotalPrice,po.TotalDiscount,po.GrandTotal,so.TotalAmount,so.AmountPaid,so.RemainingAmount,p.PaymentType,po.CreatedDate,
ISNULL((select Count(po.SupplierId) from PurchaseOrders po where po.SupplierId = so.SupplierId),0) as TotalSuppliers from PurchaseOrders po
left join PurchaseOrderItems poi on poi.PurchaseOrderId = po.Id
left join Stocks stk on stk.Id = poi.StockId
left join Payments p on p.Id = po.PaymentId
left join SupplierOrders so on so.SupplierId = po.SupplierId
where (@PurchaseOrderId IS NULL OR @PurchaseOrderId = po.Id) 
end