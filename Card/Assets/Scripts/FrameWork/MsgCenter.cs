using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 消息处理中心 负责消息的发送
///     根据不同的模块，发送不同的消息，做不同的事情。
/// </summary>
public class MsgCenter : MonoBase
{
    public static MsgCenter _Instance = null;
    private void Awake()
    {
        _Instance = this;
        gameObject.AddComponent<AudioManager>();
        gameObject.AddComponent<UIManager>();
        gameObject.AddComponent<CharacterManager>();
    }

    public void DisPatch(int areaCode,int eventCode,object message)
    {
        switch (areaCode)
        {
            case AreaCode.AUDIO:
                AudioManager._Instance.Execute(eventCode,message);
                break;
            case AreaCode.CHARACTER:
                CharacterManager._Instance.Execute(eventCode,message);
                break;
            case AreaCode.GAME:
                break;
            case AreaCode.NET:
                break;
            case AreaCode.UI:
                UIManager._Instance.Execute(eventCode, message);
                break;


            default:
                break;
        }
    }
}
