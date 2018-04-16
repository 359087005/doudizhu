using AhpilyServer.ConCurrent;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace AhpilyServer.Timer
{
    /// <summary>
    /// 定时任务 管理类
    /// </summary>
    public class TimeManager
    {
        private static TimeManager instance = null;

        public static TimeManager Instance
        {
            get
            {
                lock (instance)
                {
                    if (instance == null)
                        instance = new TimeManager();
                    return instance;
                }
            }
        }

        private System.Timers.Timer timer;

        /// <summary>
        /// 任务ID 和模型 的映射
        /// </summary>
        private ConcurrentDictionary<int, TimerModel> idModelDic = new ConcurrentDictionary<int, TimerModel>();

        /// <summary>
        /// 要移除的任务ID
        /// </summary>
        private List<int> removeID = new List<int>();


        private ConCurrentInt id = new ConCurrentInt(-1);

        public TimeManager()
        {
            timer = new System.Timers.Timer(10);
            timer.Elapsed += Timer_Elapsed;
        }
        /// <summary>
        /// 达到事件间隔触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (removeID)
            {
                TimerModel timeModel = null;
                foreach (var id in removeID)
                {
                    idModelDic.TryRemove(id, out timeModel);
                }
                removeID.Clear();
            }

            foreach (var model in idModelDic.Values)
            {

                if (model.time <= DateTime.Now.Ticks)
                    model.Run();
            }

        }

        /// <summary>
        /// 添加定时任务  指定触发时间
        /// </summary>
        public void AddTimerEvent(DateTime datetime, TimeDelegate timeDelegate)
        {
            long delayTime = datetime.Ticks - DateTime.Now.Ticks;
            if (delayTime <= 0) return;
            AddTimerEvent(delayTime, timeDelegate);
        }
        /// <summary>
        /// 添加定时任务  延迟时间
        /// </summary>
        /// <param name="datetime">毫秒！！！！</param>
        /// <param name="timeDelegate"></param>
        public void AddTimerEvent(long datetime, TimeDelegate timeDelegate)
        {
            TimerModel model = new TimerModel(id.Add_Get(), DateTime.Now.Ticks + datetime, timeDelegate);

            idModelDic.TryAdd(model.id, model);
        }
    }
}
