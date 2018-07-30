using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            if (datache.Count < 4)
                throw new Exception("数据长度不足4，无法构成数据");

            using (MemoryStream ms = new MemoryStream(datache.ToArray()))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    int length = br.ReadInt32();
                    int remainLenth = (int)(ms.Length - ms.Position);
                    if (length > remainLenth)
                    {
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

    }
}
