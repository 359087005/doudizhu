using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AhpilyServer;
using Protocol;
using Protocol.Dto.Fight;
using GameServer.Cache;
using GameServer.Cache.Match;

namespace GameServer.Logic
{
    public class ChatHandler : IHandler
    {
        UserCache userCache = Caches.user;
        MatchCache matchCache = Caches.match;

        public void OnDisConnect(ClientPeer client)
        {
           
        }

        public void OnReceive(ClientPeer client, int subCode, object value)
        {
            switch (subCode)
            {
                case ChatCode.CREQ:
                    Console.WriteLine("发送的chatType是："+ (int)value);
                    ChatRequest(client,(int)value);
                    break;
            }
        }


        private void ChatRequest(ClientPeer client, int chatType)
        {
            //接收到的是类型
            //返回什么  ？  所以创建一个数据传输模型DTO

            //谁发的？
            //需要一个userId  所以获取userCache
            if (userCache.IsOnLine(client) == false) return;
            int userId = userCache.GetIdByClient(client);
            ChatDto chatDto = new ChatDto(userId,chatType);

            //发给谁?
            //通过matchCache 获取房间  通过房间告诉其他玩家(挨个告诉  全体广播)
            if (matchCache.IsMatching(userId))
            {
                MatchRoom mRoom = matchCache.GetRoom(userId);
                mRoom.Brocast(OpCode.CHAT, ChatCode.SRES, chatDto);
            }
            else if (false)
            {
                //在战斗房间内 
                //TODO
            }
        }

    }
}
