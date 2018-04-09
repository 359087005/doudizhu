using System;
using System.Collections.Generic;
using System.Text;

using System.Net;
using System.Net.Sockets;
using System.Threading;


namespace AhpilyServer
{
    /// <summary>
    /// 服务器端
    /// </summary>
    public class ServerPeer
    {

        private Socket serverPeer;

        /// <summary>
        /// 客户端连接池
        /// </summary>
        ClientPeerPool clientPeerPool;

        /// <summary>
        /// 应用层
        /// </summary>
        IApplication app;

        /// <summary>
        /// 设置应用层
        /// </summary>
        /// <param name="app"></param>
        public void SetApplication(IApplication app)
        {
            this.app = app;
        }

        /// <summary>
        /// 限制客户端连接数量的信号量
        /// </summary>
        private Semaphore acceptSemaphore;  //位于system.Threanding 下。限制可同时访问某一资源池的线程数。 

        /// <summary>
        /// 开启服务器    1，bind  2，listen  3.accept
        /// </summary>
        /// <param name="port"></param>
        /// <param name="maxCount"></param>
        public void Start(int port, int maxCount)
        {
            try
            {
                serverPeer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                acceptSemaphore = new Semaphore(maxCount, maxCount);
                //在连接之前就调用客户端线程池 初始化
                clientPeerPool = new ClientPeerPool(maxCount);
                ClientPeer clientPeer = null;
                for (int i = 0; i < maxCount; i++)
                {
                    clientPeer = new ClientPeer();
                    clientPeer.receiveArgs.Completed += Receive_Complete;
                    clientPeer.receiveCompleted = ReceiveCompleted;
                    clientPeer.sendDisConnected = DisConnectd;
                    clientPeerPool.Enqueue(clientPeer);
                }

                serverPeer.Bind(new IPEndPoint(IPAddress.Any, port));
                serverPeer.Listen(10);

                Console.WriteLine("服务器启动成功...");
                StartAccept(null);
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
        }

        #region 接收客户端连接
        //使用异步 防止卡顿 进行多线程处理
        /// <summary>
        /// 开始等待客户端的连接
        /// </summary>
        /// <param name="e"></param>
        private void StartAccept(SocketAsyncEventArgs e)
        {   //为何要加这个奇怪的参数~
            //答：服务器启动成功之后，需要开始监听，通常调用 serverPeer.Accept 的方法
            //但是使用异步加载更好，异步加载需要一个 SocketAsyncEventArgs 的参数 
            //所以此方法加入此参数
            if (e == null)
            {
                e = new SocketAsyncEventArgs();
                e.Completed += Accept_Completed;
            }

            bool result = serverPeer.AcceptAsync(e); //返回值是bool类型，判断事情是否执行完毕。
                                                     //返回true，表示正在执行,执行完毕后触发一个事件。
                                                     //返回false,表示已经执行完毕，直接处理。
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
            ///限制线程的访问    计数。 假设客户端100个  每调用一次加一。等到100就等待 有位置就继续
            acceptSemaphore.WaitOne();
            //得到客户端的对象
            //1,原始方法
            //Socket socket = e.AcceptSocket;
            //2,更改方法。 但是每次都需要new  耗费性能。所以新建一个对象池。。。。
            //ClientPeer clientPeer = new ClientPeer();
            //clientPeer.SetSocket(e.AcceptSocket);
            //3,
            ClientPeer clientPeer = clientPeerPool.Dequeue();
            clientPeer.clientSocket = e.AcceptSocket;

            //开始接收数据
            StartReceive(clientPeer);
            //继续进行处理
            //一直进行接收客户端发来的数据  伪循环
            e.AcceptSocket = null;
            StartAccept(e);
        }
        #endregion

        #region    接收数据
        /// <summary>
        /// 开始接受数据
        /// </summary>
        /// <param name="client"></param>
        private void StartReceive(ClientPeer client)
        {
            try
            {
                bool result = client.clientSocket.ReceiveAsync(client.receiveArgs);
                if (result == false)
                {
                    ProcessReceive(client.receiveArgs);
                }

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// 处理接收的请求
        /// </summary>
        /// <param name="e"></param>
        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            ClientPeer client = e.UserToken as ClientPeer;   //userToken  获得与此异步套接字操作相关的用户或应用程序

            //判断网络消息是否接收成功
            if (client.receiveArgs.SocketError == SocketError.Success && client.receiveArgs.BytesTransferred > 0)
            {
                //拷贝数据
                byte[] packet = new byte[client.receiveArgs.BytesTransferred];
                Buffer.BlockCopy(client.receiveArgs.Buffer, 0, packet, 0, client.receiveArgs.BytesTransferred);
                //客户端自身处理数据包
                client.StartReceive(packet);

                //尾递归
                StartReceive(client);
            }
            //断开连接  没有传输的字节数
            else if (client.receiveArgs.BytesTransferred == 0)
            {
                //客户端主动断开
                if (client.receiveArgs.SocketError == SocketError.Success)
                {
                    DisConnectd(client,"客户端主动断开");
                }
                //网络异常导致
                else
                {
                    DisConnectd(client,e.SocketError.ToString());
                }
            }
        }
        /// <summary>
        /// 当接收完成时 触发的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Receive_Complete(object sender, SocketAsyncEventArgs e)
        {
            ProcessReceive(e);
        }

        /// <summary>
        /// 一条数据解析完成的处理
        /// </summary>
        /// <param name="client">对应的连接对象</param>
        /// <param name="sender">解析出来的一个具体能使用的类型</param>

        private void ReceiveCompleted(ClientPeer client,SocketMsg msg)
        {
            //给应用层(应用程序 program)使用、
            app.OnReceive(client,msg);
        }

        #endregion

        #region 断开连接
        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="client">断开的连接对象</param>
        /// <param name="reason">断开的原因</param>
        public void DisConnectd(ClientPeer client,string reason)
        {
            try
            {
                //清空一些数据
                if (client == null) 
                {
                    throw new Exception("当前客户端对象为空，无法断开");
                }

                //通知应用层 这个客户断开连接
                app.DisConnected(client);

                client.Disconnected();
                //回收对象
                clientPeerPool.Enqueue(client);

                acceptSemaphore.Release();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
        }
        #endregion

    }
}
