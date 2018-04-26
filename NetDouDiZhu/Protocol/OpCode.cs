using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol
{
    public class OpCode
    {
        public const int ACCOUNT = 0;//账户模块
        public const int USER = 1;//角色模块
        public const int MATCH = 2;//匹配模块
        public const int CHAT = 3;//聊天模块
        public const int FIGHT = 4;//战斗模块
    }
}
