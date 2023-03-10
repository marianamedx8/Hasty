USE [Hasty]
GO
/****** Object:  StoredProcedure [dbo].[Users_SelectById]    Script Date: 3/7/2023 8:09:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: <Mariana Hernandez>
-- Create date: <01-25-2023T 11:29:00>
-- Description: Users SelectById
-- Code Reviewer: Doupis

-- MODIFIED BY: 
-- MODIFIED DATE:
-- Code Reviewer:
-- Note:
-- =============================================

ALTER PROC [dbo].[Users_SelectById]
			@Id int

/*---Test Code---

	DECLARE @Id int=11

	EXECUTE [dbo].[Users_SelectById] @Id

*/

AS

BEGIN

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
		FROM [dbo].[Users]
		WHERE Id = @Id

END