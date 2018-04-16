using System;
using AhpilyServer;


namespace GameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerPeer serverPeer = new ServerPeer();
            serverPeer.SetApplication(new NetMsgCenter());
            serverPeer.Start(8860, 10);

            Console.ReadKey();
        }
    }
}
