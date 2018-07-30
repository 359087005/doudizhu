using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TCPSocket
{
    /// <summary>
    /// TCP服务端
    /// 接收请求
    /// 发送数据
    /// 接收数据
    /// 断开连接
    /// </summary>
    class Program
    {
        private static Socket serverSocket = null;
        static void Main(string[] args)
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 8860); //0-65535
            serverSocket.Bind(endPoint);

            serverSocket.Listen(10);

            Console.WriteLine("服务器开始监听");
            //开启一个新线程接收客户端连接
            Thread thread = new Thread(ListenClient);
            thread.Start();

            while (true)
            {

            }
        }

        /// <summary>
        ///监听客户端的连接
        /// </summary>
        private static void ListenClient()
        {
            ///等待有客户端连接的时候就会触发这个函数  返回一个客户端的socket对象
            Socket clientSocket = serverSocket.Accept();
            Console.WriteLine("客户端连接成功," + clientSocket.AddressFamily.ToString());
            //给客户端发消息
            clientSocket.Send(Encoding.UTF8.GetBytes("服务器连接成功"));

            Thread receiveThread = new Thread(ReceiveClientMessage);
            receiveThread.Start(clientSocket);
        }

        private static void ReceiveClientMessage(object clientSocket)
        {
            Socket client = clientSocket as Socket;
            byte[] buffer = new byte[1024];
            int length = client.Receive(buffer);
            Console.WriteLine(Encoding.UTF8.GetString(buffer, 0, length));

        }
    }
}


