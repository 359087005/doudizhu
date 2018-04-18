using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : UIBase
{
    private void Awake()
    {
        Bind(UIEvent.REFRESH_INFO_PANEL);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.REFRESH_INFO_PANEL:
                //Refresh(); TODO
                break;
            default:
                break;
        }
    }

    private Text textName;
    private Text textLV;

    private Slider sliderExp;
    private Text textExp_Num;

    private Text textBeen;

    private Button btn_Set;
    void Start()
    {
        textName = transform.Find("TextName").GetComponent<Text>();
        textLV = transform.Find("TextLV").GetComponent<Text>();
        sliderExp = transform.Find("SldExp").GetComponent<Slider>();
        textExp_Num = transform.Find("TextExp_Num").GetComponent<Text>();
        textBeen = transform.Find("TextBeen").GetComponent<Text>();

        btn_Set = transform.Find("BtnSet").GetComponent<Button>();

        btn_Set.onClick.AddListener(BtnSetClick);
    }

    private void BtnSetClick()
    {
        Dispatch(AreaCode.UI, UIEvent.SET_PANEL_ACTIVE, true);
    }
    public override void Destroy()
    {
        base.Destroy();
        btn_Set.onClick.RemoveListener(BtnSetClick);
    }

    /// <summary>
    /// 刷新视图
    ///       名字 等级  经验 豆子
    /// </summary>
    private void Refresh(string name, int level, int exp, int been)
    {
        //todo
        textName.text = name;
        textLV.text = "Lv." + level;
        textExp_Num.text = exp + " / " + level * 100;
        sliderExp.value = (float)exp / level * 100;
        textBeen.text = been.ToString();
    }
}
