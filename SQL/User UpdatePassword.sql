USE [Hasty]
GO
/****** Object:  StoredProcedure [dbo].[Users_UpdatePassword]    Script Date: 3/7/2023 8:08:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: <Mariana Hernandez>
-- Create date:  2023-02-08T20:16
-- Description: User UpdatePassword
-- Code Reviewer: Ryan Mangum

-- MODIFIED BY: 
-- MODIFIED DATE:
-- Code Reviewer:
-- Note:
-- =============================================
ALTER PROC [dbo].[Users_UpdatePassword]
		    @Token nvarchar(50)
			,@Email nvarchar(255)
			,@Password varchar(100)
			,@PasswordConfirm varchar(100)

/*---Test Code---

	DECLARE @Token nvarchar(50)='4bc11eb8-f6f4-4bc1-b909-a497d4138bbf'
		   ,@Email nvarchar(255) = 'tara@dispostable.com'
		   ,@Password varchar(100) = 'Password1!'
		   ,@PasswordConfirm varchar(100)='Password1!'



	EXECUTE [dbo].[Users_UpdatePassword]
			@Token
			,@Email
			,@Password


	SELECT *
	FROM [dbo].[Users]
	WHERE email = @Email

*/

AS 

BEGIN

	DECLARE @datNow datetime2 = getutcdate();

	WITH UserToken AS (SELECT u.Email
							,t.Token
							,t.UserId
						FROM dbo.UserTokens AS t join dbo.Users AS u
							ON t.UserId = u.Id
						WHERE t.Token = @Token and @Email = u.email)

	UPDATE [dbo].[Users]
	   SET [Password] = @Password
		  ,[DateModified] = @datNow
	 WHERE Id = (SELECT UserId FROM UserToken)

	 EXECUTE [dbo].[UserToken_DeleteByToken] @Token

END