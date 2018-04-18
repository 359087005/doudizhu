using Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/// <summary>
/// 网络模块
/// </summary>
public class NetMananger : ManagerBase
{
    public static NetMananger instance = null;

    private ClientPeer client = new ClientPeer("127.0.0.1", 8860);

    private void Start()
    {
        client.Connect();
    }

    private void Update()
    {
        if (client == null)
        {
            return;
        }
        while (client.socketMsgQueue.Count > 0)
        {
            SocketMsg msg = client.socketMsgQueue.Dequeue();
            // 操作这个MSG
            ReceiveSocketMsg(msg);
        }

    }
    #region 处理客户端内部 给服务器发消息的事件
    private void Awake()
    {
        instance = this;

        Add(0, this);
    }
    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case 0:
                client.Send(message as SocketMsg);
                break;
        }
    }
    #endregion

    #region 处理收到的服务器发来的消息

    HandlerBase accountHandler = new AccountHandler();
    /// <summary>
    /// 接收网络发来的消息
    /// </summary>
    /// <param name="msg"></param>
    private void ReceiveSocketMsg(SocketMsg msg)
    {
        UnityEngine.Debug.Log("NetManager_SubCode: " + msg.subCode);
        switch (msg.opCode)
        {
            case OpCode.ACCOUNT:
                accountHandler.OnReceive(msg.subCode,msg.value);
                break;
        }
    }


    #endregion
}

