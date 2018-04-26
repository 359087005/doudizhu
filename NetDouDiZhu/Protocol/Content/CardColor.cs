using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Content
{
    /// <summary>
    /// 卡牌花色
    /// </summary>
   public class CardColor
    {
        public const int NONE = 0;
        public const int SPADE = 1;// 黑
        public const int HEART = 2;//红
        public const int CLUB = 3;//梅
        public const int SQUARE = 4;//方


        public static string GetColor(int color)
        {
            string cardColor = string.Empty;
            switch (color)
            {
                case SPADE:
                    cardColor = "Spade";
                    break;
                case HEART:
                    cardColor = "Heart";
                    break;
                case CLUB:
                    cardColor = "Club";
                    break;
                case SQUARE:
                    cardColor = "Square";
                    break;
                default:
                    throw new Exception("呵呵呵");
                    break;
            }
            return cardColor;
        }
    }
}
