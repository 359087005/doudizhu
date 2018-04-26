using Protocol.Content;
using Protocol.Dto.Fight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Cache.Fight
{
    /// <summary>
    /// 房间模型
    /// 牌的存储  玩家身份....
    /// </summary>
    public class FightRoom
    {
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// //所有玩家  玩家怎么表示  需要一个玩家模型  需要放到protocol   玩家客户端和服务器共同存储
        /// </summary>
        public List<PlayerDto> PlayerList { get; set; }
        /// <summary>
        /// //逃跑list
        /// </summary>
        public List<int> LeaveUidList { get; set; }

        /// <summary>
        /// 牌库
        /// </summary>
        public CardModel cardModel { get; set; }

        /// <summary>
        /// 底牌
        /// </summary>
        public List<CardDto> TableCardList { get; set; }

        /// <summary>
        /// 倍数
        /// </summary>
        public int Mutiple { get; set; }

        //回合管理类
        //管理谁出牌   出的什么牌  顺子？对子？单？  为下一位出牌者准备
        public RoundModel roundModel { get; set; }

        /// <summary>
        /// 构造方法 做初始化
        /// </summary>
        /// <param name="id"></param>
        public FightRoom(int id, List<int> uIdList)
        {
            cardModel = new CardModel();

            this.id = id;
            this.PlayerList = new List<PlayerDto>();

            foreach (int uid in uIdList)
            {
                PlayerDto dto = new PlayerDto(uid);
                PlayerList.Add(dto);
            }

            this.LeaveUidList = new List<int>();
            this.TableCardList = new List<CardDto>();
            this.Mutiple = 1;
            this.roundModel = new RoundModel();
        }

        public void Init(List<int> uIdList)
        {
            foreach (int uid in uIdList)
            {
                PlayerDto dto = new PlayerDto(uid);
                PlayerList.Add(dto);
            }
        }


        public bool isOffline(int userId)
        {
            return LeaveUidList.Contains(userId);
        }


        /// <summary>
        /// 转换出牌
        /// </summary>
        public int Turn()
        {
            int currentUid = roundModel.CurrentUid;
            //下一个ID
            int nextUid = GetNextUid(currentUid);
            //顺带更改当前出牌者
            roundModel.CurrentUid = nextUid;

            return nextUid;
        }
        /// <summary>
        /// 获取下一个出牌者
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        public int GetNextUid(int current)
        {
            for (int i = 0; i < PlayerList.Count; i++)
            {
                if (PlayerList[i].id == current)
                {
                    if (i == 2)
                    {
                        return PlayerList[0].id;
                    }
                    else
                        return PlayerList[i + 1].id;
                }
            }
            throw new Exception("查无此人");
        }


        /// <summary>
        /// 判断能不能压死上回合的牌
        /// </summary>
        /// <returns></returns>
        public bool DealCard(int length,int type,int weight,int userId,List<CardDto> cardList)
        {
            bool CanDeal = false;

            //用什么  管什么  
            //对子 单  三带一   炸弹                      顺子除外！！！
            if (type == roundModel.LastCardType && weight > roundModel.LastWeight)
            {
                if (type == CardType.STRAIGHT)//特殊类型 顺子  
                {
                    if (length == roundModel.LastLength)
                    {
                        //满足顺子出牌条件
                        CanDeal = true;
                    }
                }
                else  //不是顺子
                {
                    CanDeal = true;
                }
            }
            else if (type == CardType.BOOM && roundModel.LastCardType != CardType.BOOM)
            {
                CanDeal = true;
            }
            else if (type == CardType.JOKER_BOOM)
            {
                CanDeal = true;
            }
            //能管上
            if (CanDeal)
            {
                //移除玩家手牌
                RemoveCards(userId,cardList);
                //炸弹倍数
                SetMutiple(type);
                //保存回合信息
                roundModel.Change(userId,length,weight,type);
            }
            return CanDeal;
        }
        /// <summary>
        /// 移除手牌
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cardList"></param>
        public void RemoveCards(int userId, List<CardDto> cardList)
        {
            //获取玩家现在又的手牌
            List<CardDto> currentCardList = GetPlayerCard(userId);

            for (int i = 0; i < currentCardList.Count; i++)
            {
                foreach (CardDto item in cardList)
                {
                    if (currentCardList[i].name == item.name)
                    {
                        currentCardList.RemoveAt(i);
                    }
                }
            }
        }

        /// <summary>
        /// 获取玩家现有手牌
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<CardDto> GetPlayerCard(int userId)
        {
            foreach (PlayerDto player in PlayerList)
            {
                if (player.id == userId)
                    return player.cardList;
            }

            throw new Exception("查无此人~");
        }


        /// <summary>
        /// 设置倍数
        /// </summary>
        /// <param name="type"></param>
        private void SetMutiple(int type)
        {
            if (type == CardType.BOOM)
            {
                Mutiple *= 2;
            }
            else if (type == CardType.JOKER_BOOM)
            {
                Mutiple *= 4;
            }
        }
        /// <summary>
        /// 发牌 初始化角色手牌    
        /// </summary>
        public void InitPlayerCards()
        {
            //一人   17
            for (int i = 0; i < 17; i++)
            {
                CardDto dto =  cardModel.Deal();
                PlayerList[0].Add(dto);
            }
            for (int i = 0; i < 17; i++)
            {
                CardDto dto = cardModel.Deal();
                PlayerList[1].Add(dto);
            }
            for (int i = 0; i < 17; i++)
            {
                CardDto dto = cardModel.Deal();
                PlayerList[2].Add(dto);
            }
            //3个底牌显示
            for (int i = 0; i < 3; i++)
            {
                CardDto dto = cardModel.Deal();
                TableCardList.Add(dto);
            }
            //给地主
        }

        /// <summary>
        /// 设置地主
        /// </summary>
        /// <param name="userId"></param>
        public void SetLandlord(int userId)
        {
            foreach (PlayerDto player in PlayerList)
            {
                if (player.id == userId)
                {
                    //找到地主
                    player.identity = Identity.LANDLORD;
                    //给地主发底牌
                    for (int i = 0; i < TableCardList.Count; i++)
                    {
                        player.Add(TableCardList[i]);
                    }

                    //开始回合
                    roundModel.Start(userId);
                }
            }
        }


        /// <summary>
        /// 获取玩家数据模型
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public PlayerDto GetPlayerDto(int userId)
        {
            foreach (PlayerDto player in PlayerList)
            {
                if (player.id == userId)
                {
                    return player;
                }
            }
            throw new Exception("无人");
        }

        /// <summary>
        /// 获取用户身份
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetPlayerIdentity(int userId)
        {
            return GetPlayerDto(userId).identity;
            throw new Exception("查无此人_IDENTITY");
        }

        /// <summary>
        /// 获取相同身份的用户ID   最后胜利可以用到
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public List<int> GetSameIdentityUids(int identity)
        {
            List<int> uids = new List<int>();

            foreach (PlayerDto dto in PlayerList)
            {
                if (dto.identity == identity)
                {
                    uids.Add(dto.id);
                }
            }
            return uids;
        }

        /// <summary>
        /// 获取不同身份的用户ID
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public List<int> GetDifIdentityUids(int identity)
        {
            List<int> uids = new List<int>();

            foreach (PlayerDto dto in PlayerList)
            {
                if (dto.identity != identity)
                {
                    uids.Add(dto.id);
                }
            }
            return uids;
        }

        /// <summary>
        /// 获取房间内第一个玩家ID   第一个抢地主的
        /// </summary>
        /// <returns></returns>
        public int GetFirstUid()
        {
            return PlayerList[0].id;
        }

        /// <summary>
        /// 手牌排序
        /// </summary>
        /// <param name="cardList"></param>
        /// <param name="asc"></param>
        private void SortCard(List<CardDto> cardList,bool asc = true)  //asc  des 升降顺序
        {
            cardList.Sort
                (
             delegate(CardDto a,CardDto b) 
             {
                 if (asc)
                 {
                     return a.weight.CompareTo(b.weight);
                 }
                 else
                     return a.weight.CompareTo(b.weight) * -1;
             }   
             );
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="asc"></param>
        public void Sort(bool asc = true)
        {
            SortCard(PlayerList[0].cardList, asc);
            SortCard(PlayerList[1].cardList, asc);
            SortCard(PlayerList[2].cardList, asc);
            SortCard(TableCardList,true);
        }
    }
}
