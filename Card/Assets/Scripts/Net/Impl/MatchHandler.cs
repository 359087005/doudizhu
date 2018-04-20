﻿using Assets.Scripts.GameDataModel;
using Protocol;
using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchHandler : HandlerBase 
{
    public override void OnReceive(int subCode, object value)
    {
        switch (subCode)
        {
            case MatchCode.ENTER_SRES:
                EnterResponse(value as MatchRoomDto);
                break;
            case MatchCode.ENTER_BRO:
                EnterBroadcast(value as UserDto);
                break;
            case MatchCode.LEAVE_BRO:
                LeaveBro((int)value);
                break;
            case MatchCode.READY_BRO:
                ReadyBro((int)value);
                break;
            case MatchCode.START_BRO:
                StartBro();
                break;
        }
    }

    PromptMsg msg = new PromptMsg();
    /// <summary>
    /// 自身进入的服务器响应
    /// </summary>
    /// <param name="dto"></param>
    private void EnterResponse(MatchRoomDto dto)
    {
        //数据保存到本地
        Model.gameModel.matchRoomDto = dto;

        //显示进入房间的按钮
        Dispatch(AreaCode.UI,UIEvent.SHOW_ENTER_ROOM_BUTTON,true);

    }
    /// <summary>
    /// 他人进入的的处理
    /// </summary>
    private void EnterBroadcast(UserDto newUser)
    {
        //更新房间数据  
        Model.gameModel.matchRoomDto.Add(newUser);
        //发消息  打开玩家状态面板所有物体
        Dispatch(AreaCode.UI, UIEvent.PLAYER_ENTER, newUser.id);
        //告诉用户玩家进入
        msg.ChangeText(newUser.name + "进入了游戏",Color.yellow);
        Dispatch(AreaCode.UI,UIEvent.PROMPTA_ANIM,msg);
    }
    
    /// <summary>
    /// 离开
    /// </summary>
    /// <param name="leaveUserId"></param>
    private void LeaveBro(int leaveUserId)
    {
        //发消息  隐藏玩家状态面板所有物体
        Dispatch(AreaCode.UI,UIEvent.PLAYER_LEAVE,leaveUserId);
        
        Model.gameModel.matchRoomDto.Leave(leaveUserId);
    }
    /// <summary>
    /// 开始游戏的广播处理
    /// </summary>
    private void StartBro()
    {
        msg.ChangeText("game start",Color.blue);
        Dispatch(AreaCode.UI,UIEvent.PROMPTA_ANIM,msg);
        Dispatch(AreaCode.UI,UIEvent.PLAYER_HIDE_STATE,null);
    }
    /// <summary>
    /// 准备的广播处理
    /// </summary>
    private void ReadyBro(int readyUserId)
    {
        //保存数据
        Model.gameModel.matchRoomDto.Ready(readyUserId);

        //显示为玩家状态面板准备的文字
        Dispatch(AreaCode.UI,UIEvent.PLAYER_READY, readyUserId);
    }
}
