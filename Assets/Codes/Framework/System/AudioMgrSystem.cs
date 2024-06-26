using System;
using UnityEngine;

namespace QFramework
{
    public struct StopBgmEvent { public bool isStop; }
    // public struct PauseBgmEvent { }

    public interface IAudioMgrSystem : ISystem//定义接口  继承框架接口
    {
        void PlayBgm(string name);
        void StopBgm(bool isPause);

        void PlaySound(string name);
        AudioSource GetSound(string name);
        //void StopSound(AudioSource source);
        void RecoverySound(AudioSource source);
        void Clear();

        //BindableProperty<float> BgmVolume { get; }
        //BindableProperty<float> SoundVolume { get; }

    }
    /// <summary>
    /// 实现音频池 和  音效组件管理
    /// </summary>
    class AudioMgrSystem : AbstractSystem, IAudioMgrSystem
    {
        //public BindableProperty<float> BgmVolume { get; } = new BindableProperty<float>(1);//定义 float 属性  初始1

        //public BindableProperty<float> SoundVolume { get; } = new BindableProperty<float>(1);
        /// <summary>
        /// 背景音乐播放组件
        /// </summary>
        private AudioSource bgm;
        /// <summary>
        /// 临时组件 方便引用
        /// </summary>
        private AudioSource tempSource;
        /// <summary>
        /// 音量渐变工具
        /// </summary>
        private FadeNum fade;
        /// <summary>
        /// 资源加载系统
        /// </summary>
        private ResPool<AudioClip> clipPool;
        /// <summary>
        /// AudioSource 组件池
        /// </summary>
        private ComponentPool<AudioSource> sourcePool;
        ///游戏音频数据
        private IGameAudioModel audioModel;
        /// <summary>
        /// 初始化系统
        /// </summary>
        protected override void OnInit()
        {
            sourcePool = new ComponentPool<AudioSource>("GameSound");
            clipPool = new ResPool<AudioClip>();

            audioModel = this.GetModel<IGameAudioModel>();

            fade = new FadeNum();
            fade.SetMinMax(0, audioModel.BgmVolume.Value);

        
            //this.RegisterEvent<StopBgmEvent>(OnStopBgm);
            //this.RegisterEvent<PauseBgmEvent>(OnPauseBgm);

            audioModel.BgmVolume.Register(OnBgmVolumeChanged);
            audioModel.SoundVolume.Register(v =>
            sourcePool.SetAllEnablecomponent(source => source.volume = v));

            PublicMono.Instance.OnUpdate += UpdateVolume;

        }
        /// 初始化组件
        public void InitSource()
        {
            sourcePool.AutoPush(source => !source.isPlaying);
            sourcePool.Get(out tempSource);
            tempSource.volume = audioModel.SoundVolume.Value;
        }
        /// <summary>
        /// 更新音量
        /// </summary>
        private void Update()
        {
            if (!fade.IsEnabled) return;
            fade.Update(Time.deltaTime);
            bgm.volume = fade.CurrentValue;
        }
        /// <summary>
        /// 当背景音乐音量改变时
        /// </summary>
        /// <param name="v">音量</param>
        private void OnBgmVolumeChanged(float v)
        {
            fade.SetMinMax(0, v);

            if (bgm == null) return;

            bgm.volume = v;
        }
        ///// <summary>
        ///// 当音效音量改变时
        ///// </summary>
        ///// <param name="v">音量</param>
        //private void OnSoundVolumeChanged(float v)
        //{
        //    ///当有音效再播 调节所有在播音效音量
        //    sourcePool.SetAllEnableconponent(source => source.volume = v);
        //}
        /// <summary>
        /// 停止背景音乐
        /// </summary>
        /// <param name="e"></param>
        //private void OnStopBgm(StopBgmEvent e)
        //{
        //    if (bgm == null || !bgm.isPlaying) return;
        //    PublicMono.Instance.OnUpdate += Update;
        //    fade.SetState(FadeState.FadeOut, () =>
        //    {
        //        PublicMono.Instance.OnUpdate -= Update;
        //        if (e.isStop) bgm.Stop();
        //        else bgm.Pause();

