using Protocol.Dto.Fight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Content
{
    public class CardWeight
    {
        public const int THREE = 3;
        public const int FOUR = 4;
        public const int FIVE = 5;
        public const int SIX = 6;
        public const int SEVEN = 7;
        public const int EIGHT = 8;
        public const int NINE = 9;
        public const int TEN = 10;
        public const int JACK = 11;
        public const int QUEEN = 12;
        public const int KING = 13;

        public const int ONE = 14;
        public const int TWO = 15;

        public const int SJOKER = 16;
        public const int LJOKER = 17;

        public static string GetWeight(int weight)
        {
            string cardWeight = string.Empty;
            switch (weight)
            {

                case 3:
                    cardWeight = "Three";
                    break;
                case 4:
                    cardWeight = "Four";
                    break;
                case 5:
                    cardWeight = "Five";
                    break;
                case 6:
                    cardWeight = "Six";
                    break;
                case 7:
                    cardWeight = "Seven";
                    break;
                case 8:
                    cardWeight = "Eight";
                    break;
                case 9:
                    cardWeight = "Nine";
                    break;
                case 10:
                    cardWeight = "Ten";
                    break;
                case 11:
                    cardWeight = "Jack";
                    break;
                case 12:
                    cardWeight = "Queen";
                    break;
                case 13:
                    cardWeight = "King";
                    break;
                case 14:
                    cardWeight = "One";
                    break;
                case 15:
                    cardWeight = "Two";
                    break;
                default:
                    throw new Exception("呵");
            }
            return cardWeight;
        }

        public static int GetWeight(List<CardDto> cardlist, int cardType)
        {
            int totalWeight = 0;

            if (cardType == CardType.THREE || cardType == CardType.THREE_ONE || cardType == CardType.THREE_TWO)
            {
                for (int i = 0; i < cardlist.Count -2; i++)
                {
                    if (cardlist[i].weight == cardlist[i + 1].weight && cardlist[i].weight == cardlist[i + 2].weight)
                    {
                        totalWeight += (cardlist[i].weight * 3);
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < cardlist.Count; i++)
                {
                    totalWeight += cardlist[i].weight;
                }
            }
            return totalWeight;
        }
    }
}
