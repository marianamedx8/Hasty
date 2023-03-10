USE [Hasty]
GO
/****** Object:  StoredProcedure [dbo].[Listings_Select_ByCreatedBy_PaginatedV2_isActive]    Script Date: 3/7/2023 8:12:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Mariana Hernandez>
-- Create date: <2023-02-16T14:52>
-- Description:	<Listings Select_ByCreatedBy_PaginatedV2>
-- Code Reviewer: Doupis

-- MODIFIED BY: 
-- MODIFIED DATE:
-- Code Reviewer:
-- Note:
-- =============================================

ALTER PROC [dbo].[Listings_Select_ByCreatedBy_PaginatedV2_isActive]
	 @PageIndex INT
	,@PageSize INT
	,@CreatedBy INT
	,@isActive bit


/*
----TEST CODE----
DECLARE @PageIndex INT = 0
	   ,@PageSize INT = 4
	   ,@CreatedBy INT = 120
	   ,@isActive bit = 1

EXECUTE dbo.Listings_Select_ByCreatedBy_PaginatedV2
		 @PageIndex
		,@PageSize
		,@CreatedBy
		,@isActive
*/

AS

BEGIN
	DECLARE @Offset INT = @PageIndex * @PageSize

	SELECT l.Id,
		   l.InternalName,
		   l.Title,
		   l.ShortDescription,
		   l.Description,
		   l.BedRooms,
		   l.Baths,
		   hts.Id AS HousingTypeId,
		   hts.Name,
		   ats.Id AS AccessTypeId,
		   ats.Name,
		   ListingServices = (
			SELECT avs.Id,
				   avs.Name
			FROM dbo.ListingServices AS ls
			INNER JOIN dbo.AvailableServices AS avs
				ON ls.ServiceId = avs.Id
			WHERE l.Id = ls.ListingId
			FOR JSON AUTO
		   ),
		   ListingAmenities = (
			SELECT a.Id,
				   a.Name
			FROM dbo.ListingAmenities AS lga
			INNER JOIN dbo.Amenities AS a
				ON lga.AmenityId = a.Id
			WHERE l.Id = lga.ListingId
			FOR JSON AUTO
		   ),
		   l.GuestCapacity,
		   l.CostPerNight,
		   l.CostPerWeek,
		   l.CheckInTime,
		   l.CheckOutTime,
		   l.DaysAvailable,
		   ln.Id AS LocationId,
		   ln.LocationTypeId,
		   ln.LineOne,
		   ln.LineTwo,
		   ln.City,
		   s.Name AS State,
		   ln.Zip,
		   ln.Latitude,
		   ln.Longitude,
		   l.HasVerifiedOwnerShip,
		   l.IsActive,
		   		   files = (
			SELECT f.Id AS fileId,
				   f.FileTypeId,
				   f.Name,
				   f.Url
			FROM dbo.Files AS f
			INNER JOIN dbo.ListingImages AS li
				ON li.FileId = f.Id
			WHERE li.ListingId = l.Id
			FOR JSON AUTO
		   ),
		   l.CreatedBy,
		   l.DateCreated,
		   l.DateModified,
		   TotalCount = COUNT(1) OVER()
	FROM dbo.Listings AS l
	INNER JOIN dbo.AccessTypes AS ats
		ON l.AccessTypeId = ats.Id
	INNER JOIN dbo.HousingTypes AS hts
		ON l.HousingTypeId = hts.Id
	INNER JOIN dbo.Locations AS ln
		ON l.LocationId = ln.Id
	INNER JOIN dbo.States AS s
		ON ln.StateId = s.Id
	WHERE l.CreatedBy = @CreatedBy
	AND l.IsActive = @isActive
	ORDER BY l.CreatedBy

	OFFSET @Offset ROWS
	FETCH NEXT @PageSize ROWS ONLY
END