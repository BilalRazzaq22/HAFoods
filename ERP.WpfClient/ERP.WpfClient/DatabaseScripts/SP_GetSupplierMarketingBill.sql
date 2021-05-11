Create OR ALTER proc [dbo].[SP_GetSupplierMarketingBill] --3,1
@SupplierMarketingBillId int = NULL,
@SupplierId int = NULL
as
begin
select (s.FirstName + ' ' + s.LastName) as SupplierName,stk.ItemName,smi.PurchaseQuantity,smi.BuyPrice,smi.Discount,smi.TotalPrice,smo.OrderNo,
smo.TotalPrice as GrandTotalPrice,smo.TotalDiscount,smo.GrandTotal,Sum(so.TotalAmount) as TotalAmount,Sum(so.AmountPaid) as AmountPaid,
ISNULL((select Balance from SupplierMarketingOrderAmounts where CreatedDate in (select MAX(CreatedDate) from SupplierMarketingOrderAmounts)),0) as RemainingAmount,
p.PaymentType,smo.CreatedDate,ISNULL((select Count(SupplierId) from SupplierMarketingOrders smo where smo.SupplierId = @SupplierId),0) as TotalSuppliers,
(smo.GrandTotal - smo.TotalPrice) as PreviousBalance from SupplierMarketingOrders smo
left join SupplierMarketingOrderItems smi on smi.SupplierMarketingOrderId = smo.Id
left join Stocks stk on stk.Id = smi.StockId
left join Payments p on p.Id = smo.PaymentId
left join SupplierMarketingOrderAmounts so on so.SupplierId = smo.SupplierId
left join Suppliers s on s.Id = smo.SupplierId
where (@SupplierMarketingBillId IS NULL OR @SupplierMarketingBillId = smo.Id) 
group by s.FirstName,s.LastName,stk.ItemName,smi.PurchaseQuantity,smi.BuyPrice,smi.Discount,smi.TotalPrice,smo.OrderNo,
smo.TotalPrice,smo.TotalDiscount,smo.GrandTotal,p.PaymentType,smo.CreatedDate
end