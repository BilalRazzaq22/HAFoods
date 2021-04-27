USE [HAFOODDB]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetCustomerReport]    Script Date: 4/27/2021 10:01:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create proc [dbo].[SP_GetCustomerReport] --1
@CustomerId int = NULL
as
begin
select t.CustomerName,t.ContactNo,t.Debit,t.Credit, (t.Credit - t.Debit) as Balance from 
(select c.FirstName,(c.FirstName + ' ' + c.LastName) as CustomerName,c.ContactNo,co.RemainingAmount as Credit,co.AmountPaid as Debit,co.TotalAmount from Customers c
left join CustomerOrders co on c.Id = co.CustomerId 
where (@CustomerId IS NULL OR @CustomerId = c.Id)) t

end