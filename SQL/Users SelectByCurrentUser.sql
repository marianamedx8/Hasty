USE [Hasty]
GO
/****** Object:  StoredProcedure [dbo].[Users_SelectByCurrentUser]    Script Date: 3/7/2023 8:10:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: <Mariana Hernandez>
-- Create date:  2023-02-07T18:29
-- Description: Users SelectByCurrentUser
-- Code Reviewer: James Harvey

-- MODIFIED BY: 
-- MODIFIED DATE:
-- Code Reviewer:
-- Note:
-- =============================================

ALTER PROC [dbo].[Users_SelectByCurrentUser]
			@Id int

/*---Test Code---

	DECLARE @Id int=71

	EXECUTE [dbo].[Users_SelectByCurrentUser] @Id

*/

AS

BEGIN

	SELECT	 u.Id
			,u.FirstName
			,u.LastName
			,u.Mi
			,u.AvatarUrl
			,u.Email
			,Roles =(SELECT r.Id AS Id
							,r.Name AS Name
					FROM dbo.Roles AS r inner join dbo.UserRoles AS ur
							ON	 r.Id = ur.RoleId
					WHERE u.Id = ur.UserId
					FOR JSON AUTO)
	FROM [dbo].[Users] as u 
		WHERE Id = @Id

END
