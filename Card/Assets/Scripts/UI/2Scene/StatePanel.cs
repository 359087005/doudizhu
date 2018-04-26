using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Protocol.Dto;
public class StatePanel : UIBase 
{
    protected virtual void Awake()
    {
        Bind(UIEvent.PLAYER_READY, UIEvent.PLAYER_HIDE_STATE, UIEvent.PLAYER_LEAVE, UIEvent.PLAYER_ENTER
            ,UIEvent.PLAYER_CHAT);
    }
    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.PLAYER_READY:
                {
                    if (dto == null) break;
                    int userId = (int)message;
                    if (userId == dto.id)
                        ReadyState();
                    break;
                }
            case UIEvent.PLAYER_HIDE_STATE:
                {
                    textReady.gameObject.SetActive(false);
                    break;
                }
            case UIEvent.PLAYER_LEAVE:
                {
                    if (dto == null) break;
                    int userId = (int)message;
                    if (userId == dto.id)
                        SetPanelActive(false);
                    break;
                }
            case UIEvent.PLAYER_ENTER:
                {
                    if (dto == null) break;
                    int userId = (int)message;
                    if (userId == dto.id)
                        SetPanelActive(true);
                    break;
                }

            case UIEvent.PLAYER_CHAT:
                {
                    if (dto == null) break;
                    ChatMsg msg = message as ChatMsg;
                    if (dto.id == msg.userId)
                    {
                        imgChat.gameObject.SetActive(true);
                        ShowContent(msg.text);
                    }
                        break;
                }
        }
    }

    /// <summary>
    /// 角色数据
    /// </summary>
    protected UserDto dto;

    protected Image imgIdentity;
    protected Text textReady;
    protected Image imgChat;
    protected Text text;

    protected virtual void Start()
    {
        imgIdentity = transform.Find("imgIdentity").GetComponent<Image>();
        textReady = transform.Find("textReady").GetComponent<Text>();
        imgChat = transform.Find("imgChat").GetComponent<Image>();
        text = imgChat.transform.Find("Text").GetComponent<Text>();

        textReady.gameObject.SetActive(false);
        imgChat.gameObject.SetActive(false);
    }
    
    protected virtual void ReadyState()
    {
        textReady.gameObject.SetActive(true);
    }

    /// <summary>
    /// 设置身份 是农民还是地主   
    /// 0 是农民 1 是地主
    /// </summary>
    /// <param name="identity"></param>
    protected void SetIdentity(int identity)
    {
        string identityStr = identity == 0 ? "Farmer" : "Landlord";
        imgIdentity.sprite = Resources.Load<Sprite>("Identity/" + identityStr);
    }
    /// <summary>
    /// chat面板显示时间
    /// </summary>
    protected float showTime = 3f;

    protected float timer = 0f;
    /// <summary>
    /// 是否显示
    /// </summary>
    protected bool isShow = false;

    protected virtual void Update()
    {
        timer += Time.deltaTime;
        if (isShow == true)
        {
            SetChatActive(true);
            timer = 0f;
            isShow = false;
            
        }
    }

    protected void SetChatActive(bool active)
    {
        imgChat.gameObject.SetActive(active);
    }

    /// <summary>
    /// 设置内容
    /// </summary>
    /// <param name="content">显示的文字</param>
    protected void ShowContent(string content)
    {
        text.text = content;
        //设置完毕 开始显示啊！！！
        SetChatActive(true);
        isShow = true;
    }
}
