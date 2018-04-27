using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEvent  {

    public const int START_PANEL_ACTICE = 0;
    public const int REGIST_PANEL_ACTICE = 1;

    public const int SET_PANEL_ACTIVE = 101;
    public const int CREAT_PANEL_ACTIVE = 102;

    public const int REFRESH_INFO_PANEL = 2;
    public const int SHOW_ENTER_ROOM_BUTTON = 3;

    public const int SET_TABLE_CARDS = 5; //设置底牌

    //public const int SET_MY_PLAYER_DATA = 16;//设置自身数据  425
    public const int SET_LEFT_PLAYER_DATA = 6; //设置左边玩家的数据
    public const int PLAYER_READY = 7; //角色准备
    public const int PLAYER_ENTER = 8;//角色进入
    public const int PLAYER_LEAVE = 9;//角色离开
    public const int PLAYER_CHAT = 10;//角色聊天
    public const int PLAYER_CHANGE_IDENTITY = 11;//角色切换身份
    public const int PLAYER_HIDE_STATE = 12;//隐藏角色状态面板：当游戏开始的时候  已准备文字需要关闭

    public const int SET_RIGHT_PLAYER_DATA = 13;//设置右边玩家的数据

    public const int SHOW_GRAB_BUTTON = 14;//抢地主~~~~~~~
    public const int SHOW_DEAL_BUTTON = 15;//出牌~
    public const int PLAYER_HIDE_READY_BUTTON = 17;//准备按钮

    public const int CHANGE_MUTIPLE = 18; //改变倍数

    public const int HIDE_DEAL_BUTTON = 19;//关闭隐藏按钮

    public const int PROMPTA_ANIM = int.MaxValue;
}
