using System;
using System.Collections.Generic;
using System.Text;

using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace AhpilyServer
{
    /// <summary>
    /// 这里并不是客户端 而是当客户端连接到服务器时  服务器保存的客户端连接的socket
    /// </summary>
   public class ClientPeer
    {
        private Socket clientSocket;

        public void  SetSocket(Socket socket)
        {
            clientSocket = socket;
        }

        #region 接收数据
        /// <summary>
        /// 一旦接收到数据 就缓存到缓存区
        /// </summary>
        private List<byte> dataCache = new List<byte>();

        #endregion
        //方便添加和删除

        #region 粘包拆包
        //一个完整的业务被TCP拆分成多个包发送 拆包
        //粘包 或者多个小包封装成一个大的数据包发送
        //解决方案：1，定长。2，简单文件协议。头尾加特殊字符。3，消息头消息尾。头：消息的长度 尾：具体的消息（使用第三种）
        void Test()
        {
            byte[] bt =  Encoding.Default.GetBytes("12345");
            int length = bt.Length;
            byte[] bt1 = BitConverter.GetBytes(length);
            //bt1 + bt  即发送的消息
            //获取  前四个字节  即int length - 4 即消息
        }
        #endregion
    }
}
