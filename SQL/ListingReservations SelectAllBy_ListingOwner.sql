USE [Hasty]
GO
/****** Object:  StoredProcedure [dbo].[ListingReservations_SelectAllBy_ListingOwner]    Script Date: 3/7/2023 8:13:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Mariana Hernandez 
-- Create date: 2023-02-17T19:27
-- Description: ListingReservations SelectAllBy_ListingOwner
-- Code Reviewer: Doupis

-- MODIFIED BY: 
-- MODIFIED DATE:
-- Code Reviewer:
-- Note:
-- =============================================
ALTER proc [dbo].[ListingReservations_SelectAllBy_ListingOwner]
		@CreatedBy int

as
/*----------Test Code-------------------
	Declare @CreatedBy int = 120

	Execute [dbo].[ListingReservations_SelectAllBy_ListingOwner]
			@CreatedBy

	Select *
	From dbo.ListingReservations

*/

BEGIN

				
				SELECT	LR.[Id]
						,l.Id as ListingId,
						l.InternalName,
						l.Title,
						l.ShortDescription,
						l.Description,
						l.BedRooms,
						l.Baths,
						hts.Id AS 'HousingType.Id',
						hts.[Name] AS 'HousingType.Name',
						ats.Id AS 'AccessType.Id',
						ats.Name AS 'AccessType.Name',
									
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
						l.CreatedBy,
						l.DateCreated,
						l.DateModified
					  ,LR.[DateCheckIn]
					  ,LR.[DateCheckOut]
					  ,LR.[ChargeId]
					  ,RS.[Id] as ReservationStatusId
					  ,RS.[Name] as ReservationStatusName
					  ,USR.Id
					  ,USR.FirstName
					  ,USR.LastName
					  ,USR.Mi
					  ,USR.AvatarUrl
					  ,LR.[DateCreated]
					  ,LR.[DateModified]
				  

				  FROM dbo.Listings AS l
						INNER JOIN dbo.AccessTypes AS ats
							ON l.AccessTypeId = ats.Id
						INNER JOIN dbo.HousingTypes AS hts
							ON l.HousingTypeId = hts.Id
						INNER JOIN dbo.Locations AS ln
							ON l.LocationId = ln.Id
						INNER JOIN dbo.States AS s
							ON ln.StateId = s.Id
						INNER JOIN dbo.ListingReservations as lr
							ON l.Id = lr.ListingId
						INNER JOIN dbo.ReservationStatus as rs
							ON rs.Id =  lr.StatusId
						INNER JOIN dbo.Users as usr 
							ON usr.Id = lr.UserId
				  WHERE l.CreatedBy = @CreatedBy
				  ORDER BY l.CreatedBy


END