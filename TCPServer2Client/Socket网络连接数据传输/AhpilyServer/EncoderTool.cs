using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace AhpilyServer
{

   public class EncoderTool
    {
        #region     粘包 拆包

        /// <summary>
        /// 构造消息体
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] EncoderPacket(byte[] data)
        {
            using (MemoryStream me = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(me))
                {
                    bw.Write(data.Length);
                    bw.Write(data);
                    byte[] byteArray = new byte[me.Length];

                    Buffer.BlockCopy(me.GetBuffer(),0,byteArray,0,(int)me.Length);
                    return byteArray;
                }
            }
        }

        /// <summary>
        /// 解析消息体
        /// </summary>
        /// <returns></returns>
        public static byte[] DecoderPacket(ref List<byte> datache)
        {
            if (datache.Count < 4) return null;
                

            using (MemoryStream ms = new MemoryStream(datache.ToArray()))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    int length = br.ReadInt32();
                    int remainLenth = (int)(ms.Length - ms.Position);
                    if (length > remainLenth)
                    {
                        return null;
                        throw new Exception("数据长度不够包头约定长度，无法构成完整消息");
                    }
                    //获得数据
                    byte[] data = br.ReadBytes(length);
                    //数据缓存更新
                    datache.Clear();
                    datache.AddRange(br.ReadBytes(remainLenth));
                    return data;
                }
            }
        }
        #endregion


        #region  构造需要发送的sokcetmsg类
        /// <summary>
        /// 吧sokcetMsg转换成字节数组
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static byte[] EncodeMsg(SocketMsg msg)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.Write(msg.OpCode);
                    bw.Write(msg.SubCode);
                    if (msg.Value != null)
                    {
                        byte[] valueBytes = EncoderObj(msg.Value);
                        bw.Write(valueBytes);
                    }

                    byte[] data = new byte[ms.Length];
                    Buffer.BlockCopy(ms.GetBuffer(),0,data,0,(int)ms.Length);
                    return data;
                }
            }
        }

        public static SocketMsg DecodeMsg(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    SocketMsg msg = new SocketMsg();
                    msg.OpCode = br.ReadInt32();
                    msg.SubCode = br.ReadInt32();

                    if (ms.Length > ms.Position)
                    {
                        byte[] valueBytes = br.ReadBytes((int)(ms.Length - ms.Position));
                        msg.Value = DecoderObj(valueBytes);
                    }
                    return msg;
                }
            }
        }

        #endregion


        #region 
        public static byte[] EncoderObj(object value)
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

        public static object DecoderObj(byte[] valueBytes)
        {
            using (MemoryStream ms = new MemoryStream(valueBytes))
            {
                BinaryFormatter bf = new BinaryFormatter();
                object value = bf.Deserialize(ms);
                return value;
            }
        }




        #endregion
    }
}
