using Protocol.Dto.Fight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Content
{
    /// <summary>
    /// 卡牌类型
    /// </summary>
    public class CardType
    {
        public const int NONE = 0;
        public const int SINGLE = 1;//单
        public const int TWO = 2;//对
        public const int STRAIGHT = 3;//顺子
        public const int TRIPLE_DOUBLE = 4;//三联对
        public const int THREE = 5;//三不带
        public const int THREE_ONE = 6;//三带一
        public const int THREE_TWO = 7;//三带二
        public const int DOUBEL_THREE = 8;//飞机
        public const int BOOM = 9;
        public const int JOKER_BOOM = 10;

        /// <summary>
        /// 是不是单
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool isSingle(List<CardDto> cards)
        {
            return cards.Count == 1;
        }
        /// <summary>
        /// 判断是不是对
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool isDouble(List<CardDto> cards)
        {
            if (cards.Count == 2)
            {
                if (cards[0].weight == cards[1].weight)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 是否是顺子
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool isStraight(List<CardDto> cards)
        {
            if (cards.Count < 5 || cards.Count > 12)
            {
                return false;
            }

            for (int i = 0; i < cards.Count - 1; i++)
            {
                int tempWeight = cards[i].weight;
                if (cards[i + 1].weight - tempWeight != 1)
                    return false;
                //不能超过A
                if (tempWeight > CardWeight.ONE || cards[i + 1].weight > CardWeight.ONE)
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 姊妹对
        /// </summary>
        public static bool isTriple_double(List<CardDto> cards)
        {
            if (cards.Count < 6 || cards.Count % 2 != 0)
            {
                return false;
            }
            for (int i = 0; i < cards.Count; i++)
            {
                if (cards[i].weight != cards[i + 1].weight)
                    return false;
                if (cards[i + 2].weight - cards[i].weight != 1)
                    return false;
                if (cards[i + 2].weight > CardWeight.ONE)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 飞机 太麻烦 //TODO
        /// </summary>
        public static bool isDoubleThree(List<CardDto> cards)
        {
            //33344456   56333444
            if (cards.Count < 8)
                return false;

            for (int i = 0; i < cards.Count; i += 3)
            {
                if (cards[i].weight == cards[i + 1].weight && cards[i].weight == cards[i + 2].weight)
                {
                    if (cards[i + 3].weight - cards[1].weight == 1)
                    {
                        if (cards[i].weight > CardWeight.ONE || cards[i + 3].weight > CardWeight.ONE)
                        {
                            return false;
                        }
                        return true;
                    }
                }
                else if (cards[i + 2].weight == cards[i + 3].weight && cards[i].weight == cards[i + 4].weight)
                {
                    if (cards[i + 5].weight - cards[i + 2].weight == 1)
                    {
                        if (cards[i + 2].weight > CardWeight.ONE || cards[i + 5].weight > CardWeight.ONE)
                        {
                            return false;
                        }
                        return true;
                    }
                }

            }
            return false;
        }

        /// <summary>
        /// 三不带
        /// </summary>
        public static bool isThree(List<CardDto> cards)
        {
            if (cards.Count != 3) return false;
            if (cards[0].weight == cards[1].weight && cards[0].weight == cards[2].weight)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 三带一
        /// </summary>
        public static bool isThree_One(List<CardDto> cards)
        {
            if (cards.Count != 4) return false;
            if (cards[0].weight == cards[1].weight && cards[0].weight == cards[2].weight)
            {
                return true;
            }
            else if (cards[1].weight == cards[2].weight && cards[1].weight == cards[3].weight)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 三带二
        /// </summary>
        public static bool isThree_Two(List<CardDto> cards)
        {
            if (cards.Count != 5) return false;
            if (cards[0].weight == cards[1].weight && cards[0].weight == cards[2].weight)
            {
                if(cards[3].weight == cards[4].weight)
                return true;
            }
            else if (cards[2].weight == cards[3].weight && cards[2].weight == cards[4].weight)
            {
                if (cards[0].weight == cards[1].weight)
                    return true;
            }
            return false;
        }

        public static bool isBoom(List<CardDto> cards)
        {
            if (cards.Count != 4) return false;
            if (cards[0] != cards[1]) return false;
            if (cards[0] != cards[2]) return false;
            if (cards[0] != cards[3]) return false;
            return true;
        }
        public static bool isJokerBoom(List<CardDto> cards)
        {
            if (cards.Count != 2) return false;
            if (cards[0].weight == CardWeight.SJOKER && cards[1].weight == CardWeight.LJOKER)
            {
                return true;
            }
            else if (cards[1].weight == CardWeight.SJOKER && cards[0].weight == CardWeight.LJOKER)
            {
                return true;
            }
            return false; 
        }


        /// <summary>
        /// 获取卡牌类型
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static int GetCardType(List<CardDto> cards)
        {
            int cardType = CardType.NONE;

            switch (cards.Count)
            {
                case 1:
                    if (isSingle(cards))
                    {
                        cardType = CardType.SINGLE;
                    }
                    break;
                case 2:
                    if (isDouble(cards))
                    {
                        cardType = CardType.TWO;
                    }
                    else if (isJokerBoom(cards))
                    {
                        cardType = CardType.JOKER_BOOM;
                    }
                    break;
                case 3:
                    if (isThree(cards))
                    {
                        cardType = CardType.THREE;
                    }
                    break;
                case 4:
                    if (isThree_One(cards))
                    {
                        cardType = CardType.THREE_ONE;
                    }
                    else if(isBoom(cards))
                    {
                        cardType = CardType.BOOM;
                    }
                    break;
                case 5:
                    if (isThree_Two(cards))
                    {
                        cardType = CardType.TRIPLE_DOUBLE;
                    }
                    else if (isStraight(cards))
                    {
                        cardType = CardType.STRAIGHT;
                    }
                    break;
                case 6:
                    if (isStraight(cards))
                    {
                        cardType = CardType.STRAIGHT;
                    }
                    else if(isTriple_double(cards))
                    {
                        cardType = CardType.TRIPLE_DOUBLE;
                    }
                    break;
                case 8:
                    if (isDoubleThree(cards))
                    {
                        cardType = CardType.DOUBEL_THREE;
                    }
                    else if (isStraight(cards))
                    {
                        cardType = CardType.STRAIGHT;
                    }
                    break;
                case 7:
                case 9:
                case 10:
                case 11:
                case 12:
                    if (isStraight(cards))
                    {
                        cardType = CardType.STRAIGHT;
                    }
                    break;
                default:
                    break;
            }
            return cardType;
        }

    }
}
