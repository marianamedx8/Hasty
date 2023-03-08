using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hasty.Models.Domain.Users
{
    public class UserAuth
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<LookUp> Roles { get; set; }
    }
}
