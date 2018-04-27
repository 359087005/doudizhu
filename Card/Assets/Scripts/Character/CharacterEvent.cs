using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterEvent
{
    public const int INIT_MY_CARD = 0;//初始化自己的牌
    public const int INIT_LEFT_CARD = 1;
    public const int INIT_RIGHT_CARD = 2;

    public const int ADD_MY_CARD = 3; //添加自己的地主牌
    public const int ADD_LEFT_CARD = 4;
    public const int ADD_RIGHT_CARD = 5;


    public const int DEAL_CARD = 6; //出牌

    public const int REMOVE_MY_CARD = 7;//移除手牌
    public const int REMOVE_LEFT_CARD = 8;
    public const int REMOVE_RIGHT_CARD = 9;

    public const int UPDATE_SHOW_DESK = 10;
}
