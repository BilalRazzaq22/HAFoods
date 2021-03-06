Create OR ALTER proc [dbo].[SP_GetItemList] --NULL,50
@Category nvarchar(50) = NULL,
@AlertQuantity int = NULL
as
begin
select stk.Category,stk.ItemName,stk.BuyPrice,stk.SalePrice,stk.Quantity from Stocks stk
where (@Category IS NULL OR @Category = stk.Category)
AND (@AlertQuantity IS NULL OR stk.Quantity <= @AlertQuantity )
end