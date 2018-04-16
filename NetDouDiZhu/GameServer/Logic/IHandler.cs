using AhpilyServer;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Logic
{
    public interface IHandler
    {
        void OnReceive(ClientPeer client, int subCode, object value);

        void OnDisConnect(ClientPeer client);
    }
}
