using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : ManagerBase
{
    public static CharacterManager _Instance = null;

    private void Awake()
    {
        _Instance = this;
    }
}
