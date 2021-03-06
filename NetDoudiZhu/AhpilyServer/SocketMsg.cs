﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AhpilyServer
{

    /// <summary>
    /// 网络消息
    /// 作用：发送的时候 都要发送这个类
    /// </summary>
    public class SocketMsg
    {
        /// <summary>
        /// 操作码
        /// </summary>
        public int opCode { get; set; }
        /// <summary>
        /// 子操作
        /// </summary>
        public int subCode { get; set; }
        /// <summary>
        /// 参数
        /// </summary>
        public object value { get; set; }

        public SocketMsg() { }
        public SocketMsg(int opCode, int subCode, object value)
        {
            this.opCode = opCode;
            this.subCode = subCode;
            this.value = value;
        }
    }
}
