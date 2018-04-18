using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PromptPanle : UIBase
{
    private Text txt;
    private CanvasGroup cg;

    [SerializeField]
    [Range(0, 3)]
    private float showTime = 1f;
    private float timer = 0f;

    private void Awake()
    {
        Bind(UIEvent.PROMPTA_ANIM);
    }
    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.PROMPTA_ANIM:
                PromptMsg msg = message as PromptMsg;
                PromptMsg(msg.txt, msg.color);
                break;
        }

    }

    void Start()
    {
        txt = transform.Find("Text").GetComponent<Text>();
        cg = transform.Find("Text").GetComponent<CanvasGroup>();
    }



    private void PromptMsg(string txt, Color color)
    {
        this.txt.text = txt;
        this.txt.color = color;
        cg.alpha = 0;

        //播放动画
        StartCoroutine(PromptAnim());
    }

    IEnumerator PromptAnim()
    {
        while (cg.alpha < 1f)
        {
            cg.alpha += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        while (timer < showTime)
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        while (cg.alpha > 0f)
        {
            cg.alpha -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

    }
}
