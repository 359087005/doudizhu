using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftPlayerControl : PlayerControl
{
    private void Awake()
    {
        Bind(CharacterEvent.INIT_LEFT_CARD);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case CharacterEvent.INIT_LEFT_CARD:
                StartCoroutine(InitCardList());
                break;
            default:
                break;
        }
    }

}
