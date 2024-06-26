using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace QFramework
{/// <summary>
/// Resources加载工具封装  以便简化使用
/// </summary>
    class ResHlper
    {
        //同步加载
        public static T SyncLoda<T>(String name)where T : UnityEngine.Object
        {
            T res = Resources.Load<T>(name);
            return res is GameObject ? GameObject.Instantiate(res) : res;//三元运算 
        }
        //异步加载 写协程
        private static IEnumerator AsyncLoadRes<T>(String name,Action<T> callBack) where T:UnityEngine.Object
        {
            var r = Resources.LoadAsync<T>(name);
            while (!r.isDone) yield return null;
            callBack(r.asset is GameObject ?GameObject.Instantiate(r.asset) as T:r.asset as T);//回调函数抛给外部处理
        }
        //提供给外部一个使用的方法 提供协程功能   通过mono工具设置协程开启函数  外部调用就行
        public static void AsyncLoad<T>(String name,Action<T> callBack) where T:UnityEngine.Object
        {
            PublicMono.Instance.StartCoroutine(AsyncLoadRes(name, callBack));
        }
        public static void Clear()
        {
            //卸载未占用的asset资源
            Resources.UnloadUnusedAssets();
            //回收内存
            GC.Collect();
        }
    }
}
