USE [Hasty]
GO
/****** Object:  StoredProcedure [dbo].[Users_SelectAll_Pagination]    Script Date: 3/7/2023 8:10:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: <Mariana Hernandez>
-- Create date: <01-25-2023T 11:29:00>
-- Description: Users Pagination
-- Code Reviewer: Doupis

-- MODIFIED BY: 
-- MODIFIED DATE:
-- Code Reviewer:
-- Note:
-- =============================================

ALTER PROC [dbo].[Users_SelectAll_Pagination]
			@PageIndex int
			,@PageSize int

as
/*----------Test Code-------------------
	DECLARE @PageIndex int= 0
			,@PageSize int = 3

	EXECUTE [dbo].[Users_SelectAll_Pagination]
			@PageIndex
			,@PageSize

	SELECT *
	FROM [dbo].[Users]

*/

BEGIN
	
	DECLARE @offset int = @PageIndex * @PageSize

	SELECT [Id]
			,[FirstName]
			,[LastName]
			,[Mi]
			,[AvatarUrl]
			,[Email]
			,[isConfirmed]
			,[StatusId]
			,[DateCreated]
			,[DateModified]
			,TotalCount = COUNT(1) OVER()
		FROM [dbo].[Users]
		ORDER BY Id
				  

	OFFSET @offSet ROWS
	FETCH NEXT @PageSize ROWS ONLY

END