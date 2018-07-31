using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AhpilyServer
{
    /// <summary>
    /// 服务器类
    /// </summary>
    public class ServerPeer
    {
        private Socket serverSocket;
        /// <summary>
        /// 限制客户端连接数量的信号量
        /// </summary>
        private Semaphore acceptSemaphore;

        private ClientPeerPool clientPeerPool;


        public ServerPeer()
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        /// <summary>
        /// 用来开启服务器
        /// </summary>
        /// <param name="port"></param>
        /// <param name="maxCount"></param>
        public void Start(int port, int maxCount)
        {
            try
            {
                acceptSemaphore = new Semaphore(maxCount, maxCount);

                serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
                serverSocket.Listen(maxCount);
                clientPeerPool = new ClientPeerPool(maxCount);
                ClientPeer tmpClientPeer = null;
                for (int i = 0; i < maxCount; i++)
                {
                    tmpClientPeer = new ClientPeer();
                    
                    tmpClientPeer.ReceiveArgs.Completed += Receive_Completed;
                    tmpClientPeer.snedDisconnect = DisConnect;
                    tmpClientPeer.receiveCompleted = ReceiveCompleted;
                    clientPeerPool.EnQueue(tmpClientPeer);
                }

                StartAccept(null);
                Console.WriteLine("服务器启动成功。。。");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        #region 接收连接
        /// <summary>
        ///  异步开始接收 ，2种方式。serverSocket.AcceptAsync(e);   和serverSocket的beginAccept 方式。
        ///  第一种效率高。
        ///  serverSocket.AcceptAsync(e); 需要参数 则在方法中传入该参数       
        ///  serverSocket.AcceptAsync(e); 返回一个bool值    通过F12  可知，该方法返回true则调用一个事件。返回false则自己主动添加该事件。    
        ///  （该事件是干啥的？？？？？？？？有啥用？）
        ///  
        /// </summary>
        /// <param name="e"></param>
        private void StartAccept(SocketAsyncEventArgs e)
        {
            if (e == null)
            {
                e = new SocketAsyncEventArgs();
                e.Completed += Accept_Completed;
            }
            //限制线程的访问
            acceptSemaphore.WaitOne();


            bool result = serverSocket.AcceptAsync(e);
            if (result == false)
            {
                ProcessAccept(e);
            }
        }

        private void Accept_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);
        }


        private void ProcessAccept(SocketAsyncEventArgs e)
        {
            //Socket clientSocket = e.AcceptSocket;
            ClientPeer client = clientPeerPool.DeQueue();
            client.clientSocket = (e.AcceptSocket);

            //开始接收数据
            StartReceive(client);



            e.AcceptSocket = null;
            StartAccept(e);
        }
        #endregion

        #region 断开连接
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client">断开的连接对象</param>
        /// <param name="reason"></param>
        public void DisConnect(ClientPeer client,string reason)
        {
            try
            {
                //断开连接清空数据 包括clientpeer的缓存  socket对象 
                if (client == null)
                {
                    throw new Exception("当前客户端为空，无法断开连接");
                }

                //通知应用层断开连接 TODO



                client.DisConnect();
                //连接池 回收
                clientPeerPool.EnQueue(client);
                //信号量  回复
                acceptSemaphore.Release();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


        #endregion

        #region 发送数据
        //根据客户端调用  所以这里不需要


        #endregion

        #region 接收数据

        private void StartReceive(ClientPeer client)
        {
            try
            {
                bool result = client.clientSocket.ReceiveAsync(client.ReceiveArgs);
                if (result == false)
                {
                    ProcessReceive(client.ReceiveArgs);

                }
            }
            catch (Exception e)
            {

            }
        }

        private void Receive_Completed(object sender,SocketAsyncEventArgs e)
        {
            ProcessReceive(e);
        }

        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            ClientPeer client = e.UserToken as ClientPeer;  //为什么这里不用e.acceptSocket
            ///判断网络消息是否接收成功
            if (client.ReceiveArgs.SocketError == SocketError.Success && client.ReceiveArgs.BytesTransferred > 0)
            {
                //拷贝到数组中
                byte[] packet = new byte[client.ReceiveArgs.BytesTransferred];
                Buffer.BlockCopy(client.ReceiveArgs.Buffer, 0, packet, 0, client.ReceiveArgs.BytesTransferred);

                client.StartReceive(packet);

                StartReceive(client);
            }
            //断开连接   //无传输数据表示断开连接
            else if (client.ReceiveArgs.BytesTransferred == 0)
            {

                if (client.ReceiveArgs.SocketError == SocketError.Success)
                {
                    //客户端主动断开连接
                    DisConnect(client,"客户端主动断开连接");
                }
                else
                {
                    //由于网络异常 被动断开
                    DisConnect(client, client.ReceiveArgs.SocketError.ToString());
                }
            }
        }


        private void ReceiveCompleted(ClientPeer client, SocketMsg msg)
        {
            //给应用层让其使用
            //todo
        }

        #endregion
    }
}
