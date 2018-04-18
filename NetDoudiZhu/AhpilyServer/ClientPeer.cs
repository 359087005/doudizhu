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
        public ClientPeer()
        {
            this.receiveArgs = new SocketAsyncEventArgs();
            this.receiveArgs.SetBuffer(new byte[1024], 0, 1024);
            this.receiveArgs.UserToken = this;
            this.sendArgs = new SocketAsyncEventArgs();
           

            this.sendArgs.Completed += SendArgs_Completed;
        }

        public Socket clientSocket { get; set; }

        public delegate void SendDisConnect(ClientPeer client, string reason);

        public SendDisConnect sendDisConnected;
        #region 接收数据

        public delegate void ReceiveCompleted(ClientPeer client, SocketMsg msg);

        public ReceiveCompleted receiveCompleted;

        /// <summary>
        /// 一旦接收到数据 就缓存到缓存区
        /// </summary>
        private List<byte> dataCache = new List<byte>();

        /// <summary>
        /// 接收的异步套接字请求
        /// </summary>
        public SocketAsyncEventArgs receiveArgs { get; set; }


        /// <summary>
        /// 是否正在接收数据
        /// </summary>
        private bool isReceiveProcess = false;

        #endregion
        //方便添加和删除

        /// <summary>
        /// 自身处理数据包
        /// </summary>
        /// <param name="packet"></param>
        public void StartReceive(byte[] packet)
        {
            dataCache.AddRange(packet);
            if (!isReceiveProcess)
                ProcessReceive();
        }

        /// <summary>
        /// 处理接收的数据
        /// </summary>
        private void ProcessReceive()
        {
            isReceiveProcess = true;

            byte[] data = EncoderTool.DeconderPacket(ref dataCache);

            if (data == null)
            {
                isReceiveProcess = false;
                return;
            }

            //TODO 需要再次转成一个具体的类型供使用

            SocketMsg msg = EncoderTool.DeCodeMsg(data);

            //回调给上层
            if (receiveCompleted != null)
                receiveCompleted(this, msg);
            //尾递归 
            ProcessReceive();
        }


        #region 断开连接
        public void Disconnected()
        {
            //数据清空
            dataCache.Clear();
            isReceiveProcess = false;

            sendQueue.Clear();
            isSendProcess = false;

            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
            clientSocket = null;
        }
        #endregion
        /// <summary>
        /// 发送的消息的队列
        /// </summary>
        private Queue<byte[]> sendQueue  = new Queue<byte[]>();
        /// <summary>
        /// 发送的异步套接字对象
        /// </summary>
        private SocketAsyncEventArgs sendArgs;
        /// <summary>
        /// 是否正在发送中
        /// </summary>
        private bool isSendProcess = false;

        #region 发送数据
        /// <summary>
        /// 发送网络消息
        /// </summary>
        /// <param name="opCode"></param>
        /// <param name="subCode"></param>
        /// <param name="value"></param>
        public void Send(int opCode, int subCode, object value)
        {
            SocketMsg msg = new SocketMsg(opCode, subCode, value);
            byte[] data = EncoderTool.EncodeMsg(msg);
            byte[] packet = EncoderTool.EnconderPacket(data);

            sendQueue.Enqueue(packet);
            if (!isSendProcess)
                Send();
        }

        private void Send()
        {
            isSendProcess = true;
            //如果数据为0
            if (sendQueue.Count == 0)
            {
                isSendProcess = false;
                return;
            }
            //取出一条数据
            byte[] packet = sendQueue.Dequeue();
            //设置消息 发送的异步套接字 操作     的发送数据缓冲区
            sendArgs.SetBuffer(packet, 0, packet.Length);
            bool result = clientSocket.SendAsync(sendArgs);
            if (result == false)
            {
                SendComplete();
            }
        }
        /// <summary>
        /// 发送完成的时候调用
        /// </summary>
        /// <param name="e"></param>
        private void SendComplete()
        {
            //发送的是否正确
            if (sendArgs.SocketError != SocketError.Success)
            {
                //客户端断开连接
                sendDisConnected(this, sendArgs.SocketError.ToString());
            }
            else
            {
                Send();
            }
        }
        private void SendArgs_Completed(object sneder, SocketAsyncEventArgs e)
        {
            SendComplete();
        }
        #endregion

        #region 粘包拆包
        //一个完整的业务被TCP拆分成多个包发送 拆包
        //粘包 或者多个小包封装成一个大的数据包发送
        //解决方案：1，定长。2，简单文件协议。头尾加特殊字符。3，消息头消息尾。头：消息的长度 尾：具体的消息（使用第三种）
        void Test()
        {
            byte[] bt = Encoding.Default.GetBytes("12345");
            int length = bt.Length;
            byte[] bt1 = BitConverter.GetBytes(length);
            //bt1 + bt  即发送的消息
            //获取  前四个字节  即int length - 4 即消息
        }
        #endregion
    }
}
