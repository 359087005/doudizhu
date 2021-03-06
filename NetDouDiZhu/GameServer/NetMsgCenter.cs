﻿using AhpilyServer;
using GameServer.Logic;
using Protocol;

namespace GameServer
{
    /// <summary>
    /// 网络消息处理中心
    /// </summary>
    public class NetMsgCenter : IApplication
    {
        IHandler account = new AccountHandler();
        IHandler user = new UserHandler();
        MatchHandler match = new MatchHandler();
        IHandler chat = new ChatHandler();
        FightHandler fight = new FightHandler();


        public NetMsgCenter()
        {
            match.startFight += fight.StartFight;
        }

        public void DisConnected(ClientPeer client)
        {
            fight.OnDisConnect(client);
            chat.OnDisConnect(client);
            match.OnDisConnect(client);
            user.OnDisConnect(client);
            account.OnDisConnect(client);
        }

        public void OnReceive(ClientPeer client, SocketMsg msg)
        {
            switch (msg.opCode)
            {
                case OpCode.ACCOUNT:
                    account.OnReceive(client, msg.subCode, msg.value);
                    break;
                case OpCode.USER:
                    user.OnReceive(client,msg.subCode,msg.value);
                    break;
                case OpCode.MATCH:
                    match.OnReceive(client, msg.subCode, msg.value);
                    break;
                case OpCode.CHAT:
                    chat.OnReceive(client, msg.subCode, msg.value);
                    break;
                case OpCode.FIGHT:
                    fight.OnReceive(client, msg.subCode, msg.value);
                    break;
            }
        }
    }
}
