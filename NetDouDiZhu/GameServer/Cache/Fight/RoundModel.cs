using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Cache.Fight
{
    /// <summary>
    /// 回合管理类
    /// </summary>
    public class RoundModel
    {
        /// <summary>
        /// 当前出牌者
        /// </summary>
        public int CurrentUid { get; set; }

        /// <summary>
        /// 当前最大出牌者
        /// </summary>
        public int BiggestUid { get; set; }

        /// <summary>
        /// 上次出牌长度
        /// </summary>
        public int LastLength { get; set; }

        /// <summary>
        /// 权值
        /// </summary>
        public int LastWeight { get; set; }

        /// <summary>
        /// 上次出牌类型
        /// </summary>
        public int LastCardType { get; set; }

        public RoundModel()
        {
            this.CurrentUid = -1;
            this.BiggestUid = -1;
            this.LastCardType = -1;
            this.LastLength = -1;
            this.LastWeight = -1;
        }

        public void Init()
        {
            this.CurrentUid = -1;
            this.BiggestUid = -1;
            this.LastCardType = -1;
            this.LastLength = -1;
            this.LastWeight = -1;
        }

        /// <summary>
        /// 开始游戏
        /// </summary>
        public void Start(int userId)
        {
            this.CurrentUid = userId;
            this.BiggestUid = userId;
        }
        /// <summary>
        /// 改变出牌
        /// </summary>
        public void Change(int userId,int length,int weight,int type)
        {
            this.BiggestUid = userId;
            this.LastLength = length;
            this.LastCardType = type;
            this.LastWeight = weight;
        }

        /// <summary>
        /// 转换出牌
        /// </summary>
        public void Turn(int userId)
        {
            this.CurrentUid = userId;
        }
    }
}
