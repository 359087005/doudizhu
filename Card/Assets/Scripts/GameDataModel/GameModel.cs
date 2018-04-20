using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 游戏数据的存储类
/// </summary>
public class GameModel  
{
    //登录用户的数据
    public UserDto UserDto { get; set; }

    //匹配用户的数据 
    public MatchRoomDto matchRoomDto { get; set; }
}
