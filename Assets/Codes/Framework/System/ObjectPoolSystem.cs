using System;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{
    public interface IObjectPoolSystem : ISystem
    {
        GameObject Get(string name);
        void Get(string name, Action<GameObject> callBack = null);
        void Recovery(GameObject obj);
        void Dispose();
    }
    /// <summary>
    /// 对象池管理系统
    /// </summary>
    public class ObjectPoolSystem : AbstractSystem, IObjectPoolSystem
    {
        /// <summary>
        /// 缓存池字典容器
        /// </summary>
        private Dictionary<string, PoolData> poolDic;
        /// <summary>
        /// 缓存池根对象
        /// </summary>
        private Transform poolRoot;

        protected override void OnInit()
        {
            poolDic = new Dictionary<string, PoolData>();
        }
        /// <summary>
        /// 清空缓冲池  场景转换
        /// </summary>
        void IObjectPoolSystem.Dispose()
        {
            poolDic.Clear();
            poolRoot = null;
        }

        GameObject IObjectPoolSystem.Get(string name)
        {
            return poolDic.TryGetValue(name, out PoolData data) && data.CanGet ? data.Get() : new GameObject(name);
        }

        void IObjectPoolSystem.Get(string name, Action<GameObject> callBack)
        {
            //如果有对应的格子 如果格子有东西 就取出来
            if (poolDic.TryGetValue(name, out PoolData data) && data.CanGet)
            {
                if (callBack == null) { data.Get(); }
                else { callBack(data.Get()); }
                return;
            }
            //异步加载资源 创建对象给外部调用 如果回调对象不为空 则抛弃该对象
            ResHlper.AsyncLoad<GameObject>(name, o =>
            {
                o.name = name;
                callBack?.Invoke(o);
            });
        }
        /// <summary>
        /// 把加载资源放入缓冲池
        /// </summary>
        /// <param name="obj"></param>
        void IObjectPoolSystem.Recovery(GameObject obj)
        {
            if (poolDic.TryGetValue(obj.name, out var data))
            {
                data.Push(obj);
                return;
            }
            //判断是否有根对象 没有就创建一个
            if (poolRoot == null) poolRoot = new GameObject("PoolRoot").transform;

            poolDic.Add(obj.name, new PoolData(obj, poolRoot));
        }
    }
    /// <summary>
    /// unity 游戏对象缓冲池
    /// </summary>
    public class PoolData
    {
        /// <summary>
        /// 可激活对象的队列
        /// </summary>
        private Queue<GameObject> activatableobject = new Queue<GameObject>();
        /// <summary>
        /// 可获取对象标识
        /// </summary>
        public bool CanGet => activatableobject.Count > 0;
        /// <summary>
        /// 对象挂载的父节点
        /// </summary>
        private Transform fatherObj;
        /// <summary>
        /// 构造函数
        /// </summary>
        public PoolData(GameObject obj, Transform root)
        {
            fatherObj = new GameObject(obj.name).transform;
            fatherObj.SetParent(root.transform);//给父对象设置根节点
            Push(obj);
        }
        /// <summary>
        /// 从可使用对象出列一个
        /// </summary>
        /// <returns></returns>
        public GameObject Get()
        {
            GameObject obj = activatableobject.Dequeue();
            obj.SetActive(true);
            obj.transform.SetParent(null);
            return obj;
        }
        /// <summary>
        /// 失活处理 设置父级为fathear对象 然后就将对象加入到可使用列表
        /// </summary>
        /// <param name="obj"></param>
        public void Push(GameObject obj)
        {
            obj.SetActive(false);
            obj.transform.SetParent(fatherObj.transform);
            activatableobject.Enqueue(obj);
        }
    }
}
