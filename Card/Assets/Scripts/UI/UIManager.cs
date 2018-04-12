using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : ManagerBase {

    public static UIManager _Instance = null;
    public void Awake()
    {
        _Instance = this;
    }
}
