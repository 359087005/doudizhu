using Protocol.Content;
using Protocol.Dto.Fight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Cache.Fight
{
    /// <summary>
    ///  牌库
    ///  54张牌 
    /// </summary>
    public class CardModel
    {
        public Queue<CardDto> CardQueue { get; set; }

        public CardModel()
        {
            //创建牌
            Creat();
            //洗牌
            Shuffle();
        }

        public void Init()
        {
            //创建牌
            Creat();
            //洗牌
            Shuffle();
        }

        private void Creat()
        {
            CardQueue = new Queue<CardDto>();
            //创建普通的牌
            for (int color = CardColor.SPADE; color <= CardColor.SQUARE; color++)
            {
                for (int weight = CardWeight.THREE; weight <= CardWeight.TWO; weight++)
                {
                    string cardName = CardColor.GetColor(color) + CardWeight.GetWeight(weight);
                    CardDto dto = new CardDto(cardName, color, weight);
                    CardQueue.Enqueue(dto);
                }
            }
            CardDto sJoker = new CardDto("SJoker", CardColor.NONE, CardWeight.SJOKER);
            CardDto lJoker = new CardDto("LJoker", CardColor.NONE, CardWeight.LJOKER);
            CardQueue.Enqueue(sJoker);
            CardQueue.Enqueue(lJoker);
        }

        private void Shuffle()
        {
            List<CardDto> newList = new List<CardDto>();
            Random r = new Random();

            foreach (CardDto card in CardQueue)
            {
                int index = r.Next(0, newList.Count + 1); //第一次是只能随机0 第二次随机0,1 第三次随机0,1,2
                newList.Insert(index, card);  //防止出现插入位置无东西
            }
            CardQueue.Clear();

            foreach (CardDto dto in newList)
            {
                CardQueue.Enqueue(dto);
            }
        }

        public CardDto Deal()
        {
            return CardQueue.Dequeue();
        }
    }


}
