using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace QFramework
{/// <summary>
/// 公共Mono脚本  全局访问   提供帧更新和协程等函数  减少开销 (需要挂载和查找)    。一个方案是再写一个单例模式的公共Mono管理器，实现全局访问
/// </summary>
    class PublicMono : MonoSingle<PublicMono>
    {
        ////全局访问 公开静态实例
        //public static PublicMono Instance;
        ////赋值
        //private void Awake()
        //{
        //    Instance = this;
        //    GameObject.DontDestroyOnLoad(gameObject);
        //}
        //定义了三个委托事件
        public event Action OnUpdate;
        public event Action OnLateUpdate;
        public event Action OnFixedUpdate;
        private void Update()
        {
            OnUpdate?. Invoke();
        }
        private void LateUpdate()
        {
            OnLateUpdate?.Invoke();
        }
        private void FixedUpdate()
        {
            OnFixedUpdate?.Invoke();
        }
    }
}
