using AhpilyServer;
using AhpilyServer.ConCurrent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Cache.Match
{
    /// <summary>
    /// 匹配的缓存层
    /// </summary>
    public class MatchCache
    {
        /// <summary>
        /// 正在等待的用户ID  和 房间的ID
        /// </summary>
        private Dictionary<int, int> uIDRoomIdDict = new Dictionary<int, int>();
        /// <summary>
        /// 等待的房间id和房间数据模型
        /// </summary>
        private Dictionary<int, MatchRoom> roomIDModelDict = new Dictionary<int, MatchRoom>();
        /// <summary>
        /// 重用的房间队列
        /// </summary>
        private Queue<MatchRoom> roomQueue = new Queue<MatchRoom>();
        /// <summary>
        /// 房间的ID
        /// </summary>
        private ConCurrentInt id = new ConCurrentInt(-1);

        /// <summary>
        /// 进入匹配队列
        /// </summary>
        /// <returns></returns>
        public MatchRoom Enter(int userID,ClientPeer client)
        {
            //遍历一下等待的房间  有正在等待的  吧玩家加进去
            foreach (MatchRoom mr in roomIDModelDict.Values)
            {
                //如果房间满了
                if (mr.IsFull())
                    continue;
                //没满
                mr.EnterRoom(userID, client);
                uIDRoomIdDict.Add(userID, mr.id);
                return mr;
            }
            //没有等待的房间  看等待队列里是否有等待房间
            MatchRoom room = null;
            if (roomQueue.Count > 0)
            {
                room = roomQueue.Dequeue();
            }
            else
            {  //如果没有 就自己创建房间  并把这些房间加到等待房间字典中
                room = new MatchRoom(id.Add_Get());
            }
            room.EnterRoom(userID,client);
            roomIDModelDict.Add(room.id, room);
            uIDRoomIdDict.Add(userID, room.id);
            return room;
        }

        /// <summary>
        /// 离开匹配房间
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public MatchRoom Leave(int userID)
        {
            //根据用户ID 获取房间ID 
            int roomID = uIDRoomIdDict[userID];
            //根据房间ID  获取房间数据模型对象
            MatchRoom room = roomIDModelDict[roomID];
            room.LeaveRoom(userID);
            //   移除玩家等待房间字典映射
            uIDRoomIdDict.Remove(userID);
            //房间是否空
            if (room.IsEmpty())
            {
                roomIDModelDict.Remove(room.id);
                roomQueue.Enqueue(room);
            }
            return room;
        }

        /// <summary>
        /// 用户正在匹配
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool IsMatching(int userID)
        {
            return uIDRoomIdDict.ContainsKey(userID);
        }

        /// <summary>
        /// 获取玩家所在的等待房间
        /// </summary>
        /// <returns></returns>
        public MatchRoom GetRoom(int userId)
        {
            //根据用户ID 获取房间ID 
            int roomID = uIDRoomIdDict[userId];
            //根据房间ID  获取房间数据模型对象
            MatchRoom room = roomIDModelDict[roomID];
            return room;
        }
        /// <summary>
        /// 摧毁房间
        /// </summary>
        /// <param name="room"></param>
        public void Destroy(MatchRoom room)
        {
            roomIDModelDict.Remove(room.id);
            foreach (var userID in room.uIdClientDict.Keys)
            {
                uIDRoomIdDict.Remove(userID);
            }
            ///清空数据
            room.uIdClientDict.Clear();
            room.readyIdList.Clear();
            roomQueue.Enqueue(room);
        }
    }
}
