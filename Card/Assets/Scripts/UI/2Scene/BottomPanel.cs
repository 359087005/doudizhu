using Assets.Scripts.GameDataModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomPanel : UIBase
{
    private void Awake()
    {

    }
    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            default:
                break;
        }
    }
    Text textBeen;
    Text textMutiple;
    Button btnChat;
    GameObject imgChoose;
    Button[] btns;

    private void Start()
    {
        textBeen = transform.Find("textBeen").GetComponent<Text>();
        textMutiple = transform.Find("textMutiple").GetComponent<Text>();
        btnChat = transform.Find("btnChat").GetComponent<Button>();
        imgChoose = transform.Find("imgChoose").gameObject;
        btns = new Button[7];
        for (int i = 0; i < 7; i++)
        {
            btns[i] = imgChoose.transform.GetChild(i).GetComponent<Button>();
        }

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

    public override void Destroy()
    {
        base.Destroy();
        btnChat.onClick.RemoveListener(SetChooseActive);
    }
}
