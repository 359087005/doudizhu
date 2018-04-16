using System;
using System.Collections.Generic;
using System.Text;

using System.Threading.Tasks;

namespace AhpilyServer
{
    /// <summary>
    /// 客户端的连接池
    ///     作用：重用客户端的连接对象
    /// </summary>
    public class ClientPeerPool
    {
        private Queue<ClientPeer> clientQueue;

        public ClientPeerPool(int capacity)
        {
            clientQueue = new Queue<ClientPeer>(capacity);
        }

        /// <summary>
        /// 在队列末尾添加一个元素
        /// </summary>
        /// <param name="clientPeer"></param>
        public void Enqueue(ClientPeer clientPeer)
        {
            clientQueue.Enqueue(clientPeer);
        }
        /// <summary>
        /// 删除并返回队列第一个元素  与peek类似
        /// </summary>
        public ClientPeer Dequeue()
        {
            return clientQueue.Dequeue();
        }
    }
}
