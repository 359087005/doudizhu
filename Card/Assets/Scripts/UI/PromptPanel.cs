using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PromptPanel : UIBase
{
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
            default:
                break;
        }
    }

    private Text text;
    private CanvasGroup cg;

    [SerializeField]
    [Range(0, 3)]
    private float showTime = 1f;
    private float timer = 0f;
    void Start()
    {
        text = transform.Find("Text").GetComponent<Text>();
        cg = transform.Find("Text").GetComponent<CanvasGroup>();

        cg.alpha = 0;
    }

    private void PromptMsg(string txt, Color color)
    {
        this.text.text = txt;
        this.text.color = color;
        cg.alpha = 0;
        timer = 0;
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
