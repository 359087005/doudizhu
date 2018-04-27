using Protocol;
using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterPanel : UIBase
{
    private Button btnRegist;
    private Button btnClose;
    private InputField id;
    private InputField password;
    private InputField repeat;

    private PromptMsg promptMsg;
    private SocketMsg socketMsg;
    private void Awake()
    {
        Bind(UIEvent.REGIST_PANEL_ACTICE);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.REGIST_PANEL_ACTICE:
                SetPanelActive((bool)message);
                break;
        }
    }

    void Start()
    {
        btnRegist = transform.Find("BtnRegister").GetComponent<Button>();
        btnClose = transform.Find("BtnClose").GetComponent<Button>();
        id = transform.Find("ID").GetComponent<InputField>();
        password = transform.Find("Password").GetComponent<InputField>();
        repeat = transform.Find("Repeat").GetComponent<InputField>();

        btnRegist.onClick.AddListener(BtnRegistClick);
        btnClose.onClick.AddListener(CloseClick);

        promptMsg = new PromptMsg();
        socketMsg = new SocketMsg();
        SetPanelActive(false);
    }

    void BtnRegistClick()
    {
        if (string.IsNullOrEmpty(id.text))
        {
            promptMsg.ChangeText("账号不能为空", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPTA_ANIM, promptMsg);
            return;
        }

        if (string.IsNullOrEmpty(password.text))
        {
            promptMsg.ChangeText("密码不能为空", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPTA_ANIM, promptMsg);
            return;
        }
        if (string.IsNullOrEmpty(repeat.text) || repeat.text != password.text)
        {
            promptMsg.ChangeText("账号密码不一致", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPTA_ANIM, promptMsg);
            return;
        }

        AccountDto dto = new AccountDto(id.text, password.text);
        socketMsg.Change(OpCode.ACCOUNT, AccountCode.REGIST_CREQ, dto);
        Dispatch(AreaCode.NET, 0, socketMsg);


        //ClearText();
    }
    void CloseClick()
    {
        SetPanelActive(false);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        btnRegist.onClick.RemoveListener(BtnRegistClick);
        btnClose.onClick.RemoveListener(CloseClick);
    }
    void ClearText()
    {
        id.text = password.text = repeat.text = string.Empty;
    }

}
