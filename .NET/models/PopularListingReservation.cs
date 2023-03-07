using Sabio.Models.Domain.Listings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.ListingReservations
{
    public class PopularListingReservation
    {
        public int ListingId { get; set; }
        public int TotalRepetitions { get; set; }

        public string InternalName { get; set; }
    }
}
