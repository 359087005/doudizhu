using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AhpilyServer;

namespace GameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerPeer serverPeer = new ServerPeer();
            serverPeer.Start(8860,10);

            Console.ReadKey();
        }
    }
}
