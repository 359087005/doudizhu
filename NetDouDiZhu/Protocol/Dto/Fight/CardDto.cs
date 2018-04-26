using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto.Fight
{
    /// <summary>
    /// 卡牌类
    /// </summary>
    [Serializable]
   public class CardDto
    {
        public string name;
        public int color;//花色
        public int weight;//权重

        public CardDto()
        {

        }

        public CardDto(string name, int color, int weight)
        {
            this.name = name;
            this.name = name;
            this.weight = weight;
        }
    }
}
