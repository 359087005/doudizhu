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

        SetPanelActive(false);
    }

    void BtnRegistClick()
    {
        if (string.IsNullOrEmpty(id.text)) return;
        if (string.IsNullOrEmpty(password.text)) return;
        if (string.IsNullOrEmpty(repeat.text)) return;

        //TODO  和服务器交互
    }
    void CloseClick()
    {
        SetPanelActive(false);
    }

    public override void Destroy()
    {
        base.Destroy();

        btnRegist.onClick.RemoveListener(BtnRegistClick);
        btnClose.onClick.RemoveListener(CloseClick);
    }


}
