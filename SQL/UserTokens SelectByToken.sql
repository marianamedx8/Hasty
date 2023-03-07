USE [Hasty]
GO
/****** Object:  StoredProcedure [dbo].[TokenTypes_SelectAll]    Script Date: 3/7/2023 8:11:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: <Mariana Hernandez>
-- Create date: <01-25-2023T 13:51:00>
-- Description: UserTokens SelectByToken
-- Code Reviewer: Doupis

-- MODIFIED BY: 
-- MODIFIED DATE:
-- Code Reviewer:
-- Note:
-- =============================================

ALTER PROC [dbo].[TokenTypes_SelectAll]

/*---Test Code---

	EXECUTE [dbo].[TokenTypes_SelectAll]

*/

AS

BEGIN

	SELECT [Id]
			,[Name]
	FROM [dbo].[TokenTypes]

End


