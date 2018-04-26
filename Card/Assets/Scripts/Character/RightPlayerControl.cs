using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightPlayerControl : PlayerControl 
{
    private void Awake()
    {
        Bind(CharacterEvent.INIT_RIGHT_CARD);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case CharacterEvent.INIT_RIGHT_CARD:
                StartCoroutine(InitCardList());
                break;
        }
    }
}
