using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.Identity.Service
{
    public class TestAuthCodeService : IAuthCodeService
    {
        public bool Validatae(string phone, string authCode)
        {
            return true;
        }
    }
}
