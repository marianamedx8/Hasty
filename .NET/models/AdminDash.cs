using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hasty.Models.Domain.Users
{
    public class AdminDash
    {
        public List<UserStatus> Status { get; set; }
        public int UserCount { get; set; }
        public int ActiveSubscriptions { get; set; }
        public List<Subs> Income { get; set; }
        public List<SubsPlans> ByWeek { get; set; }        
        public List<SubsPlans> ByYear { get; set; }
        public List<LookUp>ByMonth { get; set; }

    }
}
