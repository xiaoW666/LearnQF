using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PlatformShoot
{/// <summary>
/// 创建一个空对象挂载所有音频组件 使用一个脚本管理
/// </summary>
    class AudioPlay : MonoBehaviour
    {//声明一个音频正在播放列表
        private List<AudioSource> pPlayingList;
        //对外提供一个静态实例 用于调用
        public static AudioPlay Instance;
        //载Awake中对实例赋值
        private void Awake()
        {
            Instance = this;
        }
        //start中初始化列表
        private void Start()
        {
            pPlayingList = new List<AudioSource>();
            //考虑场景转换 对当前对象进行保护
            GameObject.DontDestroyOnLoad(gameObject);

        }
        private void Update()
        {
            //遍历列表
            for (int i = pPlayingList.Count-1; i >= 0; i--)
            {
                var source = pPlayingList[i];
                if (!source.isPlaying)
                {
                    pPlayingList.RemoveAt(i);
                    Destroy(source);//销毁或者移除组件
                }
            }
        }
        //对外提供一个音效播放方法
        public void PlayeSound(String name)
        {
            //给当前与对像挂载音频组件
            var source = gameObject.AddComponent<AudioSource>();
            //通过Resources加载对应音频资源切片给音频组件
            source.clip = Resources.Load<AudioClip>("Audio/Sound/"+name);
            //播放
            source.Play();
            //把播放的音频加入正在播放列表
            pPlayingList.Add(source);
        }
    }
}
