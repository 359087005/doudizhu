using System;
using System.Collections.Generic;


class LoadSceneMsg
{
    public int sceneBuildIndex;
    public string sceneBuildName;
    public Action OnSceneLoad;

    public LoadSceneMsg()
    {
        sceneBuildIndex = -1;
        sceneBuildName = null;
        OnSceneLoad = null;
    }

    public LoadSceneMsg(int sceneBuildIndex,  Action onSceneLoad)
    {
        this.sceneBuildIndex = sceneBuildIndex;
        this.sceneBuildName = null;
        this.OnSceneLoad = onSceneLoad;
    }
    public LoadSceneMsg(string sceneBuildName, Action onSceneLoad)
    {
        this.sceneBuildIndex = -1;
        this.sceneBuildName = sceneBuildName;
        this.OnSceneLoad = onSceneLoad;
    }
}

