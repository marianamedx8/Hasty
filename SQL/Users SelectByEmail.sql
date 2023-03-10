USE [Hasty]
GO
/****** Object:  StoredProcedure [dbo].[Users_SelectPass_ByEmail]    Script Date: 3/7/2023 8:09:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: <Mariana Hernandez>
-- Create date: <01-25-2023T 11:29:00>
-- Description: Users SelectByEmail
-- Code Reviewer: Doupis

-- MODIFIED BY: 
-- MODIFIED DATE:
-- Code Reviewer:
-- Note:
-- =============================================

ALTER PROC [dbo].[Users_SelectPass_ByEmail]
	@Email nvarchar(255)

/*---Test Code---

	DECLARE @Email nvarchar(255)='user@email.com'

	EXECUTE [dbo].[Users_SelectPass_ByEmail] @Email

*/

AS

BEGIN

	SELECT Password
	FROM [dbo].[Users] 
	Where Email = @Email

END