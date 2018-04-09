using System;
using System.Collections.Generic;
using System.Text;

namespace AhpilyServer.ConCurrent
{
    /// <summary>
    /// 线程安全的int类型  lock只能锁object类型  不能锁int基本类型
    /// </summary>
    public class ConCurrentInt
    {
        private int value;

        public ConCurrentInt(int value)
        {
            this.value = value;
        }

        public int Add_Get()
        {
            lock (this)
            {
                value++;
                return value;
            }
        }
        public int Reduce_Get()
        {
            lock (this)
            {
                value--;
                return value;
            }
        }

        public int Get()
        {
            return value;
        }
    }
}
