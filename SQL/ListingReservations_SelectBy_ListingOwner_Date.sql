USE [Hasty]
GO
/****** Object:  StoredProcedure [dbo].[ListingReservations_SelectBy_ListingOwner_Date]    Script Date: 3/7/2023 8:13:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Mariana Hernandez
-- Create date: 2023-02-21
-- Description: ListingReservations_SelectBy_ListingOwner_Date
-- Code Reviewer: Doupis

-- MODIFIED BY: 
-- MODIFIED DATE:
-- Code Reviewer:
-- Note:
-- =============================================
ALTER PROC [dbo].[ListingReservations_SelectBy_ListingOwner_Date]
			 @PageIndex int
			,@PageSize int
			,@SelectDate date
			,@CreatedBy int
			

as
/*----------Test Code-------------------
	Declare @PageIndex int = 0
			,@PageSize int = 10
			,@SelectDate date = '2023-03-16'
			,@CreatedBy int = 120
			

	Execute [dbo].[ListingReservations_SelectBy_ListingOwner_Date]
			 @PageIndex, @PageSize, @SelectDate, @CreatedBy

	Select *
	From dbo.ListingReservations

*/

BEGIN
		Declare @offset int = @PageIndex * @PageSize		

	SELECT				lr.Id
						,l.Id as ListingId
						,l.InternalName
						,l.Title
						,l.ShortDescription
						,l.Description
						,l.BedRooms
						,l.Baths
						,hts.Id AS 'HousingType.Id'
						,hts.[Name] AS 'HousingType.Name'
						,ats.Id AS 'AccessType.Id'
						,ats.Name AS 'AccessType.Name'			
						,ListingServices = (
							SELECT avs.Id,
									avs.Name
							FROM dbo.ListingServices AS ls
							INNER JOIN dbo.AvailableServices AS avs
								ON ls.ServiceId = avs.Id
							WHERE l.Id = ls.ListingId
							FOR JSON AUTO
						)	   
						,ListingAmenities = (
							SELECT a.Id,
									a.Name
							FROM dbo.ListingAmenities AS lga
								INNER JOIN dbo.Amenities AS a
									ON lga.AmenityId = a.Id
							WHERE l.Id = lga.ListingId
							FOR JSON AUTO
						)								   
						,l.GuestCapacity
						,l.CostPerNight
						,l.CostPerWeek
						,l.CheckInTime
						,l.CheckOutTime
						,l.DaysAvailable
						,ln.Id AS LocationId
						,ln.LocationTypeId
						,ln.LineOne
						,ln.LineTwo
						,ln.City
						,s.Name AS State
						,ln.Zip
						,ln.Latitude
						,ln.Longitude
						,l.HasVerifiedOwnerShip
						,l.IsActive
						,files = (SELECT f.Id AS fileId,
										   f.FileTypeId,
										   f.Name,
										   f.Url
									FROM dbo.Files AS f
									INNER JOIN dbo.ListingImages AS li
										ON li.FileId = f.Id
									WHERE li.ListingId = l.Id
									FOR JSON AUTO
								   )
						,l.CreatedBy
						,l.DateCreated
						,l.DateModified			
					    ,lr.DateCheckIn
					    ,lr.DateCheckOut
					    ,lr.ChargeId
					    ,rs.Id as ReservationStatusId
					    ,rs.Name as ReservationStatusName
					    ,u.Id
					    ,u.FirstName
					    ,u.LastName
					    ,u.Mi
					    ,u.AvatarUrl
					    ,lr.DateCreated
					    ,lr.DateModified
						,TotalCount = COUNT(1) OVER()			  
				  FROM dbo.ListingReservations as lr 						
						INNER JOIN dbo.Listings AS l
							ON l.Id = lr.ListingId
						INNER JOIN dbo.AccessTypes AS ats
							ON l.AccessTypeId = ats.Id
						INNER JOIN dbo.HousingTypes AS hts
							ON l.HousingTypeId = hts.Id
						INNER JOIN dbo.Locations AS ln
							ON l.LocationId = ln.Id
						INNER JOIN dbo.States AS s
							ON ln.StateId = s.Id
						INNER JOIN dbo.ReservationStatus as rs
							ON rs.Id =  lr.StatusId
						INNER JOIN dbo.Users as u 
							ON u.Id = lr.UserId
				  WHERE (l.CreatedBy = @CreatedBy)
				  and lr.DateCheckIn = @SelectDate
						--AND  (lr.DateCheckIn >= DATEADD(MONTH, -1, @SelectDate) AND lr.DateCheckIn < DATEADD(DAY, 1, @SelectDate))
				  ORDER BY lr.DateCheckIn ASC
		OFFSET @offSet Rows
	Fetch Next @PageSize Rows ONLY
	
END