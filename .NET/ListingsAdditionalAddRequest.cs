using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.ListingReservations
{
    public class ListingsAdditionalAddRequest
    {
        [Required]
        [Range(1, Int32.MaxValue)]
        public int ListingId { get; set; }
        public string AdditionalInfo { get; set; }
        public string HouseRules { get; set; }

    }
}
