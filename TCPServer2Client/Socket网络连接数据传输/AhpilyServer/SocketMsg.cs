using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AhpilyServer
{
    /// <summary>
    /// 网络消息
    /// 发送的时候都要发送这个类
    /// </summary>
    public class SocketMsg
    {
        public int OpCode { get; set; }
        public int SubCode { get; set; }
        public object Value { get; set; }

        public SocketMsg()
        {

        }
        public SocketMsg(int opCode,int subCode,int value)
        {
            this.OpCode = opCode;
            this.SubCode = subCode;
            this.Value = value;
        }
    }
}
