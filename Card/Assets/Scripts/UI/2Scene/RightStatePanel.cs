using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightStatePanel : StatePanel 
{
    private void Awake()
    {
        Bind(UIEvent.SET_LEFT_PLAYER_DATA, UIEvent.PLAYER_READY);
    }
    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.SET_LEFT_PLAYER_DATA:
                this.dto = message as UserDto;
                break;
            case UIEvent.PLAYER_READY:
                {
                    int userId = (int)message;
                    if (userId == dto.id)
                        textReady.gameObject.SetActive(true);
                    break;
                }
            case UIEvent.PLAYER_HIDE_STATE:
                textReady.gameObject.SetActive(false);
                break;
            case UIEvent.PLAYER_LEAVE:
                {
                    int userId = (int)message;
                    if (userId == dto.id)
                        SetPanelActive(false);
                    break;
                }
            case UIEvent.PLAYER_ENTER:
                {
                    int userId = (int)message;
                    if (userId == dto.id)
                        SetPanelActive(true);
                    break;
                }
            default:
                break;
        }
    }
}
