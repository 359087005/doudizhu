using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol
{
    public class FightCode
    {
        public const int GRAB_LANDLORD_CREQ = 0; //客户端发起抢地主请求
        public const int GRAB_LANDLORD_BRO = 1; //服务器返回抢地主结果
        public const int TURN_GRAB__BRO = 2;//服务器广播下一个玩家抢地主的结果


        public const int DEAL_CREQ = 3; //客户端发起出牌请求
        public const int DEAL_SRES = 4; //服务器发回客户端响应
        public const int DEAL_BRO = 5;//服务器广播出牌结果

        public const int PASS_CREQ = 6; //客户端发起不出请求
        public const int PASS_SRES = 7;//服务器返回不出响应

        public const int TURN_DEAL_BRO = 8; //服务器广播转换结果

        public const int LEAVE_BRO = 9; //服务器广播玩家离开

        public const int OVER_BRO = 10;//服务器广播游戏结束

        public const int GET_CARD_SRES = 11;//服务器给客户端卡牌的响应

    }
}
