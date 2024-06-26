using System;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{
    /// <summary>
    /// 基础计时器
    /// </summary>
    public class Timer
    {
        private Action Onfinished;
        private float finishTime;
        private float delayTime;
        private bool loop;
        private bool mIsFinish;
        public bool isFinish => mIsFinish;
        /// <summary>
        /// 开始倒计时
        /// </summary>
        public void Start(Action OnFinished, float _delayTime, bool isLoop)
        {
            Onfinished = OnFinished;
            finishTime = Time.time + _delayTime;
            delayTime = _delayTime;
            loop = isLoop;
            mIsFinish = false;
        }
        /// <summary>
        /// 提供给外部手动停止方法
        /// 
        /// </summary>
        public void Stop() => mIsFinish = true;
        /// <summary>
        /// 更新计时器
        /// </summary>
        public void Update()
        {
            if (mIsFinish) return;
            if (Time.time < finishTime) return;
            if (!loop) Stop();
            else finishTime = Time.time + delayTime;
            Onfinished?.Invoke();
        }
    }
    public interface ITimerSystem : ISystem
    {
        Timer AddTimer(float delayTime, Action onFinished, bool isLoop = false);
    }
    /// <summary>
    /// 计时器系统
    /// </summary>
    public class TimeSystem : AbstractSystem, ITimerSystem
    {
        private List<Timer> updateList = new List<Timer>();
        private Queue<Timer> availableQueue = new Queue<Timer>();
        /// <summary>
        /// 初始化
        /// </summary>
        protected override void OnInit()
        {
            PublicMono.Instance.OnUpdate += Update;
        }
        /// <summary>
        /// 添加计时器
        /// </summary>
        /// <param name="delayTime"></param>
        /// <param name="onFinished"></param>
        /// <param name="isLoop"></param>
        /// <returns></returns>
        public Timer AddTimer(float delayTime, Action onFinished, bool isLoop)
        {
            var timer = availableQueue.Count == 0 ? new Timer() : availableQueue.Dequeue();
            timer.Start(onFinished, delayTime, isLoop);
            updateList.Add(timer);
            return timer;
        }
        /// <summary>
        /// 更新计时器
        /// </summary>
        public void Update()
        {
            if (updateList.Count == 0) return;
            for (int i = updateList.Count-1; i >=0; i--)
            {
                if (updateList[i].isFinish)
                {
                    availableQueue.Enqueue(updateList[i]);
                    updateList.RemoveAt(i);
                    continue;
                }
                updateList[i].Update();
            }
        }
    }
}
