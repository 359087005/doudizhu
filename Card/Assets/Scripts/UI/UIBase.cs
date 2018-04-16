using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 辅助Base类
/// 
///     消息的绑定，解绑，执行
/// </summary>
public class UIBase : MonoBase
{
    /// <summary>
    /// 自身关心的一个消息集合
    /// </summary>
    List<int> list = new List<int>();


    /// <summary>
    ///绑定不确定个的消息
    /// </summary>
    protected void Bind(params int[] eventCode)
    {
        list.AddRange(eventCode);
        UIManager._Instance.Add(list.ToArray(), this);
    }
    /// <summary>
    /// 解除绑定
    /// </summary>

    protected void UnBind()
    {
        UIManager._Instance.Remove(list.ToArray(), this);
        list.Clear();
    }
    /// <summary>
    ///自动解除绑定
    /// </summary>
    public virtual void Destroy()
    {
        UnBind();
    }

    public void Dispatch(int areaCode, int eventCode, object message)
    {
        MsgCenter._Instance.DisPatch(areaCode, eventCode, message);
    }

    protected void SetPanelActive(bool active)
    {
        gameObject.SetActive(active);
    }
}
