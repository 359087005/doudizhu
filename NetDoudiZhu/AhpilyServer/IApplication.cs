using System;
using System.Collections.Generic;
using System.Text;

namespace AhpilyServer
{
   public interface IApplication
    {
        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="client"></param>
        void DisConnected(ClientPeer client);

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="client"></param>
        /// <param name="msg"></param>
        void OnReceive(ClientPeer client, SocketMsg msg);
    }
}
