using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetPanel : UIBase 
{
 private void Awake()
    {
        Bind(UIEvent.SET_PANEL_ACTIVE);
    }
    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.SET_PANEL_ACTIVE:
                SetPanelActive((bool)message);
                break;
        }
    }

    private Button btn_Close;
    private Toggle toggle_Audio;
    private Button btn_Quit;
    private Slider sld_Volume;
	void Start ()
    {
        btn_Close = transform.Find("BtnClose").GetComponent<Button>();
        btn_Quit = transform.Find("BtnQuit").GetComponent<Button>();
        toggle_Audio = transform.Find("ToggleAudio").GetComponent<Toggle>();
        sld_Volume = transform.Find("SliderVolume").GetComponent<Slider>();

        btn_Close.onClick.AddListener(BtnCloseClick);
        btn_Quit.onClick.AddListener(BtnQuitClick);
        toggle_Audio.onValueChanged.AddListener(ToggleClick);
        sld_Volume.onValueChanged.AddListener(SldVolumeClick);

        SetPanelActive(false);
    }

    void ToggleClick(bool value)
    {
        //TODO
    }
    void SldVolumeClick(float value)
    {
        //TODO
    }
    void BtnCloseClick()
    {
        SetPanelActive(false);
    }

    void BtnQuitClick()
    {
        Application.Quit();
    }

    public override void Destroy()
    {
        base.Destroy();
        btn_Close.onClick.RemoveListener(BtnCloseClick);
        btn_Quit.onClick.RemoveListener(BtnQuitClick);
        toggle_Audio.onValueChanged.RemoveListener(ToggleClick);
        sld_Volume.onValueChanged.RemoveListener(SldVolumeClick);
    }

}
