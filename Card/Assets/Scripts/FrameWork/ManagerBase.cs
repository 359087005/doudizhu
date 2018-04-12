using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 每个模块的基类 
///     保存自身注册的一系列消息
/// </summary>
public class ManagerBase : MonoBase
{
    /// <summary>
    /// 存储消息的事件码 和哪个脚本相互关联的字典
    ///     
    /// 
    ///     譬如角色哦，模块有个动作是移动
    ///                     移动脚本需要关心    角色移动
    ///                     动画脚本关心        播放移动动画
    ///                     音效脚本关心        播放移动音效
    /// </summary>
    private Dictionary<int, List<MonoBase>> dict = new Dictionary<int, List<MonoBase>>();


    //处理自身的消息
    public override void Execute(int eventCode, object message)
    {
        if (!dict.ContainsKey(eventCode))
        {
            Debug.LogWarning(eventCode + "事件没有注册");
            return;
        }
        //注册过这个消息 给所有脚本发送消息
        List<MonoBase> list = dict[eventCode];
        for (int i = 0; i < list.Count; i++)
        {
            list[i].Execute(eventCode,message);
        }
    }


    /// <summary>
    /// 添加事件
    /// </summary>
    public void Add(int eventCode, MonoBase mono)
    {
        //未注册 
        List<MonoBase> list = null;
        if (!dict.ContainsKey(eventCode))
        {
            list = new List<MonoBase>();
            list.Add(mono);
            dict.Add(eventCode,list);
            return;
        }
        //已注册
        list = dict[eventCode];
        list.Add(mono);
    }
    
    public void Add(int[] eventCodes, MonoBase mono)
    {
        for (int i = 0; i < eventCodes.Length; i++)
        {
            Add(eventCodes[i],mono);
        }
    }
    /// <summary>
    /// 移除事件
    /// </summary>
    public void Remove(int eventCode, MonoBase mono)
    {
        if (!dict.ContainsKey(eventCode))
        {
            Debug.LogWarning(eventCode + "事件未注册");
            return;
        }
        List<MonoBase> list = dict[eventCode];
        if (list.Count == 1)
        {
            dict.Remove(eventCode);
        }
        else
        {
            list.Remove(mono);
        }
    }
    public void Remove(int[] eventCodes, MonoBase mono)
    {
        for (int i = 0; i < eventCodes.Length; i++)
        {
            Remove(eventCodes[i],mono);
        }
    }
}
