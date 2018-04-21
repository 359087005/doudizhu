using Protocol;
using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyStatePanel : StatePanel
{

    protected override void Awake()
    {
        base.Awake();
        Bind(UIEvent.SHOW_GRAB_BUTTON,UIEvent.SHOW_DEAL_BUTTON,UIEvent.SET_MY_PLAYER_DATA,UIEvent.PLAYER_HIDE_READY_BUTTON);
    }
    public override void Execute(int eventCode, object message)
    {
        base.Execute(eventCode, message);
        switch (eventCode)
        {
            case UIEvent.SHOW_GRAB_BUTTON:
                {
                    bool active = (bool)message;
                    btnNGrab.gameObject.SetActive(active);
                    btnGrab.gameObject.SetActive(active);
                }
                break;
            case UIEvent.SHOW_DEAL_BUTTON:
                {
                    bool active = (bool)message;
                    btnDeal.gameObject.SetActive(active);
                    btnNDeal.gameObject.SetActive(active);
                }
                break;
            case UIEvent.SET_MY_PLAYER_DATA:
                {
                    this.dto = message as UserDto;
                }
                break;
            case UIEvent.PLAYER_HIDE_READY_BUTTON:
                btnReady.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }

    private Button btnReady;
    private Button btnDeal;
    private Button btnNDeal;
    private Button btnGrab;
    private Button btnNGrab;

    private SocketMsg socketMsg;
    protected override void Start()
    {
        base.Start();
        btnReady = transform.Find("btnReady").GetComponent<Button>();
        btnDeal = transform.Find("btnDeal").GetComponent<Button>();
        btnNDeal = transform.Find("btnNDeal").GetComponent<Button>();
        btnGrab = transform.Find("btnGrab").GetComponent<Button>();
        btnNGrab = transform.Find("btnNGrab").GetComponent<Button>();

        btnReady.onClick.AddListener(BtnReadyClick);
        btnDeal.onClick.AddListener(BtnDealClick);
        btnNDeal.onClick.AddListener(BtnNDealClick);
        btnGrab.onClick.AddListener(BtnGrabClick);
        btnNGrab.onClick.AddListener(BtnNGrabClick);

        socketMsg = new SocketMsg();
        btnDeal.gameObject.SetActive(false);
        btnNDeal.gameObject.SetActive(false);
        btnGrab.gameObject.SetActive(false);
        btnNGrab.gameObject.SetActive(false);
    }

    protected override void ReadyState()
    {
        base.ReadyState();
        btnReady.gameObject.SetActive(false);
    }


    private void BtnReadyClick()
    {
        //向服务器发送准备
        socketMsg.Change(OpCode.MATCH,MatchCode.READY_CREQ,null);
        Dispatch(AreaCode.NET,0,socketMsg);
    }
    private void BtnDealClick()
    {

    }
    private void BtnNDealClick()
    {

    }
    private void BtnGrabClick()
    {

    }
    private void BtnNGrabClick()
    {

    }

    public override void Destroy()
    {
        base.Destroy();
        btnReady.onClick.RemoveListener(BtnReadyClick);
        btnDeal.onClick.RemoveListener(BtnDealClick);
        btnNDeal.onClick.RemoveListener(BtnNDealClick);
        btnGrab.onClick.RemoveListener(BtnGrabClick);
        btnNGrab.onClick.RemoveListener(BtnNGrabClick);
    }
}
