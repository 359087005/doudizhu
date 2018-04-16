using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
/// <summary>
/// 客户端socket封装
/// </summary>
public class ClientPeer
{
    private Socket socket;

    private string ip;
    private int port;
    /// <summary>
    /// 构造连接对象
    /// </summary>
    /// <param name="ip"></param>
    /// <param name="port"></param>
    public ClientPeer(string ip, int port)
    {
        try
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.ip = ip;this.port = port;

            StartReceive();
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
            throw;
        }
    }

    public void Connect()
    {
        socket.Connect(ip, port);
        Debug.Log("连接服务器成功...");
    }
    #region 接收数据
    //接收的数据缓冲区
    private byte[] receiveBytes = new byte[1024];
    //一旦接收到数据  就存到缓存区
    private List<byte> dataCache = new List<byte>();

    bool isProgressReceive = false;

    public Queue<SocketMsg> socketMsgQueue = new Queue<SocketMsg>();
    /// <summary>
    /// 开始异步接收数据
    /// </summary>
    private void StartReceive()
    {
        if (socket == null && socket.Connected)
        {
            Debug.LogError("连接未成功...");
            return;
        }
        socket.BeginReceive(receiveBytes, 0, receiveBytes.Length, SocketFlags.None, ReceiveCallBack, socket);
    }
    /// <summary>
    /// 收到消息之后 的回调
    /// </summary>
    private void ReceiveCallBack(IAsyncResult ar)
    {
        try
        {
            int length = socket.EndReceive(ar);
            byte[] tmpByteArray = new byte[length];
            Buffer.BlockCopy(receiveBytes, 0, tmpByteArray, 0, length);

            //处理收到的消息
            dataCache.AddRange(tmpByteArray);
            if (isProgressReceive == false)
                ProcessReceive();
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    private void ProcessReceive()
    {
        isProgressReceive = true;

        byte[] data = EncoderTool.DeconderPacket(ref dataCache);
        if (data == null)
        {
            isProgressReceive = false;
            return;
        }
        //TODO 需要再次转成一个具体的类型供使用

        SocketMsg msg = EncoderTool.DeCodeMsg(data);

        //存储消息等待处理
        socketMsgQueue.Enqueue(msg);
        //尾递归
        ProcessReceive();

    }

    #endregion
    #region 发送数据
    public void Send(int opCode, int subCode, object value)
    {
        SocketMsg msg = new SocketMsg(opCode, subCode, value);
        byte[] data = EncoderTool.EncodeMsg(msg);
        byte[] packet = EncoderTool.EnconderPacket(data);

        try
        {
            socket.Send(packet);
        }
        catch (System.Exception e)
        {

            Debug.LogError(e.Message);
        }
    }

    #endregion
}
