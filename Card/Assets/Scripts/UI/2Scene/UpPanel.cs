using Protocol.Dto.Fight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpPanel : UIBase
{

    private void Awake()
    {
        Bind(UIEvent.SET_TABLE_CARDS);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.SET_TABLE_CARDS:
                SetCards(message as List<CardDto>);
                break;
            default:
                break;
        }
    }

    private Image[] imageCards;
    void Start()
    {
        imageCards = new Image[3];

        imageCards[0] = transform.Find("imgCard1").GetComponent<Image>();
        imageCards[1] = transform.Find("imgCard2").GetComponent<Image>();
        imageCards[2] = transform.Find("imgCard3").GetComponent<Image>();

    }

    /// <summary>
    /// 设置底牌
    /// 卡牌数据类  暂无  用obj代替
    /// </summary>
    /// <param name="cards"></param>
    private void SetCards(List<CardDto> cards)
    {
        imageCards[0].sprite = Resources.Load<Sprite>("Poker/" + cards[0].name);
        imageCards[1].sprite = Resources.Load<Sprite>("Poker/" + cards[1].name);
        imageCards[2].sprite = Resources.Load<Sprite>("Poker/" + cards[2].name);
    }
}
