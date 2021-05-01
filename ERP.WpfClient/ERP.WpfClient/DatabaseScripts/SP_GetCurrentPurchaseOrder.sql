ALTER proc [dbo].[SP_GetCurrentPurchaseOrder] --3
@PurchaseOrderId int = null
as
begin
select stk.ItemName,poi.PurchaseQuantity,poi.BuyPrice,poi.Discount,poi.TotalPrice,po.OrderNo,po.PurchaseOrderDate,po.CreatedDate,p.PaymentType,po.TotalQuantity,
po.GrandTotal from PurchaseOrders po
left join PurchaseOrderItems poi on poi.PurchaseOrderId = po.Id
left join Stocks stk on stk.Id = poi.StockId
left join Payments p on p.Id = po.PaymentId
where (@PurchaseOrderId IS NULL OR @PurchaseOrderId = po.Id) 
end