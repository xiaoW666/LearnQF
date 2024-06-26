using System;
using System.Collections;
using System.Collections.Generic;

namespace QFramework
{
    /// <summary>
    /// 资源池工具 静态资源加载
    /// </summary>
    public class ResPool<T> where T : UnityEngine.Object
    {
        // 字典
        private Dictionary<string, T> resDic = new Dictionary<string, T>();//
        public void Get(string key, Action<T> callBack)//Action<T>回调  因为异步加载资源 时间影响当前帧不一定加载到资源  对资源进行延迟使用
        {
            ///因为out关键字必须与方法参数一起使用，用于指示该参数是一个输出参数。
            ///参数列表中的每个out参数都必须在方法调用之前被赋值
            ///string Object  都是引用类型  所以 加out标识引用类型传递   ref out
            if (resDic.TryGetValue(key, out T data))
            {
                callBack(data);//调用回调函数
                return;
            }
            resDic.Add(key, null);
            ResHlper.AsyncLoad<T>(key, o =>
             {
                 callBack(o);
                 resDic[key] = o;
             });
        }
        ///清理资源
        public void Clear() =>resDic.Clear();

    }
}

