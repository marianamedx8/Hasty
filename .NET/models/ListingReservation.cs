using Sabio.Models.Domain.Listings;
using Sabio.Models.Domain.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.ListingReservations
{
    public class ListingReservation
    {
        public int Id { get; set; }
        public Listing Listing { get; set; }
        public DateTime DateCheckIn { get; set; }
        public DateTime DateCheckOut { get; set; }
        public string ChargeId { get; set; }
        public LookUp ReservationStatus { get; set; }
        public BaseUser User { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }

}

