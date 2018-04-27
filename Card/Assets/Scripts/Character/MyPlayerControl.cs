using Assets.Scripts.GameDataModel;
using Protocol;
using Protocol.Dto.Fight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyPlayerControl : CharacterBase
{
    private void Awake()
    {
        Bind(CharacterEvent.INIT_MY_CARD,
            CharacterEvent.ADD_MY_CARD,
            CharacterEvent.DEAL_CARD,
            CharacterEvent.REMOVE_MY_CARD);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case CharacterEvent.INIT_MY_CARD:
                StartCoroutine(InitCardList(message as List<CardDto>));
                break;
            case CharacterEvent.ADD_MY_CARD:
                AddTableCard(message as GrabDto);
                break;
            case CharacterEvent.DEAL_CARD:
                DealSelectCard();
                break;
            case CharacterEvent.REMOVE_MY_CARD:
                RemoveCard(message as List<CardDto> );
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 自身的卡牌管理
    /// </summary>
    private List<CardCtrl> cardCtrlList;
    /// <summary>
    /// 卡牌父物体
    /// </summary>
    private Transform cardParent;


    private PromptMsg promptMsg;
    private SocketMsg socketMsg;
    void Start()
    {
        cardParent = transform.Find("CardPoint");
        cardCtrlList = new List<CardCtrl>();

        promptMsg = new PromptMsg();
        socketMsg = new SocketMsg();
    }

    /// <summary>
    /// 出选中的牌   并发送给服务器
    /// </summary>
    private void DealSelectCard()
    {
        List<CardDto> selectCardList = GetSelectCard();
        DealDto dto = new DealDto(selectCardList,Model.gameModel.UserDto.id);
        //如果出牌不合法
        if (dto.isRegular == false)
        {
            promptMsg.ChangeText("卡牌不对", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPTA_ANIM, promptMsg);
            return;
        }
        else //可以出牌
        {
            socketMsg.Change(OpCode.FIGHT,FightCode.DEAL_CREQ, dto);
            Dispatch(AreaCode.NET,0,socketMsg);
        }
    }
    /// <summary>
    /// 获取选中的牌
    /// </summary>
    private List<CardDto> GetSelectCard()
    {
        List<CardDto> selectCardList = new List<CardDto>();
        foreach (CardCtrl item in cardCtrlList)
        {
            if (item.isSelected == true)
            {
                selectCardList.Add(item.cardDto);
            }
        }
        return selectCardList;
    }
    /// <summary>
    /// 移除卡牌
    /// </summary>
    private void RemoveCard(List<CardDto> remainCardList)
    {
        //显示剩余的牌 
        int index = 0;
        foreach (var item in cardCtrlList)
        {
            if (remainCardList.Count == 0) break;
            else
            {
                item.gameObject.SetActive(true);
                item.Init(remainCardList[index],index,true);
                index++;
                //没有拍了
                if (index == remainCardList.Count)
                {
                    break;
                }
            }
        }
        //吧index之后的都隐藏掉
        for (int i = index; i < cardCtrlList.Count; i++)
        {
            cardCtrlList[i].isSelected = false;
            cardCtrlList[i].gameObject.SetActive(false);
        }
    }


    /// <summary>
    /// 添加底牌的方法
    /// </summary>
    /// <param name="cardList"></param>
    private void AddTableCard(GrabDto dto)
    {
        List<CardDto> tableCardList = dto.tableCarList;
        List<CardDto> playerCardList = dto.playerCardList;

        //复用之前创建的卡牌
        int index = 0;
        foreach (var cardCtrl in cardCtrlList)
        {
            cardCtrl.gameObject.SetActive(true);
            cardCtrl.Init(playerCardList[index], index, true);
            index++;
        }
        //在创建新的3张卡牌
        GameObject cardPrefab = Resources.Load<GameObject>("Card/MyCard");
        //
        for (int i = index; i < playerCardList.Count; i++)
        {
            CreatCard(playerCardList[i], i, cardPrefab);
        }

    }
    /// <summary>
    /// 初始化卡牌
    /// </summary>
    /// <param name="cardDtoList"></param>
    /// <returns></returns>
    private IEnumerator InitCardList(List<CardDto> cardDtoList)
    {
        GameObject cardPrefab = Resources.Load<GameObject>("Card/MyCard");
        for (int i = 0; i < cardDtoList.Count; i++)
        {
            //创建卡牌
            CreatCard(cardDtoList[i], i, cardPrefab);
            yield return new WaitForSeconds(0.1f);
        }
    }

    /// <summary>
    /// 创建卡牌
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="index"></param>
    private void CreatCard(CardDto dto, int index, GameObject cardPrefab)
    {
        GameObject card = Instantiate(cardPrefab, cardParent);
        CardCtrl ctrl = card.GetComponent<CardCtrl>();
        card.transform.localPosition = new Vector2((index * 0.3f), 0);
        card.name = dto.name;
        ctrl.Init(dto, index, true);
        //存储一下
        cardCtrlList.Add(ctrl);
    }


}
