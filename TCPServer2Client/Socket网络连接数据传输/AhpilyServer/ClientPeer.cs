using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AhpilyServer
{
  public  class ClientPeer
    {
        public Socket clientSocket { get; set; }

        #region  接收数据

        public delegate void ReceiveCompleted(ClientPeer client,object value);
        public ReceiveCompleted receiveCompleted;

        private List<byte> dataCache = new List<byte>();

        //
        public SocketAsyncEventArgs receiveArgs { get; set; }

        /// <summary>
        /// 是否正在处理数据
        /// </summary>
        private bool isProcess = false;

        /// <summary>
        /// 自身处理数据包
        /// </summary>
        /// <param name="packet"></param>
        public void StartReceive(byte[] packet)
        {
            dataCache.AddRange(packet);
            if (!isProcess)
            {
                ProcessReceive();
            }
        }

        /// <summary>
        /// 处理接收的数据
        /// </summary>
        public void ProcessReceive()
        {
            isProcess = true;

           byte[] data =  EncoderTool.DecoderPacket(ref dataCache);
            if (data == null)
            {
                isProcess = false;
                return;
            }
            //TODO 需要再次转换成一个具体的类型
            object value = data;
            //解析完毕回调给上层
            if (receiveCompleted != null)
                receiveCompleted(this,value);

            ProcessReceive();
        }
        #endregion
    }
}
