using System;

namespace QFramework
{
    public enum FadeState
    {
        Close,
        FadeIn,//深入
        FadeOut//淡出
    }
    /// <summary>
    /// 数字渐变动画  或者透明度 等等 0-1
    /// </summary>
    class FadeNum
    {   /// <summary>
        /// 淡入状态的先决条件
        /// </summary>
        private FadeState fadeState = FadeState.Close;
        /// <summary>
        /// 是否开启
        /// </summary>
        public bool IsEnabled => fadeState != FadeState.Close;
        /// <summary>
        /// 调用委托的时候避免提前结束  
        /// </summary>
        private bool init = false;
        /// <summary>
        /// 淡入结束后要做的事
        /// </summary>
        private Action onEnevt;
        /// <summary>
        /// 当前值
        /// </summary>
        private float currentValue;
        public float CurrentValue => currentValue;
        /// <summary>
        /// 最大最小范围
        /// </summary>
        private float min = 0, max = 1;
        /// <summary>
        /// 设置范围
        /// </summary>
        /// <param name="_min"></param>
        /// <param name="_max"></param>
        public void SetMinMax(float _min, float _max)
        {
            min = _min;
            max = _max;
        }
        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="sate"></param>
        /// <param name="callBack"></param>
        public void SetState(FadeState state, Action callBack = null)
        {
            onEnevt = callBack;
            fadeState = state;
            init = false;
        }
        // 当fade完成
        private void OnFinish(float value)
        {
            onEnevt?.Invoke();
            currentValue = value;
            if (!init) return;
            fadeState = FadeState.Close;
        }
        /// <summary>
        /// 持续检测
        /// </summary>
        /// <param name="step"></param>
        public float Update(float step)
        {
            switch (fadeState)
            {
                ///如果是渐入状态 0-1  
                case FadeState.FadeIn:
                    //确认初始化参数
                    if (!init)
                    {
                        currentValue = min;
                        init = true;
                    }
                    if (currentValue<max)
                    {
                        currentValue += step;
                    }
                    else OnFinish(max);
                    break;
                    ///渐出 1-0
                case FadeState.FadeOut:
                    if (!init)
                    {
                        currentValue = max;
                        init = true;
                    }
                    if (currentValue > min)
                    {
                        currentValue -= step;
                    }
                    else OnFinish(min);


                    break;
            }
            return currentValue;
        }
    }
}
