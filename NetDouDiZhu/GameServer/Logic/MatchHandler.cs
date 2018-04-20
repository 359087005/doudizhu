using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AhpilyServer;
using Protocol;
using GameServer.Cache.Match;
using GameServer.Cache;
using Protocol.Dto;
using GameServer.Model;

namespace GameServer.Logic
{
    /// <summary>
    /// 匹配逻辑层
    /// </summary>
    public class MatchHandler : IHandler
    {
        MatchCache matchCache = Caches.match;
        UserCache userCache = Caches.user;

        public void OnDisConnect(ClientPeer client)
        {
            if (!userCache.IsOnLine(client))
                return;
            int userId = userCache.GetIdByClient(client);
            if (matchCache.IsMatching(userId))
            {
                Leave(client);
            }
        }

        public void OnReceive(ClientPeer client, int subCode, object value)
        {
            switch (subCode)
            {
                case MatchCode.ENTER_CREQ:
                    Enter(client);
                    break;
                case MatchCode.LEAVE_CREQ:
                    Leave(client);
                    break;
                case MatchCode.READY_CREQ:
                    Ready(client);
                    break;
            }
        }

        /// <summary>
        /// 进入
        /// </summary>
        void Enter(ClientPeer client)
        {
            SingleExecute.Instance.Execute
                (
                delegate ()
                {
                    if (!userCache.IsOnLine(client))
                        return;
                    int userID = userCache.GetIdByClient(client);
                    //判断用户是否已经在用户匹配房间
                    if (matchCache.IsMatching(userID))
                    {
                        return;
                    }
                    //正常进入
                    MatchRoom room = matchCache.Enter(userID,client);
                    //广播给所有用户 有玩家进入  //参数，新进入的玩家ID  不用广播
                    UserModel model = userCache.GetModelByUserId(userID);
                    UserDto userDto = new UserDto(model.id,model.name,model.been,model.lv,model.exp,model.winCount,model.loseCount,model.runCount);
                    room.Brocast(OpCode.MATCH,MatchCode.ENTER_BRO, userDto, client);

                    //返回给当前client  房间的数据模型
                    MatchRoomDto dto = GetMatchRoomDto(room);
                    //给MatchRoomDto中的uidUserDtoDic<int ,userDto>  进行赋值
                    //那么需要获取UserModel  房间之中有多个用户
                    client.Send(OpCode.MATCH,MatchCode.ENTER_SRES,dto);

                }

               
                );
        }
        private MatchRoomDto GetMatchRoomDto(MatchRoom room)
        {
            MatchRoomDto dto = new MatchRoomDto();
            foreach (var uid in room.uIdClientDict.Keys)
            {
                UserModel model = userCache.GetModelByUserId(uid);
                UserDto userdto = new UserDto(model.id,model.name, model.been, model.lv, model.exp, model.winCount, model.loseCount, model.runCount);
                dto.uIdUserDtoDict.Add(uid, userdto);
            }
            dto.readyUIdList = room.readyIdList;

            return dto;
        }

        /// <summary>
        /// 离开
        /// </summary>
        /// <param name="client"></param>
        private void Leave(ClientPeer client)
        {
            SingleExecute.Instance.Execute
                (
                delegate ()
                {
                    if (!userCache.IsOnLine(client))
                        return;
                    int userId = userCache.GetIdByClient(client);
                    //用户 否 匹配
                    if (matchCache.IsMatching(userId)==false)
                    {
                        return;
                    }
                    //正常操作
                    MatchRoom room = matchCache.Leave(userId);
                    //广播给房间内  
                    room.Brocast(OpCode.MATCH,MatchCode.LEAVE_BRO,userId,client);
                }
                );
        }

        /// <summary>
        /// 准备
        /// </summary>
        /// <param name="client"></param>
        private void Ready(ClientPeer client)
        {
            SingleExecute.Instance.Execute(() =>
            {
                //安全校验
                if (!userCache.IsOnLine(client))
                    return;
                int userId = userCache.GetIdByClient(client);
                if (matchCache.IsMatching(userId) == false)
                {
                    return;
                }
                //玩家准备list添加
                MatchRoom room = matchCache.GetRoom(userId);
                room.Ready(userId);

                //每准备一个 判断一下是否全部准备
                if (room.IsReady())
                {
                    //开始进入战斗
                    //客户端群发进入战斗
                    //TODO
                    room.Brocast(OpCode.MATCH, MatchCode.START_BRO, null);
                    //销毁准备房间
                    matchCache.Destroy(room);
                }
            });
        }
    }
}
