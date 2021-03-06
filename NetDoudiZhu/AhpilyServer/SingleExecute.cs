﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace AhpilyServer
{
    /// <summary>
    /// 一个需要执行的方法
    /// </summary>
    public delegate void ExecuteDelegate();
    /// <summary>
    /// 单线程池
    /// </summary>
    public class SingleExecute
    {
        private static SingleExecute instance = null;
        public static SingleExecute Instance
        {
            get
            {
                lock (o)
                {
                    if (instance == null)
                        instance = new SingleExecute();
                    return instance;
                }
            }
        }

        private static string o = "aaa";
        /// <summary>
        /// 互斥锁
        /// </summary>
        Mutex mutex;

        private SingleExecute()
        {
            mutex = new Mutex();
        }

        /// <summary>
        /// 单线程处理逻辑
        /// </summary>
        /// <param name="executeDelegate"></param>
        public void Execute(ExecuteDelegate executeDelegate)
        {
            lock (this)
            {
                mutex.WaitOne();
                executeDelegate();
                mutex.ReleaseMutex();
            }
        }
    }
}
