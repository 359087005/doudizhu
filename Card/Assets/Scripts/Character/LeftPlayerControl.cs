using Protocol.Dto.Fight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftPlayerControl : PlayerControl
{
    private void Awake()
    {
        Bind(CharacterEvent.INIT_LEFT_CARD,
            CharacterEvent.ADD_LEFT_CARD,
            CharacterEvent.REMOVE_LEFT_CARD);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case CharacterEvent.INIT_LEFT_CARD:
                StartCoroutine(InitCardList());
                break;
            case CharacterEvent.ADD_LEFT_CARD:
                AddTableCard();
                break;
            case CharacterEvent.REMOVE_LEFT_CARD:
                RemoveCard((message as List<CardDto>).Count);
                break;
            default:
                break;
        }
    }

}
