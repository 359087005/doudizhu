using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace AhpilyServer
{
    public static class EnconderTool
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
                using (BinaryWriter bw = new BinaryWriter(me))
                {
                    //写入长度
                    bw.Write(data.Length);
                    //写入数据
                    bw.Write(data);

                    byte[] byteArray = new byte[(int)data.Length];
                    //使用buffer的blockcopy方法 极快。
                    //1,源buffer  2,资源偏移量    3,目标    4,写入目标的偏移量  5,需要复制的长度
                    Buffer.BlockCopy(me.GetBuffer(), 0, byteArray, 0, (int)data.Length);
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
                throw new Exception("数据缓存长度不足4，不能构成一个完整的消息");
            //using ()   调用后自动关闭  不用调用close方法
            using (MemoryStream me = new MemoryStream(dataCache.ToArray())) //可以理解为byte数组
            {
                using (BinaryReader br = new BinaryReader(me))
                {
                    //这个长度即数据去掉包头总长度
                    int length = br.ReadInt32();//从当前流中读取4字节  并使流的当前位置提升4字节
                    int remainLength =(int)(me.Length - me.Position);
                    if (length > remainLength) //数据总长度 大于读取长度 即消息长度不足
                        throw new Exception("数据缓存长度不够约定长度，不能构成一个完整的消息");

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
    }
}
