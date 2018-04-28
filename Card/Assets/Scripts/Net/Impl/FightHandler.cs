using Assets.Scripts.GameDataModel;
using Protocol;
using Protocol.Content;
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
            case FightCode.TURN_GRAB__BRO:
                TurnGrabBro((int)value);
                break;
            case FightCode.GRAB_LANDLORD_BRO:
                GrabLandLoardBro(value as GrabDto);
                break;
            case FightCode.TURN_DEAL_BRO:
                TurnDealBro((int)value);
                break;
            case FightCode.DEAL_BRO:
                DealBro(value as DealDto);
                break;
            case FightCode.DEAL_SRES:
                DealResponse((int)value);
                break;
            case FightCode.OVER_BRO:
                OverBro(value as OverDto);
                break;
            default:
                break;
        }
    }
    PromptMsg msg = new PromptMsg();
    private void DealResponse(int result)
    {
        if (result == -1)
        {
            //玩家出的牌关不上
            msg = new PromptMsg("管不上", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPTA_ANIM, msg);
            //重新显示出牌按钮
            Dispatch(AreaCode.UI, UIEvent.SHOW_DEAL_BUTTON, true);
        }
    }
    /// <summary>
    /// 游戏结束
    /// </summary>
    private void OverBro(OverDto dto)
    {
        if (dto.winUidList.Contains(Model.gameModel.UserDto.id))
        {
            Dispatch(AreaCode.AUDIO, AudioEvent.PLAY_EFFECT_AUDIO, "Fight/MusicEx_Win");
        }
        else
        {
            Dispatch(AreaCode.AUDIO, AudioEvent.PLAY_EFFECT_AUDIO, "Fight/MusicEx_Lose");
        }
        Dispatch(AreaCode.UI,UIEvent.SHOW_OVER_PANEL,dto);
    }

    /// <summary>
    /// 同步出牌
    /// </summary>
    /// <param name="dto"></param>
    private void DealBro(DealDto dto)
    {
        //关闭出牌按钮
        Dispatch(AreaCode.UI, UIEvent.HIDE_DEAL_BUTTON, null);
        //移除出完的手牌
        int eventCode = -1;
        if (dto.userId == Model.gameModel.matchRoomDto.leftId)
        {
            eventCode = CharacterEvent.REMOVE_LEFT_CARD;
        }
        else if (dto.userId == Model.gameModel.matchRoomDto.rightId)
        {
            eventCode = CharacterEvent.REMOVE_RIGHT_CARD;
        }
        else if (dto.userId == Model.gameModel.UserDto.id)
        {
            eventCode = CharacterEvent.REMOVE_MY_CARD;
        }
        Dispatch(AreaCode.CHARACTER, eventCode, dto.remainCardList);
        //显示到桌面//出的牌放到正中间
        Dispatch(AreaCode.CHARACTER, CharacterEvent.UPDATE_SHOW_DESK, dto.selectCardList);
        //播放音效
        PlayDealAudio(dto.type, dto.weight);
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    private void PlayDealAudio(int cardType, int weight)
    {
        string audioName = "Fight/";
        switch (cardType)
        {
            case CardType.SINGLE:
                audioName += "Woman_" + weight;
                break;
            case CardType.TWO:
                audioName += "Woman_dui" + weight / 2;
                break;
            case CardType.STRAIGHT:
                audioName += "Woman_shunzi";
                break;
            case CardType.THREE:
                audioName += "Woman_tuple" + weight / 3;
                break;
            case CardType.THREE_ONE:
                audioName += "Woman_sandaiyi";
                break;
            case CardType.TRIPLE_DOUBLE:
                audioName += "Woman_liandui";
                break;
            case CardType.THREE_TWO:
                audioName += "Woman_sandaiyidui";
                break;
            case CardType.DOUBEL_THREE:
                audioName += "Woman_feiji";
                break;
            case CardType.BOOM:
                audioName += "Woman_zhadan";
                break;
            case CardType.JOKER_BOOM:
                audioName += "Woman_wangzha";
                break;
            default:
                break;
        }
        Dispatch(AreaCode.AUDIO, AudioEvent.PLAY_EFFECT_AUDIO, audioName);
    }

    /// <summary>
    /// 转换出牌
    /// </summary>
    /// <param name="userId"></param>
    private void TurnDealBro(int userId)
    {
        if (Model.gameModel.UserDto.id == userId)
        {
            Dispatch(AreaCode.UI, UIEvent.SHOW_DEAL_BUTTON, true);
        }
    }


    ///地主广播
    private void GrabLandLoardBro(GrabDto dto)
    {
        //改变身份
        Dispatch(AreaCode.UI, UIEvent.PLAYER_CHANGE_IDENTITY, dto.userId);
        //发出声音
        Dispatch(AreaCode.AUDIO, AudioEvent.PLAY_EFFECT_AUDIO, "Fight/Woman_Order");
        //显示卡牌 
        Dispatch(AreaCode.UI, UIEvent.SET_TABLE_CARDS, dto.tableCarList);
        //增加地主底牌
        int eventCode = -1;
        if (dto.userId == Model.gameModel.matchRoomDto.leftId)
        {
            eventCode = CharacterEvent.ADD_LEFT_CARD;
        }
        else if (dto.userId == Model.gameModel.matchRoomDto.rightId)
        {
            eventCode = CharacterEvent.ADD_RIGHT_CARD;
        }
        else if (dto.userId == Model.gameModel.UserDto.id)
        {
            eventCode = CharacterEvent.ADD_MY_CARD;
        }
        Dispatch(AreaCode.CHARACTER, eventCode, dto);
    }

    /// <summary>
    /// 是不是第一个抢地主的  如果是第一个抢的不用播放转换音效 
    /// </summary>
    private bool isFirst = true;
    /// <summary>
    /// 转换抢地主
    /// </summary>
    /// <param name="userId"></param>
    private void TurnGrabBro(int userId)
    {
        if (isFirst == true)
        {
            isFirst = false;
        }
        else
        {
            Dispatch(AreaCode.AUDIO, AudioEvent.PLAY_EFFECT_AUDIO, "Fight/Woman_NoOrder");
        }
        //如果是自身 就显示2个按钮 
        if (userId == Model.gameModel.UserDto.id)
        {
            Dispatch(AreaCode.UI, UIEvent.SHOW_GRAB_BUTTON, true);
        }
    }
    /// <summary>
    /// 获取到卡牌数据
    /// </summary>
    /// <param name="cardList"></param>
    private void GetCards(List<CardDto> cardList)
    {
        //给自身玩家创建牌的对象 
        Dispatch(AreaCode.CHARACTER, CharacterEvent.INIT_MY_CARD, cardList);
        Dispatch(AreaCode.CHARACTER, CharacterEvent.INIT_LEFT_CARD, null);
        Dispatch(AreaCode.CHARACTER, CharacterEvent.INIT_RIGHT_CARD, null);
        //初始化倍数
        Dispatch(AreaCode.UI, UIEvent.CHANGE_MUTIPLE, 1);
    }

}
