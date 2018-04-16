using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class NetMananger : ManagerBase
{
    public static NetMananger instance = null;


    private ClientPeer client = new ClientPeer("127.0.0.1",8860);

    public void Connected()
    {
        client.Connect();
    }


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Connected();
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
            //TODO 操作这个MSG
        }

    }
}

