using Assets.Scripts.GameDataModel;
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
        GameModel gModel =  Model.gameModel;
        gModel.matchRoomDto = dto;
        
        //重置玩家的位置信息
        //gModel.matchRoomDto.ResetPositon(userId);
        //打开玩家数据 
        ResetPositon();
        //自身的角色是肯定存在的 
        int userId = gModel.UserDto.id;
        UserDto myUseDto = dto.uIdUserDtoDict[userId];

        //显示进入房间的按钮
        Dispatch(AreaCode.UI, UIEvent.SHOW_ENTER_ROOM_BUTTON, true);

    }
    /// <summary>
    /// 他人进入的的处理
    /// </summary>
    private void EnterBroadcast(UserDto newUser)
    {
        MatchRoomDto room = Model.gameModel.matchRoomDto;
        //更新房间数据  
        room.Add(newUser);
        //重置玩家位置
        //显示现在房间内的玩家
        ResetPositon();
        if (room.leftId != -1)
        {
            UserDto leftDto = room.uIdUserDtoDict[room.leftId];
            Dispatch(AreaCode.UI, UIEvent.SET_LEFT_PLAYER_DATA, leftDto);
        }
        if (room.rightId != -1)
        {
            UserDto rightDto = room.uIdUserDtoDict[room.rightId];
            Dispatch(AreaCode.UI, UIEvent.SET_RIGHT_PLAYER_DATA, rightDto);
        }
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
        ResetPositon();
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
        //发送消息 隐藏准备按钮
        Dispatch(AreaCode.UI, UIEvent.PLAYER_HIDE_READY_BUTTON, null);
    }

    /// <summary>
    /// 重置玩家位置  打开数据
    /// </summary>
    private void ResetPositon()
    {
        GameModel gModel = Model.gameModel;
        MatchRoomDto matchRoom = gModel.matchRoomDto;
        //重置玩家位置
        matchRoom.ResetPositon(gModel.UserDto.id);
        //显示现在房间内的玩家
        //if (matchRoom.leftId != -1)
        //{
        //    UserDto leftDto = matchRoom.uIdUserDtoDict[matchRoom.leftId];
        //    Dispatch(AreaCode.UI, UIEvent.SET_LEFT_PLAYER_DATA, leftDto);
        //}
        //if (matchRoom.rightId != -1)
        //{
        //    UserDto rightDto = matchRoom.uIdUserDtoDict[matchRoom.rightId];
        //    Dispatch(AreaCode.UI, UIEvent.SET_RIGHT_PLAYER_DATA, rightDto);
        //}
    }
}
