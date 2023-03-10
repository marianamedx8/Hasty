using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hasty.Models.Domain.Users
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mi { get; set; }
        public string AvatarUrl { get; set; }
        public string Email { get; set; }
        public List<LookUp> Roles { get; set; }
    }
}
