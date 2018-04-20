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
        public int id;
        public string name;
        public int color;//花色
        public int weight;//权重

        public CardDto()
        {

        }

        public CardDto(int id, string name, int color, int weight)
        {
            this.name = name;
            this.id = id;
            this.name = name;
            this.weight = weight;
        }
    }
}
