USE [Hasty]
GO
/****** Object:  StoredProcedure [dbo].[Listings_Delete_ByIdV2]    Script Date: 3/7/2023 8:13:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Mariana Hernandez>
-- Create date: <2023-02-15T13:06>
-- Description:	<Listings Delete_ByIdV2>
-- Code Reviewer: Doupis

-- MODIFIED BY: 
-- MODIFIED DATE:
-- Code Reviewer:
-- Note:
-- =============================================

ALTER PROC [dbo].[Listings_Delete_ByIdV2]
	@ListingId INT

/*
----TEST CODE----
DECLARE @ListingId INT = 61

SELECT *
FROM dbo.Listings

EXECUTE [dbo].[Listings_Delete_ByIdV2]
		@ListingId

SELECT *
FROM dbo.Listings

*/

AS

BEGIN
	DELETE FROM dbo.ListingImages
	WHERE ListingId = @ListingId
	DELETE FROM dbo.ListingAmenities
	WHERE ListingId = @ListingId
	DELETE FROM dbo.ListingServices
	WHERE ListingId = @ListingId
	DELETE
	FROM dbo.Listings
	WHERE Id = @ListingId

END