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

    private void CreatCard(int index, GameObject cardPrefab)
    {
        GameObject card = Instantiate(cardPrefab, cardParent);

        card.GetComponent<SpriteRenderer>().sortingOrder = index;
        card.transform.localPosition = new Vector2((index * 0.15f), 0);
    }
}
