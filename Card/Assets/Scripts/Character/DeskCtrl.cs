using Protocol.Dto.Fight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeskCtrl : CharacterBase
{
    private void Awake()
    {
        Bind(CharacterEvent.UPDATE_SHOW_DESK);
    }
    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case CharacterEvent.UPDATE_SHOW_DESK:
                UpdateShowDesk(message as List<CardDto>);
                break;
            default:
                break;
        }
    }


    /// <summary>
    /// 自身的卡牌管理
    /// </summary>
    private List<CardCtrl> cardCtrlList;
    private Transform cardParent;
    void Start()
    {
        cardParent = transform.Find("CardPoint");
        cardCtrlList = new List<CardCtrl>();
    }


    private void UpdateShowDesk(List<CardDto> cardList)
    {
        // 33   34567 
        //34567 3
        if (cardCtrlList.Count > cardList.Count)
        {
            //原来比现在多  原来多  现在显示的要少
            //显示的牌 
            int index = 0;
            foreach (var item in cardCtrlList)
            {
                if (cardList.Count == 0) break;
                else
                {
                    item.gameObject.SetActive(true);
                    item.Init(cardList[index], index, true);
                    index++;
                    //没有拍了
                    if (index == cardList.Count)
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
        else
        {
            //原来比现在的少
            //复用之前创建的卡牌
            int index = 0;
            foreach (var cardCtrl in cardCtrlList)
            {
                cardCtrl.gameObject.SetActive(true);
                cardCtrl.Init(cardList[index], index, true);
                index++;
            }
            //在创建新的N张卡牌
            GameObject cardPrefab = Resources.Load<GameObject>("Card/MyCard");
            for (int i = index; i < cardList.Count; i++)
            {
                CreatCard(cardList[i], i, cardPrefab);
            }
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
