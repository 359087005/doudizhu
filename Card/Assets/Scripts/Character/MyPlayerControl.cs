using Protocol.Dto.Fight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyPlayerControl : CharacterBase
{
    private void Awake()
    {
        Bind(CharacterEvent.INIT_MY_CARD);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case CharacterEvent.INIT_MY_CARD:
                StartCoroutine(InitCardList(message as List<CardDto>));
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
    void Start()
    {
        cardParent = transform.Find("CardPoint");
        cardCtrlList = new List<CardCtrl>();
    }

    private IEnumerator InitCardList(List<CardDto> cardDtoList)
    {
        for (int i = 0; i < cardDtoList.Count; i++)
        {
            //创建卡牌
            CreatCard(cardDtoList[i],i);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void CreatCard(CardDto dto,int index)
    {
        GameObject cardPrefab = Resources.Load<GameObject>("Card/MyCard");
        GameObject card =  Instantiate(cardPrefab, cardParent);
        CardCtrl ctrl = card.GetComponent<CardCtrl>();
        card.transform.localPosition = new Vector2((index * 0.3f),0);
        card.name = dto.name;
        ctrl.Init(dto,index,true);
        //存储一下
        cardCtrlList.Add(ctrl);
    }


}
