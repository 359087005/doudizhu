using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


    public static class EncoderTool
    {
        #region 粘包拆包问题 封装一个有规定的数据包

        /// <summary>
        /// 构造数据包   包头+包尾
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] EnconderPacket(byte[] data)
        {
            //using ()   调用后自动关闭  不用调用close方法
            using (MemoryStream me = new MemoryStream()) //可以理解为byte数组
            {
                using (BinaryWriter bw = new BinaryWriter(me)) //二进制写入对象
                {
                    //写入长度
                    bw.Write(data.Length);
                    //写入数据
                    bw.Write(data);

                    byte[] byteArray = new byte[(int)me.Length];
                    //使用buffer的blockcopy方法 极快。
                    //1,源buffer  2,资源偏移量    3,目标    4,写入目标的偏移量  5,需要复制的长度
                    Buffer.BlockCopy(me.GetBuffer(), 0, byteArray, 0, (int)me.Length);
                    return byteArray;
                }
            }
        }

        /// <summary>
        /// 解析消息体 从缓存里取出一个完整的包
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] DeconderPacket(ref List<byte> dataCache) //ref 在方法里修改值  方法外也受影响
        {
            if (dataCache.Count < 4)
                return null;
                                                                    //throw new Exception("数据缓存长度不足4，不能构成一个完整的消息");
            //using ()   调用后自动关闭  不用调用close方法
            using (MemoryStream me = new MemoryStream(dataCache.ToArray())) //可以理解为byte数组
            {
                using (BinaryReader br = new BinaryReader(me))
                {
                    //这个长度即数据去掉包头总长度
                    int length = br.ReadInt32();//从当前流中读取4字节  并使流的当前位置提升4字节
                    int remainLength =(int)(me.Length - me.Position);
                    if (length > remainLength) //数据总长度 大于读取长度 即消息长度不足
                                                                // throw new Exception("数据缓存长度不够约定长度，不能构成一个完整的消息");
                        return null;
                    //终于可以开始读取数据了~~~~~~~~~~~~~~~~~~~
                    byte[] data = br.ReadBytes(length);
                    //保留缓存区多余的数据（有可能是下一个消息的数据） 更新缓冲区
                    dataCache.Clear();
                    dataCache.AddRange(br.ReadBytes(remainLength));
                    return data;
                }
            }
        }
        #endregion

        #region 构造发送的socketMsg的类
        /// <summary>
        /// 吧socketMsg类转换成 字节数组 发送
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static byte[] EncodeMsg(SocketMsg msg)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            bw.Write(msg.opCode);
            bw.Write(msg.subCode);
            if (msg.value != null)
            {
                byte[] valueBytes = EncodeObj(msg.value);
                bw.Write(valueBytes);
            }
            byte[] data = new byte[ms.Length];
            Buffer.BlockCopy(ms.GetBuffer(),0,data,0,(int)ms.Length);
            bw.Close();ms.Close();
            return data;
        }

        /// <summary>
        /// 吧收到的字节数据转换成socketMsg对象 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static SocketMsg DeCodeMsg(byte[] data)
        {
            MemoryStream ms = new MemoryStream(data);
            BinaryReader br = new BinaryReader(ms);

            SocketMsg msg = new SocketMsg();
            msg.opCode = br.ReadInt32();
            msg.subCode = br.ReadInt32();
            //还有剩余的字节  表示value 有值
            if (ms.Length > ms.Position)
            {
                byte[] valueBytes = br.ReadBytes((int)(ms.Length - ms.Position));
                object value = DecodeObj(valueBytes);
                msg.value = value;
            }
            br.Close();ms.Close();
            return msg;
        }
        #endregion

        #region 吧一个object类型转换成byte[]
        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] EncodeObj(object value)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms,value);
                byte[] valueBytes = new byte[ms.Length];
                Buffer.BlockCopy(ms.GetBuffer(),0,valueBytes,0,(int)ms.Length);
                return valueBytes;
            }
        }
        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <param name="valueBytes"></param>
        /// <returns></returns>
        public static object DecodeObj(byte[] valueBytes)
        {
            using (MemoryStream ms = new MemoryStream(valueBytes))
            {
                BinaryFormatter bf = new BinaryFormatter();
                object value =  bf.Deserialize(ms);
                return value;
            }
        }

        #endregion
    }

