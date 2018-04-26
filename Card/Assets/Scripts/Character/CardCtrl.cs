using Protocol.Dto.Fight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 卡牌控制类 
/// </summary>
public class CardCtrl : MonoBehaviour 
{
    //数据
    public CardDto cardDto { get; set; }
    //卡牌是否被选中
    public bool isSelected { get; set; }

    private SpriteRenderer spriteRenderer;
    private bool isMine;
    /// <summary>
    /// 卡牌初始化
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="index"></param>
    /// <param name="isMine"></param>
    public void Init(CardDto dto,int index,bool isMine)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        this.cardDto = dto;
        this.isMine = isMine;
        if (isSelected == true)
        {
            isSelected = false;
            transform.localPosition -= new Vector3(0,0.2f,0);
        }

        //加载图片
        string path = string.Empty;
        if (isMine)
        {
            path = "Poker/" + dto.name;
        }
        else
        {
            path = "Poker/CardBack";
        }
        Sprite sp = Resources.Load<Sprite>(path);
        spriteRenderer.sprite = sp;
        //这里是为了 让后一张牌盖住前一张。防止牌胡乱覆盖。不懂就去弄几张牌 改下order！
        spriteRenderer.sortingOrder = index;
    }

    private void OnMouseDown()
    {
        if (isMine == false)
            return;
        this.isSelected = !isSelected;
        if (isSelected == true)
        {
            transform.localPosition += new Vector3(0, 0.2f, 0);
        }
        else
        {
            transform.localPosition -= new Vector3(0, 0.2f, 0);
        }
    }
}
