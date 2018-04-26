using Assets.Scripts.GameDataModel;
using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightStatePanel : StatePanel
{
    protected override void Awake()
    {
        base.Awake();
        Bind(UIEvent.SET_RIGHT_PLAYER_DATA);
    }
    public override void Execute(int eventCode, object message)
    {
        base.Execute(eventCode, message);
        switch (eventCode)
        {
            case UIEvent.SET_RIGHT_PLAYER_DATA:
                this.dto = message as UserDto;
                break;
        }
    }

    protected override void Start()
    {
        base.Start();
        MatchRoomDto matchRoomDto = Model.gameModel.matchRoomDto;
        if (matchRoomDto.rightId != -1)
        {
            this.dto = matchRoomDto.uIdUserDtoDict[matchRoomDto.rightId];
            if (matchRoomDto.readyUIdList.Contains(matchRoomDto.rightId))
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

