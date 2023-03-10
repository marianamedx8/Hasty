USE [Hasty]
GO
/****** Object:  StoredProcedure [dbo].[Users_UpdateStatus]    Script Date: 3/7/2023 8:07:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: <Mariana Hernandez>
-- Create date: <01-25-2023T 11:29:00>
-- Description: Users UpdateStatus
-- Code Reviewer: Doupis

-- MODIFIED BY: 
-- MODIFIED DATE:
-- Code Reviewer:
-- Note:
-- =============================================

ALTER PROC [dbo].[Users_UpdateStatus]
			 @StatusId int
			,@Id int

/*---Test Code---

	DECLARE @Id int=7

	DECLARE @StatusId int=1

	SELECT *
	FROM [dbo].[Users]
	WHERE Id = @Id

	EXECUTE [dbo].[Users_UpdateStatus] 
			@StatusId
			,@Id

	SELECT *
	FROM [dbo].[Users]
	WHERE Id = @Id

*/

AS

BEGIN
	
	DECLARE @datNow datetime2 = GETUTCDATE()

	UPDATE [dbo].[Users]
	SET [StatusId] = @StatusId
		,[DateModified] = @datNow
	WHERE Id = @Id

END