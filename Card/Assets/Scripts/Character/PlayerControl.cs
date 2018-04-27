using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : CharacterBase 
{
    /// <summary>
    /// 卡牌父物体
    /// </summary>
    private Transform cardParent;
    void Start()
    {
        cardParent = transform.Find("CardPoint");

        cardObjectList = new List<GameObject>();
    }

    protected IEnumerator InitCardList()
    {

        GameObject cardPrefab = Resources.Load<GameObject>("Card/OtherCard");
        for (int i = 0; i < 17; i++)
        {
            //创建卡牌
            CreatCard(i, cardPrefab);
            yield return new WaitForSeconds(0.1f);
        }
    }
    /// <summary>
    /// 创建卡牌游戏物体
    /// </summary>
    /// <param name="index"></param>
    /// <param name="cardPrefab"></param>
    private void CreatCard(int index, GameObject cardPrefab)
    {
        GameObject card = Instantiate(cardPrefab, cardParent);

        card.GetComponent<SpriteRenderer>().sortingOrder = index;
        card.transform.localPosition = new Vector2((index * 0.15f), 0);

        this.cardObjectList.Add(card);
    }

    /// <summary>
    /// 添加底牌的方法
    /// </summary>
    /// <param name="cardList"></param>
    protected void AddTableCard()
    {
        //在创建新的3张卡牌
        GameObject cardPrefab = Resources.Load<GameObject>("Card/OtherCard");
        for (int i = 0; i < 3; i++)
        {
            CreatCard(i, cardPrefab);
        }

    }

    //左右两边卡牌的游戏物体
    private List<GameObject> cardObjectList;

    /// <summary>
    /// 移除卡牌
    /// </summary>
    protected void RemoveCard(int cardCount)
    {
        for (int i = cardCount; i < cardObjectList.Count; i++)
        {
            cardObjectList[i].SetActive(false);
        } 
    }
}
