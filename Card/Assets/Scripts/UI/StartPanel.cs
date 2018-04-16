using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StartPanel : UIBase
{
    private Button btnLogin;
    private Button btnClose;

    private InputField textID;
    private InputField textPassword;

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
        textPassword = transform.Find("Password").GetComponent<InputField>();

        btnLogin.onClick.AddListener(LoginClick);
        btnClose.onClick.AddListener(CloseClick);


        SetPanelActive(false);
    }

    void LoginClick()
    {
        if (string.IsNullOrEmpty(textID.text))
        {
            return;
        }
        if (string.IsNullOrEmpty(textPassword.text))
        {
            return;
        }

        //TODO  服务器交互
        
    }
    void CloseClick()
    {
        SetPanelActive(false);
    }

    public override void Destroy()
    {
        base.Destroy();
        btnLogin.onClick.RemoveAllListeners();
        btnClose.onClick.RemoveAllListeners();
    }
}
