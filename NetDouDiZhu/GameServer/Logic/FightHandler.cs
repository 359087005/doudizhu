using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AhpilyServer;
using GameServer.Cache.Fight;
using GameServer.Cache;
using Protocol;
using Protocol.Dto.Fight;
using GameServer.Model;

namespace GameServer.Logic
{
    public class FightHandler : IHandler
    {
        FightCache fight = Caches.fight;
        UserCache user = Caches.user;

        public void OnDisConnect(ClientPeer client)
        {
            Leave(client);
        }

        public void OnReceive(ClientPeer client, int subCode, object value)
        {
            switch (subCode)
            {
                case FightCode.GRAB_LANDLORD_CREQ:
                    bool result = (bool)value;  //是否抢地主
                    GrabLandLord(client, result);
                    break;
                case FightCode.DEAL_CREQ:
                    Deal(client, value as DealDto);
                    break;
                case FightCode.PASS_CREQ:
                    Pass(client);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 用户离开
        /// </summary>
        /// <param name="client"></param>
        private void Leave(ClientPeer client)
        {
            SingleExecute.Instance.Execute(
                () =>
                {
                    //玩家在线
                    if (user.IsOnLine(client) == false) return;

                    int userId = user.GetIdByClient(client);
                    if (fight.IsFightIng(userId) == false)
                    {
                        return;
                    }
                    ///TODO  玩家离开导致报错
                    FightRoom room = fight.GetRoomByUid(userId);

                    room.LeaveUidList.Add(userId);
                    Brocast(room,OpCode.FIGHT,FightCode.LEAVE_BRO,userId);

                    //全跑了
                    if (room.LeaveUidList.Count == 3)
                    {
                        for (int i = 0; i < room.LeaveUidList.Count; i++)
                        {
                            UserModel model = user.GetModelByUserId(room.LeaveUidList[i]);
                            model.runCount++;
                            model.been -= (room.Mutiple * 1000) * 3;
                            model.exp += 0;
                            user.Update(model);
                        }
                        fight.Destroy(room);
                    }
                }
                );
        }



        /// <summary>
        /// 不出的处理
        /// </summary>
        /// <param name="client"></param>
        private void Pass(ClientPeer client)
        {
            SingleExecute.Instance.Execute(
               delegate ()
               {
                   //玩家在线
                   if (user.IsOnLine(client) == false) return;

                   int userId = user.GetIdByClient(client);
                   FightRoom room = fight.GetRoom(userId);

                   //2中情况  

                   if (room.roundModel.BiggestUid == userId)
                   {
                       //我是最大的  我必须得出
                       client.Send(OpCode.FIGHT, FightCode.PASS_SRES, -1);
                       return;
                   }
                   else
                   {//可以不出 
                       client.Send(OpCode.FIGHT, FightCode.PASS_SRES, 0);
                       Turn(room);
                   }
               }
               );
        }


        /// <summary>
        /// 出牌的处理
        /// </summary>
        private void Deal(ClientPeer client, DealDto dto)
        {
            SingleExecute.Instance.Execute(
                delegate ()
                {
                    //玩家在线
                    if (user.IsOnLine(client) == false) return;

                    int userId = user.GetIdByClient(client);

                    if (userId != dto.userId)  //DTO的ID可以用来做验证。
                    {
                        return;
                    }
                    FightRoom room = fight.GetRoom(userId);
                    //玩家出牌
                    //掉线  在线
                    if (room.LeaveUidList.Contains(userId))
                    {
                        //自动出牌
                        Turn(room);
                    }
                    //玩家在线
                    bool canDeal = room.DealCard(dto.length, dto.type, dto.weight, userId, dto.selectCardList);
                    if (canDeal == false)
                    {
                        //压不住
                        client.Send(OpCode.FIGHT, FightCode.DEAL_SRES, -1);
                        return;
                    }
                    else
                    {
                        //发送给出牌者
                        client.Send(OpCode.FIGHT, FightCode.DEAL_SRES, 0);
                        List<CardDto> remainCardList = room.GetPlayerCard(userId);
                        dto.remainCardList = remainCardList;
                        //广播
                        Brocast(room, OpCode.FIGHT, FightCode.DEAL_BRO, dto);
                        //检测下 剩余牌  为0 就赢了
                        if (remainCardList.Count == 0)
                        {
                            //游戏结束
                            GameOver(userId, room);
                        }
                        else
                        {
                            //转换玩家
                            Turn(room);
                        }
                    }

                }
                );
        }

        /// <summary>
        /// 游戏结束
        /// </summary>
        private void GameOver(int userId, FightRoom room)
        {
            //确定身份 是农民还是地主 
            int identity = room.GetPlayerIdentity(userId);
            List<int> winUids = room.GetSameIdentityUids(identity);
            int winBeen = room.Mutiple * 1000;

            //给胜利的玩家  胜场   豆子  经验  等级..之后玩家数据模型刷新
            for (int i = 0; i < winUids.Count; i++)
            {
                UserModel model = user.GetModelByUserId(winUids[i]);
                model.winCount++;
                model.been += winBeen;
                model.exp += 100;
                int maxExp = model.lv * 100;
                while (maxExp <= model.exp)
                {
                    model.lv++;
                    model.exp -= maxExp;
                    maxExp = model.lv * 100;
                }
                user.Update(model);
            }

            //给失败的玩家  败场   豆子  经验  等级..之后玩家数据模型刷新
            List<int> loseUids = room.GetDifIdentityUids(identity);
            for (int i = 0; i < winUids.Count; i++)
            {
                UserModel model = user.GetModelByUserId(loseUids[i]);
                model.loseCount++;
                model.been -= winBeen;
                model.exp += 10;
                int maxExp = model.lv * 100; ;
                while (maxExp <= model.exp)
                {
                    model.lv++;
                    model.exp -= maxExp;
                    maxExp = model.lv * 100;
                }
                user.Update(model);
            }
            //给逃跑的玩家  逃跑   豆子  经验  等级..之后玩家数据模型刷新
            for (int i = 0; i < room.LeaveUidList.Count; i++)
            {
                UserModel model = user.GetModelByUserId(room.LeaveUidList[i]);
                model.runCount++;
                model.been -= (winBeen) * 3;
                model.exp += 0;
                user.Update(model);
            }
            //给客户端发消息  谁赢了。身份，豆子多少
            OverDto dto = new OverDto();
            dto.winIdentity = identity; dto.winUidList = winUids; dto.beenCount = winBeen;

            Brocast(room, OpCode.FIGHT, FightCode.OVER_BRO, dto);

            fight.Destroy(room);
        }

        /// <summary>
        /// 转换出牌
        /// </summary>
        /// <param name="room"></param>
        private void Turn(FightRoom room)
        {
            int nextUid = room.Turn();
            //如果掉线了
            if (room.isOffline(nextUid))
            {
                //或者执行出牌AI  TODO.......
                Turn(room);
            }
            else
            {
                //玩家没掉线  发消息  出牌
                ClientPeer client = user.GetClientById(nextUid);
                client.Send(OpCode.FIGHT, FightCode.TURN_DEAL_BRO, nextUid);
            }

        }

        /// <summary>
        /// 抢地主处理
        /// </summary>
        private void GrabLandLord(ClientPeer client, bool result)
        {
            SingleExecute.Instance.Execute(
            delegate ()
            {
                if (user.IsOnLine(client) == false) return;

                int userId = user.GetIdByClient(client);
                FightRoom room = fight.GetRoomByUid(userId);
                if (result == true)
                {
                    //抢
                    room.SetLandlord(userId);
                    GrabDto grab = new GrabDto(userId, room.TableCardList,room.GetPlayerCard(userId));
                    //广播  谁是地主  三张底牌
                    Brocast(room, OpCode.FIGHT, FightCode.GRAB_LANDLORD_BRO, grab);
                    //发送一个出牌的命令
                    Brocast(room,OpCode.FIGHT,FightCode.TURN_DEAL_BRO,userId);
                }
                else
                {
                    //不抢
                    int nextUid = room.GetNextUid(userId);
                    Brocast(room, OpCode.FIGHT, FightCode.TURN_GRAB__BRO, nextUid);
                }

            });

        }

        /// <summary>
        /// 开始战斗
        /// </summary>
        /// <param name="uIdList"></param>
        public void StartFight(List<int> uIdList)
        {
            SingleExecute.Instance.Execute
                (
                delegate ()
                {
                    //创建战斗房间
                    FightRoom room = fight.CreatRoom(uIdList);
                    //创建牌  并排序
                    room.InitPlayerCards();
                    room.Sort();
                    //发送给每个客户端 自身有什么牌
                    foreach (int uId in uIdList)
                    {
                        ClientPeer client = user.GetClientById(uId);
                        List<CardDto> cardList = room.GetPlayerCard(uId);
                        client.Send(OpCode.FIGHT, FightCode.GET_CARD_SRES, cardList);
                    }
                    //发送开始抢地主响应
                    int firstUserId = room.GetFirstUid();
                    Brocast(room, OpCode.FIGHT, FightCode.TURN_GRAB__BRO, firstUserId, null);
                }
                );
        }

        /// <summary>
        /// 广播
        /// </summary>
        private void Brocast(FightRoom room, int opCode, int subCode, object value, ClientPeer exClient = null)
        {
            SocketMsg msg = new SocketMsg(opCode, subCode, value);
            byte[] data = EncoderTool.EncodeMsg(msg);
            byte[] packet = EncoderTool.EnconderPacket(data);

            foreach (PlayerDto player in room.PlayerList)
            {
                if (user.IsOnLine(player.id))  //425
                {
                    ClientPeer client = user.GetClientById(player.id);
                    if (client == exClient)
                        continue;
                    client.Send(packet);
                }
            }
        }
    }
}
