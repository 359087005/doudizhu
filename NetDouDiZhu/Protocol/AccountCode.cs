using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol
{
   public class AccountCode
    {
        //注册的操作码
        public const int REGIST_CREQ = 0;  // accountdto 
        public const int REGIST_SRES = 1;
        //登录的操作码
        public const int LOGIN = 2;// accountdto 
    }
}
