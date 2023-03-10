USE [Hasty]
GO
/****** Object:  StoredProcedure [dbo].[StatusTypes_TotalCount_Each]    Script Date: 3/7/2023 8:11:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: <Mariana Hernandez>
-- Create date: 2023-02-28T17:31
-- Description: SelectAll_TotalCount_Each
-- Code Reviewer: Jailene Kelly

-- MODIFIED BY:
-- MODIFIED DATE:
-- Code Reviewer:
-- Note:
-- =============================================
ALTER PROC [dbo].[StatusTypes_TotalCount_Each]

as
/*----------Test Code-------------------


	EXECUTE [dbo].[StatusTypes_TotalCount_Each]


	SELECT *
	FROM [dbo].[Users]
	select *
	from dbo.statustypes
	select *
	from dbo.subscriptions

*/

BEGIN

		SELECT COUNT(*) as UserCount
		from dbo.Users

		SELECT  count(u.StatusId)as TotalCount
				,s.Name
		from dbo.users as u
			inner join dbo.StatusTypes as s
			on u.StatusId = s.Id
		group by u.StatusId, s.Name

		SELECT COUNT(s.id) as ActiveSubscription
				,s.Status
		from dbo.Subscriptions as s
		where s.status like 'active'
		group by s.Status

		SELECT COUNT(sp.Id) as subsByPlan
				,sp.Name
				,sum(sp.Amount) as incomeBySub
				,sp.Amount
		from dbo.Subscriptions as s
		INNER JOIN dbo.StripeProductSubscription sps ON s.Id = sps.SubscriptionId
		INNER JOIN dbo.StripeProducts sp ON sp.Id = sps.ProductId
		group by sp.name,sp.Amount

		select datepart(week,DateCreated)subsByWeek, count(*) Subscription
		from dbo. Subscriptions
		where year(datecreated)=YEAR(getutcdate())
		group by datepart(week, DateCreated)	

		select YEAR(datecreated)subsByYear, count(*) Subscription
		from dbo. Subscriptions
		where year(datecreated)=YEAR(getutcdate())
		group by year(datecreated)

		select dateName(month, datecreated)subsByMonth, count(*) Subscription
		from dbo. Subscriptions
		where year(datecreated)=YEAR(getutcdate())
		group by dateName(month, datecreated)	



END