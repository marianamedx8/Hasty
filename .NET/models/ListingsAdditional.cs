using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.ListingReservations
{
    public class ListingsAdditional
    {
        public int ListingId { get; set; }
        public string AdditionalInfo { get; set; }
        public string HouseRules { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}
