using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Enums
{
    public enum TokenType
    {
        NotSet = 0,
        NewUser = 1,
        ResetPassword = 2,
        BankAccount = 3,
        CreditCardAccount = 4,
        Facebook = 5,
        Instagram = 6,
        Twitter= 7,
    }
}
