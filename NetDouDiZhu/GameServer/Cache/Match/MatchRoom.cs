using AhpilyServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Cache.Match
{
    /// <summary>
    /// 匹配房间
    /// </summary>
    public class MatchRoom
    {
        public int id;
        /// <summary>
        /// 房间内的用户列表
        /// </summary>
        public Dictionary<int,ClientPeer> uIdClientDict ;
        /// <summary>
        /// 已准备的用户列表
        /// </summary>
        public List<int> readyIdList;

        public MatchRoom(int id)
        {
            this.id = id;
            uIdClientDict = new Dictionary<int,ClientPeer>();
            readyIdList = new List<int>();
        }
        /// <summary>
        /// 房间是否满了
        /// </summary>
        /// <returns></returns>
        public bool IsFull()
        {
            return uIdClientDict.Count == 3;
        }
        /// <summary>
        /// 房间是否空了
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return uIdClientDict.Count == 0;
        }

        /// <summary>
        /// 是否全部准备
        /// </summary>
        /// <returns></returns>
        public bool IsReady()
        {
            return readyIdList.Count == 3;
        }
        /// <summary>
        /// 进入房间
        /// </summary>
        /// <param name="id"></param>
        public void EnterRoom(int userId,ClientPeer client)
        {
            uIdClientDict.Add(id, client);
        }
        /// <summary>
        /// 离开房间   
        /// </summary>
        /// <param name="userId"></param>
        public void LeaveRoom(int userId)
        {
            uIdClientDict.Remove(userId);
        }
        /// <summary>
        /// 准备
        /// </summary>
        public void Ready(int userId)
        {
            readyIdList.Add(userId);
        }
        /// <summary>
        /// 取消准备
        /// </summary>
        /// <param name="userId"></param>
        public void CancleReady(int userId)
        {
            readyIdList.Remove(userId);
        }
        /// <summary>
        /// 广播房间内的玩家这条消息
        /// </summary>
        /// <param name="opCode"></param>
        /// <param name="subCode"></param>
        /// <param name="value"></param>
        public void Brocast(int opCode, int subCode, object value,ClientPeer exClient = null)
        {
            SocketMsg msg = new SocketMsg(opCode, subCode, value);
            byte[] data = EncoderTool.EncodeMsg(msg);
            byte[] packet = EncoderTool.EnconderPacket(data);
            foreach (var client in uIdClientDict.Values)
            {
                if (client == exClient)
                    continue;
                client.Send(packet);
            }
        }
    }
}
