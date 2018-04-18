using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchPanel : UIBase
{
    private void Awake()
    {
        Bind(UIEvent.SHOW_ENTER_ROOM_BUTTON);
    }
    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.SHOW_ENTER_ROOM_BUTTON:
                break;

        }
    }


    private Button btnMatch;
    private Image imageBG;
    private Text textDes;
    private Button btnCancel;
    private Button btnEnter;
    void Start()
    {
        btnMatch = transform.Find("BtnMatch").GetComponent<Button>();
        imageBG =transform.Find("ImageBG").GetComponent<Image>();
        textDes = transform.Find("TextDes").GetComponent<Text>();
        btnCancel = transform.Find("BtnCancel").GetComponent<Button>();
        btnEnter = transform.Find("BtnEnter").GetComponent<Button>();

        btnMatch.onClick.AddListener(BtnMathchClick);
        btnCancel.onClick.AddListener(BtnCancelClick);
        btnEnter.onClick.AddListener(BtnEnterClick);

        SetObjectActive(false);
        btnEnter.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!textDes.IsActive())
            return;
        timer += Time.deltaTime;
        if (timer >= intervalTime)
        {
            DesTextAnim();
            timer = 0f;
        }
    }

    private void SetObjectActive(bool active)
    {
        imageBG.gameObject.SetActive(active);
        textDes.gameObject.SetActive(active);
        btnCancel.gameObject.SetActive(active);
    }

    private string default_Des = "正在寻找房间";
    private int dotCount = 0;
    private float intervalTime = 1f;
    private float timer = 0f;
    private void DesTextAnim()
    {
        textDes.text = default_Des;

        dotCount++;
        if (dotCount > 4)
            dotCount = 1;
        for (int i = 0; i < dotCount; i++)
        {
            textDes.text += ".";
        }
    }
    public override void Destroy()
    {
        base.Destroy();

        btnMatch.onClick.RemoveListener(BtnMathchClick);
        btnCancel.onClick.RemoveListener(BtnCancelClick);
        btnEnter.onClick.RemoveListener(BtnEnterClick);
    }
    
    private void BtnMathchClick()
    {
        //TODO
        SetObjectActive(true);
      
    }
    private void BtnCancelClick()
    {
        //TODO
        SetObjectActive(false);
    }
    private void BtnEnterClick()
    {
        //TODO
    }
}
