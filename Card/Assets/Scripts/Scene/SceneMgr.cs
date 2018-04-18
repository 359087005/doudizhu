using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.SceneManagement;

public class SceneMgr : ManagerBase
{
    public static SceneMgr _Instance = null;
    public void Awake()
    {
        _Instance = this;
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;

        Add(SceneEvent.LOAD_SCENE, this);
    }


    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case SceneEvent.LOAD_SCENE:
                LoadSceneMsg msg = message as LoadSceneMsg;
                LoadScene(msg);
                break;
        }
    }

    private Action OnSceneLoad = null;
    /// <summary>
    /// 加载场景
    /// </summary>
    /// <param name="sceneBuildIndex"></param>
    private void LoadScene(LoadSceneMsg msg)
    {
        if (msg.sceneBuildIndex != -1)
            SceneManager.LoadScene(msg.sceneBuildIndex);
        if (msg.sceneBuildName != null)
            SceneManager.LoadScene(msg.sceneBuildName);
        if (msg.OnSceneLoad != null)
            OnSceneLoad = msg.OnSceneLoad;
    }
    /// <summary>
    /// 场景加载完毕执行
    /// </summary>
    /// <param name="arg0"></param>
    /// <param name="arg1"></param>
    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode argloadSceneMode)
    {
        if (OnSceneLoad != null)
        {
            OnSceneLoad();
        }
    }
}

