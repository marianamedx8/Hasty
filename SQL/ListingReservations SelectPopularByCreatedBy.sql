USE [Hasty]
GO
/****** Object:  StoredProcedure [dbo].[ListingReservations_SelectPopularByCreatedBy]    Script Date: 3/7/2023 8:14:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: <Mariana Hernandez>
-- Create date: 2023-02-24T18:22
-- Description: ListingReservations SelectPopularByCreatedBy
-- Code Reviewer: Jailene Kelly

-- MODIFIED BY: 
-- MODIFIED DATE:
-- Code Reviewer:
-- Note:
-- =============================================
ALTER PROC [dbo].[ListingReservations_SelectPopularByCreatedBy]
			@CreatedBy int
AS
/*----------Test Code-------------------
	DECLARE @CreatedBy int = 120

	EXECUTE [dbo].[ListingReservations_SelectPopularByCreatedBy]
					@CreatedBy

	SELECT *
	FROM dbo.ListingReservations

*/

BEGIN
	
	SELECT	lr.ListingId, COUNT(lr.ListingId) AS TotalRepetitions
			,l.InternalName
	FROM dbo.ListingReservations AS lr 						
		INNER JOIN dbo.Listings AS l
		ON lr.ListingId = l.Id
	WHERE l.CreatedBy = @CreatedBy
	GROUP BY lr.ListingId,l.InternalName
	ORDER BY COUNT(lr.ListingId) DESC
			
				  

END 