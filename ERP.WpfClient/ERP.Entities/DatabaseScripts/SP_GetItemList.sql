USE [HAFOODDB]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetItemList]    Script Date: 4/27/2021 10:03:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create proc [dbo].[SP_GetItemList] --NULL,50
@Category nvarchar(50) = NULL,
@AlertQuantity int = NULL
as
begin
select stk.Category,stk.ItemName,stk.BuyPrice,stk.SalePrice,stk.Quantity from Stocks stk
where (@Category IS NULL OR @Category = stk.Category)
AND (@AlertQuantity IS NULL OR stk.Quantity <= @AlertQuantity )
end