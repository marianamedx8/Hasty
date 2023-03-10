USE [Hasty]
GO
/****** Object:  StoredProcedure [dbo].[UserTokens_Insert]    Script Date: 3/7/2023 8:07:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: <Mariana Hernandez>
-- Create date: <01-25-2023T 13:16:00>
-- Description: UserTokens Insert
-- Code Reviewer: Doupis

-- MODIFIED BY: 
-- MODIFIED DATE:
-- Code Reviewer:
-- Note:
-- =============================================

ALTER PROC [dbo].[UserTokens_Insert]
			@Token varchar(200)
			,@UserId int
			,@TokenType int

/*---Test Code---

	DECLARE @Token varchar(200)= NEWID()
			,@UserId int = 3
			,@TokenType int = 2

	EXECUTE dbo.UserTokens_Insert
			@Token
			,@UserId
			,@TokenType

	SELECT *
	FROM [dbo].[UserTokens]
	
	SELECT *
	FROM [dbo].[Users]
*/

AS

BEGIN

	INSERT INTO [dbo].[UserTokens]
			   ([Token]
			   ,[UserId]
			   ,[TokenType])
		 VALUES
			   (@Token
			   ,@UserId
			   ,@TokenType)

END