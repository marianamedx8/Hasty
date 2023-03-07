using Sabio.Models;
using Sabio.Models.Domain.ListingReservations;
using Sabio.Models.Requests.ListingReservations;
using System;
using System.Collections.Generic;

namespace Sabio.Services.Interfaces
{
    public interface IListingReservationsService
    {
        
        Paged<ListingReservation> GetAllPaginated(int pageIndex, int pageSize);
        Paged<ListingReservation> GetByCreatedByPaginated(int pageIndex, int pageSize, int userid);
        ListingReservation GetById(int id);
        int Add(ListingReservationAddRequest model);
        void Update(ListingReservationAddRequest request, int id);
        void UpdateStatusById(ListingReservationUpdateStatus request, int id);
        void UpdateStatusByListingId(ListingReservationUpdateStatus request, int listingid);
        Paged<ListingReservation> GetByOwnerPaginated(int pageIndex, int pageSize, int userId);
        Paged<ListingReservation> GetByDateByOwner(int pageIndex, int pageSize, string selectDate, int userId);
        List<PopularListingReservation> GetPopularByOwner(int userId);
    }
}