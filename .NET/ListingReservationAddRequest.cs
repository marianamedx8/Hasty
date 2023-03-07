using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.ListingReservations
{
    public class ListingReservationAddRequest
    {
        [Required]
        [Range(1, Int32.MaxValue)]
        public int ListingId { get; set; }
        
        [Required]
        public DateTime DateCheckIn { get; set; }

        [Required]
        public DateTime DateCheckOut { get; set; }
        
        [Required]
        [StringLength(200, MinimumLength = 1)]
        public string ChargeId { get; set; }
        
        [Required]
        [Range(1, Int32.MaxValue)]
        public int StatusId { get; set; }
        
        [Required]
        [Range(1, Int32.MaxValue)]
        public int UserId { get; set; }

    }
}
