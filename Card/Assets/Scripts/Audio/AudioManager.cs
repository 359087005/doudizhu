using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : ManagerBase {

    public static AudioManager _Instance = null;

    private void Awake()
    {
        _Instance = this;
    }
}
