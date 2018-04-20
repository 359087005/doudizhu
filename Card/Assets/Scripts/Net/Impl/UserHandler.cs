using Assets.Scripts.GameDataModel;
using Protocol;
using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 角色网络消息处理类
/// </summary>
public class UserHandler : HandlerBase 
{
    public override void OnReceive(int subCode, object value)
    {
        switch (subCode)
        {
            case UserCode.CREAT_SRES:
                CreatResponse((int)value);
                break;
            case UserCode.GET_INFO_SRES:
                GetInfoResponse(value as UserDto);
                break;
            case UserCode.ONLINE_SRES:
                OnLineResponse((int)value);
                break;
        }
    }
    SocketMsg socketMsg = new SocketMsg();
    /// <summary>
    /// 获取消息的回应
    /// </summary>
    /// <param name="dto"></param>
    private void GetInfoResponse(UserDto dto)
    {
        if (dto == null) //没有角色
        {
            //创建角色面板打开
            Dispatch(AreaCode.UI,UIEvent.CREAT_PANEL_ACTIVE,true);
        }
        else
        {
            //有角色 关闭角色面板    角色上线
            Dispatch(AreaCode.UI, UIEvent.CREAT_PANEL_ACTIVE, false);
            //socketMsg.Change(OpCode.USER, UserCode.ONLINE_CREQ, null);
            //Dispatch(AreaCode.NET, 0, socketMsg);
            //保存数据到本地
            Model.gameModel.UserDto = dto;

            //更新一下本地显示 刷新UI
            Dispatch(AreaCode.UI,UIEvent.REFRESH_INFO_PANEL,dto);
        }
    }

    private void OnLineResponse(int result)
    {
        if (result == 0)
        {
            //上线成功
            Debug.Log("上线成功");
        }
        else if (result == -2)
        {
            //没有角色
            Debug.LogError("OnLineResponseErrorCode-2");
        }
        else if (result == -1)
        {
            //客户端非法登录
            Debug.LogError("OnLineResponseErrorCode-1");
        }

    }

    /// <summary>
    /// 创建角色的响应
    /// </summary>
    /// <param name="result"></param>
    private void CreatResponse(int result)
    {
        if (result == 0)
        {
            //创建成功
            //隐藏创建面板  
            Dispatch(AreaCode.UI, UIEvent.CREAT_PANEL_ACTIVE, false);
            //获取角色信息
            socketMsg.Change(OpCode.USER, UserCode.GET_INFO_CREQ, null);
            Dispatch(AreaCode.NET, 0, socketMsg);
        }
        else if (result == -1)
        {
            Debug.LogError("CreatResponseErrorCode-1");
        }
        else if (result == -2)
        {
            Debug.LogError("CreatResponseErrorCode-2");
        }
    }
}
