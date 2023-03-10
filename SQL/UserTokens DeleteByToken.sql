USE [Hasty]
GO
/****** Object:  StoredProcedure [dbo].[UserToken_DeleteByToken]    Script Date: 3/7/2023 8:07:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: <Mariana Hernandez>
-- Create date: <1/25/2023 1:38:42 PM>
-- Description: UserTokens DeleteByToken
-- Code Reviewer: Doupis

-- MODIFIED BY: 
-- MODIFIED DATE:
-- Code Reviewer:
-- Note:
-- =============================================
ALTER PROC [dbo].[UserToken_DeleteByToken]
			@Token varchar(200)

/*---Test Code---

	DECLARE @Token varchar(200) = 'AB0C99E3-3759-4928-9704-025D4A889EA1'

	SELECT *
	FROM [dbo].[UserTokens]
	WHERE Token = @Token 

	EXECUTE [dbo].[UserToken_DeleteByToken] @Token

	SELECT *
	FROM [dbo].[UserTokens]
	WHERE Token = @Token 

*/

AS

BEGIN

	DELETE FROM [dbo].[UserTokens]
	WHERE Token = @Token

END