using Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
/// <summary>
/// 账户网络消息处理类
/// </summary>
public class AccountHandler : HandlerBase
{
    public override void OnReceive(int subCode, object value)
    {
        switch (subCode)
        {
            case AccountCode.LOGIN:
                LoginResponse((int)value);
                break;

            case AccountCode.REGIST_SRES: 
                RegistResponse((int)value);
                break;
        }
    }

    private PromptMsg promptMsg = new PromptMsg();
    /// <summary>
    /// 登录响应
    /// </summary>
    /// <param name="value"></param>
    private void LoginResponse(int result)
    {
        switch (result)
        {
            case 0:
                LoadSceneMsg msg = new LoadSceneMsg(1,
                    delegate ()
                {
                    //向服务器获取信息
                    SocketMsg socketMsg = new SocketMsg(OpCode.USER,UserCode.GET_INFO_CREQ,null);
                    Dispatch(AreaCode.NET,0, socketMsg);
                });
                Dispatch(AreaCode.SCENE,SceneEvent.LOAD_SCENE,msg);
                break;
            case -1:
                promptMsg.ChangeText("账号错误", Color.red);
                Dispatch(AreaCode.UI, UIEvent.PROMPTA_ANIM, promptMsg);
                break;
            case -2:
                promptMsg.ChangeText("账号密码不匹配", Color.red);
                Dispatch(AreaCode.UI, UIEvent.PROMPTA_ANIM, promptMsg);
                break;
            case -3:
                promptMsg.ChangeText("账号已登录", Color.red);
                Dispatch(AreaCode.UI, UIEvent.PROMPTA_ANIM, promptMsg);
                break;
        }

        //if (result == "登录成功")
        //{
        //    promptMsg.ChangeText(result.ToString(), Color.green);
        //    Dispatch(AreaCode.UI,UIEvent.PROMPTA_ANIM,promptMsg);
        //    return;
        //}
        //promptMsg.ChangeText(result.ToString(), Color.red);
        //Dispatch(AreaCode.UI, UIEvent.PROMPTA_ANIM, promptMsg);

    }
    /// <summary>
    /// 注册响应
    /// </summary>
    private void RegistResponse(int result)
    {
        switch (result)
        {
            case 0:
                promptMsg.ChangeText("注册成功", Color.green);
                Dispatch(AreaCode.UI, UIEvent.PROMPTA_ANIM, promptMsg);
                break;
            case -1:
                promptMsg.ChangeText("账号已存在", Color.red);
                Dispatch(AreaCode.UI, UIEvent.PROMPTA_ANIM, promptMsg);
                break;
            case -2:
                promptMsg.ChangeText("账号为空", Color.red);
                Dispatch(AreaCode.UI, UIEvent.PROMPTA_ANIM, promptMsg);
                break;
            case -3:
                promptMsg.ChangeText("密码不合法", Color.red);
                Dispatch(AreaCode.UI, UIEvent.PROMPTA_ANIM, promptMsg);
                break;
        }

        //if (result == "注册成功")
        //{
        //    promptMsg.ChangeText(result.ToString(), Color.green);
        //    Dispatch(AreaCode.UI, UIEvent.PROMPTA_ANIM, promptMsg);
        //    return;
        //}
        //promptMsg.ChangeText(result.ToString(), Color.red);
        //Dispatch(AreaCode.UI, UIEvent.PROMPTA_ANIM, promptMsg);

    }
}

