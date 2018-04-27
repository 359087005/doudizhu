using Protocol.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto.Fight
{
    [Serializable]
    public class DealDto
    {
        /// <summary>
        /// 选中要出的牌
        /// </summary>
        public List<CardDto> selectCardList;

        /// <summary>
        /// 剩余手牌
        /// </summary>
        public List<CardDto> remainCardList;
        /// <summary>
        /// 长度
        /// </summary>
        public int length;
        /// <summary>
        /// 权值
        /// </summary>
        public int weight;
        /// <summary>
        /// 类型
        /// </summary>
        public int type;

        /// <summary>
        /// 谁出的
        /// </summary>
        public int userId;
        /// <summary>
        /// 是否合法
        /// </summary>
        public bool isRegular;

        public DealDto()
        { }
        public DealDto(List<CardDto> cardList,int userID)
        {
            this.selectCardList = cardList;
            this.length = cardList.Count;
            this.type = CardType.GetCardType(cardList);
            this.weight = CardWeight.GetWeight(cardList,this.type);
            this.userId = userID;
            this.isRegular = (this.type != CardType.NONE);
            this.remainCardList = new List<CardDto>();
        }
    }
}
