using Protocol.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto.Fight
{
    [Serializable]
    public class PlayerDto
    {
        public int id;
        public int identity;//自己的身份 农民 地主
        public List<CardDto> cardList; //自己有的手牌

        public PlayerDto(int userId)
        {
            this.id = userId;
            identity = Identity.FARMER;
            cardList = new List<CardDto>();
        }

        /// <summary>
        /// 是否有卡牌  true有 false 没有
        /// </summary>
        public bool HasCard
        {
            get { return cardList.Count != 0; }
        }

        /// <summary>
        /// 卡牌数量
        /// </summary>
        public int CardCount
        {
            get { return cardList.Count; }
        }

        /// <summary>
        /// 添加卡牌
        /// </summary>
        /// <param name="card"></param>
        public void Add(CardDto card)
        {
            cardList.Add(card);
        }
        /// <summary>
        /// 移除卡牌
        /// </summary>
        /// <param name="card"></param>
        public void Remove(CardDto card)
        {
            cardList.Remove(card);
        }

    }
}
