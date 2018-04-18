using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Cache
{
    public class Caches
    {
        public static AccountCache account { get; set; }

        static Caches()
        {
            account = new AccountCache();
        }
    }
}