        //    });
        //}
        ///// <summary>
        ///// 暂停背景音乐
        ///// </summary>
        ///// <param name="e"></param>
        //private void OnPauseBgm(PauseBgmEvent e)
        //{
        //    if (bgm == null || !bgm.isPlaying) return;
        //    PublicMono.Instance.OnUpdate += Update;
        //    fade.SetState(FadeState.FadeOut, () =>
        //    {
        //        PublicMono.Instance.OnUpdate -= Update;
        //        bgm.Pause();
        //    });
        //}
        /// <summary>
        /// 获取音效
        /// </summary>
        /// <param name="name"></param>
        /// <param name="callBack"></param>
        public void GetSound(string name, Action<AudioSource> callBack)
        {
            //先自动回收 音频源组件
            sourcePool.AutoPush(cp => !cp.isPlaying);
            //从组件池获取一个组件
            sourcePool.Get(out tempSource);
            //从资源池获取一个音效
            clipPool.Get("Audio/Sound/" + name, clip =>
            {
                tempSource.clip = clip;
                tempSource.loop = true;
                callBack(tempSource);//调用回调
            });
        }
        /// <summary>
        /// 播放BGM
        /// </summary>
        /// <param name="name">音乐资源名字</param>
        public void PlayBgm(string name)
        {
            if (bgm == null)
            {
                var o = new GameObject("GameBgm");
                GameObject.DontDestroyOnLoad(o);
                bgm = o.AddComponent<AudioSource>();
                bgm.loop = true;
                bgm.volume = 0;
            }
            clipPool.Get("Audio/Bgm/" + name, audioClip =>
            {//回调函数具体实现
                PublicMono.Instance.OnUpdate += Update;
                //如果没有东西播放
                if (!bgm.isPlaying)
                {
                    fade.SetState(FadeState.FadeIn, () =>
                    {
                        PublicMono.Instance.OnUpdate -= Update;
                    });
                    bgm.clip = audioClip;
                    bgm.Play();
                }
                else
                {
                    //如果有BGM正在播放 就先把音量降下来1-0，再播放当前音乐0-1
                    fade.SetState(FadeState.FadeOut, () =>
                    {
                        fade.SetState(FadeState.FadeIn, () =>
                        {
                            PublicMono.Instance.OnUpdate -= Update;
                        });
                        bgm.clip = audioClip;
                        bgm.Play();
                    });
                }
            });
        }
        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="name"></param>
        public void PlaySound(string name)
        {
            //先自动回收 音频源组件
            sourcePool.AutoPush(cp => !cp.isPlaying);
            //sourcePool.AutoPush((cp) => { return !cp.isPlaying; });
            //从组件池获取一个组件
            sourcePool.Get(out tempSource);
            //从资源池获取一个音效
            clipPool.Get("Audio/Sound/" + name, clip =>
            {
                tempSource.clip = clip;
                tempSource.loop = false;
                tempSource.Play();
            });
        }
        /// <summary>
        /// 用于停止循环播放
        /// </summary>
        /// <param name="source"></param>
        public void StopSound(AudioSource source)
        {
            sourcePool.Push(source, source.Stop);
        }
        ///更新音量
        private void UpdateVolume()
        {
            if (!fade.IsEnabled) return;
            bgm.volume = fade.Update(Time.deltaTime);
        }
        /// <summary>
        /// 停止Bgm
        /// </summary>
        public void StopBgm(bool isPause)
        {
            if (bgm == null || !bgm.isPlaying) return;

            fade.SetState(FadeState.FadeOut, () =>
            {
                if (isPause) bgm.Pause();
                else bgm.Stop();
            });
        }
        /// <summary>
        /// 获取音效
        /// </summary>
        public AudioSource GetSound(string name)
        {
            InitSource();
            clipPool.Get("Audio/Sound/" + name, clip =>
            {
                tempSource.clip = clip;
                tempSource.loop = true;
            });
            return tempSource;
        }

        public void RecoverySound(AudioSource source) => sourcePool.Push(source, source.Stop);
        public void Clear()
        {
            clipPool.Clear();
        }
    }
}
