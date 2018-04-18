using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatPanel : UIBase 
{
 private void Awake()
    {
        Bind(UIEvent.CREAT_PANEL_ACTIVE);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.CREAT_PANEL_ACTIVE:
                SetPanelActive((bool)message);
                break;
        }
    }

    private Button btnCreat;
    private InputField inputName;
    PromptMsg msg;
    void Start ()
    {
        btnCreat = transform.Find("BtnCreat").GetComponent<Button>();
        inputName = transform.Find("InputName").GetComponent<InputField>();
        msg = new PromptMsg();
    }

    void BtnCreatClick()
    {
        if (string.IsNullOrEmpty(inputName.text))
        {
            msg.ChangeText("你在逗我么？",Color.red);
            Dispatch(AreaCode.UI,UIEvent.PROMPTA_ANIM,msg);
            return;
        }
        //TODO
    }

}
