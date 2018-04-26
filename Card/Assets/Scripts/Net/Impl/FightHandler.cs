using Protocol;
using Protocol.Dto.Fight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightHandler : HandlerBase 
{
    public override void OnReceive(int subCode, object value)
    {
        switch (subCode)
        {
            case FightCode.GET_CARD_SRES:
                GetCards(value as List<CardDto>);
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 获取到卡牌数据
    /// </summary>
    /// <param name="cardList"></param>
    private void GetCards(List<CardDto> cardList)
    {
        //给自身玩家创建牌的对象 
        Dispatch(AreaCode.CHARACTER,CharacterEvent.INIT_MY_CARD,cardList);
        Dispatch(AreaCode.CHARACTER, CharacterEvent.INIT_LEFT_CARD, null);
        Dispatch(AreaCode.CHARACTER, CharacterEvent.INIT_RIGHT_CARD, null);
        //初始化倍数
        Dispatch(AreaCode.UI,UIEvent.CHANGE_MUTIPLE,1);
    }

}
