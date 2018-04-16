using System;
using System.Collections.Generic;
using System.Text;

namespace AhpilyServer.Timer
{
    /// <summary>
    /// 定时器到达时间后触发
    /// </summary>
    public delegate void TimeDelegate();
    /// <summary>
    /// 定时器任务的模型
    /// </summary>
    public class TimerModel
    {
        public int id;
        /// <summary>
        /// 定时器任务的时间
        /// </summary>
        public long time;

        private TimeDelegate timeDelegate;

        public TimerModel(int id, long time, TimeDelegate timeDelegate)
        {
            this.id = id;
            this.time = time;
            this.timeDelegate = timeDelegate;
        }

        public void Run()
        {
            timeDelegate();
        }
    }
}
