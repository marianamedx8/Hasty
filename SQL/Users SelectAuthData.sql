USE [Hasty]
GO
/****** Object:  StoredProcedure [dbo].[Users_Select_AuthData]    Script Date: 3/7/2023 8:10:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: <Mariana Hernandez>
-- Create date: <01-25-2023T 11:29:00>
-- Description: Users SelectAuthData
-- Code Reviewer: Doupis

-- MODIFIED BY: Mariana 
-- MODIFIED DATE: 2023-02-04T09:34
-- Code Reviewer:Doupis
-- Note:
-- =============================================

ALTER PROC [dbo].[Users_Select_AuthData]
			@Email nvarchar(255)


/*---Test Code---

	DECLARE @Email nvarchar(255) = 'hastyAdmin@dispostable.com'

	EXECUTE [dbo].[Users_Select_AuthData]
			@Email

	SELECT *
	FROM [dbo].[Users]

*/

AS

	IF EXISTS (SELECT 1
			   FROM dbo.Users
               WHERE Email = @Email 
		       AND isConfirmed = 1)
Begin

	SELECT	 u.Id
			,u.Email
			,u.Password
			,Roles =(SELECT r.Id AS Id
							,r.Name AS Name
					FROM dbo.Roles AS r inner join dbo.UserRoles AS ur
							ON	 r.Id = ur.RoleId
					WHERE u.Id = ur.UserId
					FOR JSON AUTO)
	FROM [dbo].[Users] as u 
	WHERE [Email]= @Email

END