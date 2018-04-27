using Protocol;
using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StartPanel : UIBase
{
    private Button btnLogin;
    private Button btnClose;

    private InputField textID;
    private InputField textPasword;

    PromptMsg promptMsg;
    SocketMsg socketMsg;

    private void Awake()
    {
        Bind(UIEvent.START_PANEL_ACTICE);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.START_PANEL_ACTICE:
                SetPanelActive((bool)message);
                break;
        }
    }
    void Start()
    {
        btnLogin = transform.Find("BtnLogin").GetComponent<Button>();
        btnClose = transform.Find("BtnClose").GetComponent<Button>();
        textID = transform.Find("ID").GetComponent<InputField>();
        textPasword = transform.Find("Password").GetComponent<InputField>();

        btnLogin.onClick.AddListener(LoginClick);
        btnClose.onClick.AddListener(CloseClick);

        promptMsg = new PromptMsg();
        socketMsg = new SocketMsg();
        SetPanelActive(false);
    }

    void LoginClick()
    {

        if (string.IsNullOrEmpty(textID.text))
        {
            promptMsg.ChangeText("账号不能为空",Color.red);
            Dispatch(AreaCode.UI,UIEvent.PROMPTA_ANIM, promptMsg);
            return;
        }
        if (string.IsNullOrEmpty(textPasword.text))
        {
            promptMsg.ChangeText("密码不能为空", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPTA_ANIM, promptMsg);
            return;
        }

        AccountDto dto = new AccountDto(textID.text, textPasword.text);
        socketMsg.Change(OpCode.ACCOUNT, AccountCode.LOGIN, dto);
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
        btnLogin.onClick.RemoveAllListeners();
        btnClose.onClick.RemoveAllListeners();
    }

    void ClearText()
    {
        textID.text = textPasword.text = string.Empty;
    }
}
