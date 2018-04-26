using AhpilyServer.ConCurrent;
using Protocol.Dto.Fight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Cache.Fight
{
    /// <summary>
    /// 战斗缓存
    /// </summary>
    public class FightCache
    {
        /// <summary>
        /// 用户ID 对应的房间ID
        /// </summary>
        private Dictionary<int, int> uidRoomIdDict = new Dictionary<int, int>();

        /// <summary>
        /// 房间ID 对应的房间模型对象
        /// </summary>
        private Dictionary<int, FightRoom> idRoomDict = new Dictionary<int, FightRoom>();

        /// <summary>
        /// 重用房间队列
        /// </summary>
        private Queue<FightRoom> roomQueue = new Queue<FightRoom>();
        /// <summary>
        /// 房间ID
        /// </summary>
        private ConCurrentInt id = new ConCurrentInt(-1);

        /// <summary>
        /// 创建房间
        /// </summary>
        /// <returns></returns>
        public FightRoom CreatRoom(List<int> uidList)
        {
            FightRoom room = null;
            //检测是否有重用房间
            if (roomQueue.Count > 0)  
            {
                room = roomQueue.Dequeue();
                room.Init(uidList);
            }
            else
            {
                room = new FightRoom(id.Add_Get(),uidList);
            }
            //绑定映射关系
            foreach (int uid in uidList)
            {
                uidRoomIdDict.Add(uid,room.id);
            }
            idRoomDict.Add(room.id,room);
            return room;
        }

        /// <summary>
        /// 获取房间
        /// </summary>
        /// <param name="id">房间ID</param>
        /// <returns></returns>
        public FightRoom GetRoom(int id)
        {
            if (idRoomDict.ContainsKey(id) == false)
            {
                throw new Exception("查无此房");
            }

            return idRoomDict[id];
        }
        /// <summary>
        /// 根据用户Id  获取房间
        /// </summary>
        /// <param name="uId"></param>
        /// <returns></returns>
        public FightRoom GetRoomByUid(int uId)
        {
            if (uidRoomIdDict.ContainsKey(uId) == false)
            {
                throw new Exception("查无此人ID");
            }
            int roomId = uidRoomIdDict[uId];
            return GetRoom(roomId);
        }

        /// <summary>
        /// 销毁房间
        /// </summary>
        /// <param name="room"></param>
        public void Destroy(FightRoom room)
        {
            //移除映射
            idRoomDict.Remove(room.id);
            foreach (PlayerDto player in room.PlayerList)
            {
                uidRoomIdDict.Remove(player.id);
            }
            room.Mutiple = 1;
            room.PlayerList.Clear();
            room.TableCardList.Clear();
            room.LeaveUidList.Clear();
            room.roundModel.Init();
            room.cardModel.Init();

            roomQueue.Enqueue(room);
        }

        /// <summary>
        /// 判断是否在战斗房间
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool IsFightIng(int userId)
        {
            return uidRoomIdDict.ContainsKey(userId);
        }
    }
}
