USE [Hasty]
GO
/****** Object:  StoredProcedure [dbo].[Users_Insert]    Script Date: 3/7/2023 8:11:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: <Mariana Hernandez>
-- Create date: <01-25-2023T 11:29:00>
-- Description: Users Insert
-- Code Reviewer: Doupis

-- MODIFIED BY: 
-- MODIFIED DATE:
-- Code Reviewer:
-- Note:
-- =============================================

ALTER PROC [dbo].[Users_Insert]
			@Email nvarchar(255)
			,@FirstName nvarchar(100)
			,@LastName nvarchar(100)
			,@Mi nvarchar(2)
			,@AvatarUrl varchar(255)
			,@Password varchar(100)
			,@PasswordConfirm varchar(100)
			,@StatusId int
			,@Id int OUTPUT


/*------------Test Code----------

	DECLARE @Id int = 0

	DECLARE @Email nvarchar(255)='user1@email.com'
			,@FirstName nvarchar(100)='Tom'
			,@LastName nvarchar(100)='Jones'
			,@Mi nvarchar(2)='D.'
			,@AvatarUrl varchar(255)='avatar.jpeg'
			,@Password varchar(100)='1qaz!QAZ'
			,@PasswordConfirm varchar(100)='1qaz!QAZ'
			,@StatusId int=1

	EXECUTE [dbo].[Users_Insert] 
			@Email
			,@FirstName
			,@LastName
			,@Mi
			,@AvatarUrl
			,@Password
			,@PasswordConfirm
			,@StatusId
			,@Id OUTPUT

	SELECT *
	FROM [dbo].[Users]
	WHERE Id = @Id

*/

AS

BEGIN

	INSERT INTO [dbo].[Users]
				([Email]
				,[FirstName]
				,[LastName]
				,[Mi]
				,[AvatarUrl]
				,[Password]
				,[StatusId])
			VALUES
				(@Email
				,@FirstName
				,@LastName
				,@Mi
				,@AvatarUrl
				,@Password
				,@StatusId)

	SET @Id = SCOPE_IDENTITY()

END
