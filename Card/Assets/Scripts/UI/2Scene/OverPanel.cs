using Assets.Scripts.GameDataModel;
using Protocol;
using Protocol.Content;
using Protocol.Dto.Fight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 游戏结束面板
/// </summary>
public class OverPanel : UIBase
{
    private void Awake()
    {
        Bind(UIEvent.SHOW_OVER_PANEL);
    }
    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.SHOW_OVER_PANEL:
                RefreshShow(message as OverDto);
                break;
            default:
                break;
        }
    }

    private Text textWinIdrentity;
    private Text textWinBeen;
    private Button btnBack;
    void Start()
    {
        textWinIdrentity = transform.Find("textWinIdrentity").GetComponent<Text>();
        textWinBeen = transform.Find("textWinBeen").GetComponent<Text>();
        btnBack = transform.Find("BtnBack").GetComponent<Button>();


        btnBack.onClick.AddListener(BackClick);
        SetPanelActive(false);
    }
    /// <summary>
    /// 返回点击事件
    /// </summary>
    private void BackClick()
    {
        LoadSceneMsg msg = new LoadSceneMsg(1,
    delegate ()
    {
        //向服务器获取信息
        SocketMsg socketMsg = new SocketMsg(OpCode.USER, UserCode.GET_INFO_CREQ, null);
        Dispatch(AreaCode.NET, 0, socketMsg);
    });
        Dispatch(AreaCode.SCENE, SceneEvent.LOAD_SCENE, msg);
    }

    /// <summary>
    /// 刷新显示
    /// </summary>
    public void RefreshShow(OverDto dto)
    {
        SetPanelActive(true);
        //显示谁胜利
        textWinIdrentity.text = Identity.GetString(dto.winIdentity);
        //判断自己是否胜利
        if (dto.winUidList.Contains(Model.gameModel.UserDto.id))
        {
            textWinIdrentity.text += "胜利";
            textWinBeen.text = "欢乐豆 + ";
        }
        else
        {
            textWinIdrentity.text += "失败";
            textWinBeen.text = "欢乐豆 - ";
        }

        //欢乐豆
        textWinBeen.text += dto.beenCount;
    }
}
