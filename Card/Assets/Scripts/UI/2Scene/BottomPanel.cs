using Assets.Scripts.GameDataModel;
using Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomPanel : UIBase
{
    private void Awake()
    {
        Bind(UIEvent.CHANGE_MUTIPLE);
    }
    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.CHANGE_MUTIPLE:
                ChangeMutiple((int)message);
                break;
            default:
                break;
        }
    }
    Text textBeen;
    Text textMutiple;
    Button btnChat;
    GameObject imgChoose;
    Button[] btns;

    SocketMsg socketMsg;
    private void Start()
    {
        textBeen = transform.Find("textBeen").GetComponent<Text>();
        textMutiple = transform.Find("textMutiple").GetComponent<Text>();
        btnChat = transform.Find("btnChat").GetComponent<Button>();
        imgChoose = transform.Find("imgChoose").gameObject;
        socketMsg = new SocketMsg();
        btns = new Button[7];

        for (int i = 0; i < 7; i++)
        {
            int j = i;
            btns[i] = imgChoose.transform.GetChild(i).GetComponent<Button>();
        }
        btns[0].onClick.AddListener(OnChatClick1);
        btns[1].onClick.AddListener(OnChatClick2);
        btns[2].onClick.AddListener(OnChatClick3);
        btns[3].onClick.AddListener(OnChatClick4);
        btns[4].onClick.AddListener(OnChatClick5);
        btns[5].onClick.AddListener(OnChatClick6);
        btns[6].onClick.AddListener(OnChatClick7);
        btnChat.onClick.AddListener(SetChooseActive);

        imgChoose.SetActive(false);

        RefreshPanel(Model.gameModel.UserDto.been);

    }


    /// <summary>
    /// 刷新自身面板信息
    /// </summary>
    private void RefreshPanel(int beenCount)
    {
        textBeen.text = " x " + beenCount;
    }
    private void ChangeMutiple(int mutiple)
    {
        textMutiple.text = "倍数x " + mutiple;
    }

    private void SetChooseActive()
    {
        imgChoose.SetActive(!imgChoose.activeSelf);
    }

    private void OnChatClick1()
    {
        socketMsg.Change(OpCode.CHAT, ChatCode.CREQ, 1);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }
    private void OnChatClick2()
    {
        socketMsg.Change(OpCode.CHAT, ChatCode.CREQ, 2);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }
    private void OnChatClick3()
    {
        socketMsg.Change(OpCode.CHAT, ChatCode.CREQ, 3);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }
    private void OnChatClick4()
    {
        socketMsg.Change(OpCode.CHAT, ChatCode.CREQ, 4);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }
    private void OnChatClick5()
    {
        socketMsg.Change(OpCode.CHAT, ChatCode.CREQ, 5);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }
    private void OnChatClick6()
    {
        socketMsg.Change(OpCode.CHAT, ChatCode.CREQ, 6);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }
    private void OnChatClick7()
    {
        socketMsg.Change(OpCode.CHAT, ChatCode.CREQ, 7);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }

    public override void Destroy()
    {
        base.Destroy();
        btnChat.onClick.RemoveListener(SetChooseActive);

        btns[0].onClick.RemoveListener(OnChatClick1);
        btns[1].onClick.RemoveListener(OnChatClick2);
        btns[2].onClick.RemoveListener(OnChatClick3);
        btns[3].onClick.RemoveListener(OnChatClick4);
        btns[4].onClick.RemoveListener(OnChatClick5);
        btns[5].onClick.RemoveListener(OnChatClick6);
        btns[6].onClick.RemoveListener(OnChatClick7);
    }
}
