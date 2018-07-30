using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ClientSocket
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket clientSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            //客户端连接不需要bind
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"),8860);

            clientSocket.Connect(endPoint);
            Console.WriteLine("连接成功");
            byte[] bytes = new byte[1024];

            int length =  clientSocket.Receive(bytes);

            Console.WriteLine(Encoding.UTF8.GetString(bytes,0,length));
            clientSocket.Send(Encoding.UTF8.GetBytes("服务器你好，我是client"));
            while (true)
            {

            }
        }

       
    }
}
