USE [Hasty]
GO
/****** Object:  StoredProcedure [dbo].[Users_UpdateConfirm]    Script Date: 3/7/2023 8:09:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: <Mariana Hernandez>
-- Create date: <01-25-2023T 11:29:00>
-- Description: Users UpdateConfirm
-- Code Reviewer: Doupis

-- MODIFIED BY: Mariana Hernandez
-- MODIFIED DATE: 2/3/23
-- Code Reviewer:Doupis
-- Note:
-- =============================================
ALTER PROC [dbo].[Users_UpdateConfirm]
		    @Token nvarchar(50)
			,@Email nvarchar(255)

/*---Test Code---

	DECLARE @Token nvarchar(50)='290bcf1a-e5d2-48e6-bb5d-2dffc485e0ea'

	DECLARE @Email nvarchar(255) = 'sara@dispostable.com'



	EXECUTE [dbo].[Users_UpdateConfirm]
			@Token
			,@Email


	SELECT *
	FROM [dbo].[Users]
	WHERE email = @Email

*/

AS 

BEGIN


	DECLARE @Tran nvarchar(50) = '_updateConfirmTran'
	BEGIN TRANSACTION @Tran
	DECLARE @datNow datetime2 = getutcdate()

	
	SELECT t.UserId
	FROM dbo.UserTokens AS t 
	WHERE exists (SELECT 1
					FROM dbo.Users AS u
					WHERE t.UserId = u.Id
					AND t.Token = @Token)

	UPDATE [dbo].[Users]
	   SET [isConfirmed] = 1
		  ,[StatusId]= 1
		  ,[DateModified] = @datNow
	 WHERE Email = @Email

	 EXECUTE [dbo].[UserToken_DeleteByToken] @Token
			

	COMMIT TRANSACTION @Tran
END