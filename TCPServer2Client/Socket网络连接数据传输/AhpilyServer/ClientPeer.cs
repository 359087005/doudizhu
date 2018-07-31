using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AhpilyServer
{
    public class ClientPeer
    {
        public Socket clientSocket { get; set; }

        public ClientPeer()
        {
            ReceiveArgs = new SocketAsyncEventArgs();
            ReceiveArgs.UserToken = this;
            sendArgs = new SocketAsyncEventArgs();
            this.sendArgs.Completed += SendArgs_Completed;
        }


        #region  接收数据

        public delegate void ReceiveCompleted(ClientPeer client, SocketMsg msg);
        public ReceiveCompleted receiveCompleted;

        private List<byte> dataCache = new List<byte>();

        //
        public SocketAsyncEventArgs ReceiveArgs { get; set; }

        /// <summary>
        /// 是否正在处理数据
        /// </summary>
        private bool isReceiveProcess = false;

        /// <summary>
        /// 自身处理数据包
        /// </summary>
        /// <param name="packet"></param>
        public void StartReceive(byte[] packet)
        {
            dataCache.AddRange(packet);
            if (!isReceiveProcess)
            {
                ProcessReceive();
            }
        }

        /// <summary>
        /// 处理接收的数据
        /// </summary>
        public void ProcessReceive()
        {
            isReceiveProcess = true;

            byte[] data = EncoderTool.DecoderPacket(ref dataCache);
            if (data == null)
            {
                isReceiveProcess = false;
                return;
            }

            SocketMsg msg = EncoderTool.DecodeMsg(data);
            //解析完毕回调给上层
            if (receiveCompleted != null)
                receiveCompleted(this, msg);

            ProcessReceive();
        }
        #endregion

        #region 断开连接
        public void DisConnect()
        {
            //数据清空
            dataCache.Clear();
            isReceiveProcess = false;
            //TODO 给发送数据哪里预留


            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
            clientSocket = null;
        }
        #endregion

        #region 发送数据
        /// <summary>
        /// 发送 消息队列
        /// </summary>
        private Queue<byte[]> sendQueue = new Queue<byte[]>();

        private bool isSendProcess = false;

        private SocketAsyncEventArgs sendArgs;
        //发送的时候断开连接的回调
        public delegate void SendDisConnect(ClientPeer client,string reason);

        public SendDisConnect snedDisconnect;

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
            byte[] packet = EncoderTool.EncoderPacket(data);

            sendQueue.Enqueue(packet);

            if (!isSendProcess)
            {
                ProcessSend();
            }
            //try
            //{ clientSocket.Send(packet); }
            //catch (Exception e)
            //{ Console.WriteLine(e.Message); }
        }

        private void send()
        {
            isSendProcess = true;

            if (sendQueue.Count == 0)
            {
                isSendProcess = false;
                return;
            }

            ///取出数据
            byte[] packet = sendQueue.Dequeue();

            //设置消息 发送的异步套接字  发送数据缓冲区

            sendArgs.SetBuffer(packet, 0, packet.Length);
            bool result = clientSocket.SendAsync(sendArgs);
            if (result == false)
            {
                ProcessSend();
            }
        }

        private void SendArgs_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProcessSend();
        }

        /// <summary>
        /// 当异步发送请求  发送完成时候调用
        /// </summary>
        private void ProcessSend()
        {
            //发送的有没有错误
            if (sendArgs.SocketError != SocketError.Success)
            {
                //发送出错（客户端断开连接）
                snedDisconnect(this,sendArgs.SocketError.ToString());
            }
            else
            {
                send();
            }
        }

        #endregion
    }
}
