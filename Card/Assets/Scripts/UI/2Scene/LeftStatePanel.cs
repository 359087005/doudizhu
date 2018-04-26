using Assets.Scripts.GameDataModel;
using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftStatePanel : StatePanel
{
    protected override void Awake()
    {
        base.Awake();
        Bind(UIEvent.SET_LEFT_PLAYER_DATA);
    }
    public override void Execute(int eventCode, object message)
    {
        base.Execute(eventCode,message);
        switch (eventCode)
        {
            case UIEvent.SET_LEFT_PLAYER_DATA:
                this.dto = message as UserDto;
                break;
        }
    }

    protected override void Start()
    {
        base.Start();

        MatchRoomDto matchRoomDto = Model.gameModel.matchRoomDto;
        if (matchRoomDto.leftId != -1)
        {
            this.dto = matchRoomDto.uIdUserDtoDict[matchRoomDto.leftId];
            if (matchRoomDto.readyUIdList.Contains(matchRoomDto.leftId))
            {
                ReadyState();
            }
        }
        else
        {
            SetPanelActive(false);
        }
        


    }
}
