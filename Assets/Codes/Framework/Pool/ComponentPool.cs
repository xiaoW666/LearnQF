using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace QFramework
{/// <summary>
/// 组件池
/// </summary>
/// <typeparam name="T"> 组件类型</typeparam>
    class ComponentPool<T> where T : Behaviour
    {
        /// <summary>
        /// 组件根对象
        /// </summary>
        private GameObject root;
        /// <summary>
        /// 指定组件挂载对象名字
        /// </summary>
        private String rootName;
        /// <summary>
        /// 存储所以已激活组件
        /// </summary>
        private List<T> openList = new List<T>();
        /// <summary>
        /// 储存所有未激活组件  使用队列 不在乎顺序
        /// </summary>
        private Queue<T> closeList = new Queue<T>();
        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="rootObjName"> 组件挂载名字</param>
        public ComponentPool(string rootObjName)
        {
            rootName = rootObjName;
        }
        public void Clear()
        {
            openList.Clear();
           closeList.Clear();
            GameObject.Destroy(root);
            root = null;
        }
        /// <summary>
        /// 设置所有已激活组件相同参数
        /// </summary>
        /// <param name="callBack">回调函数</param>
        public void SetAllEnablecomponent(Action<T> callBack)
        {
            foreach (T component in openList) callBack(component);
           
        }
        /// <summary>
        /// 获取一个可使用组件
        /// </summary>
        /// <param name="component"></param>
        public  void Get(out T component)
        {
            //关闭列表所有东西
            if (closeList.Count>0)
            {
                component = closeList.Dequeue();//获取一个为使用组件
                component.enabled = true;//激活组件
            }
            else //关闭列表里面没有东西 第一次使用
            {
                if (root==null)
                {
                    root = new GameObject(rootName);
                    GameObject.DontDestroyOnLoad(root);
                }
                component = root.AddComponent<T>();
            }
            //把激活的组件放到开启列表
            openList.Add(component);
        }
        //自动回收组件
        //Unity引擎为我们提供的两种封装好的类型Action和Func。
        //Func类 跟Action一样，只是该类必须有返回值反之
       
        public void AutoPush(Func<T,bool> condition)//bool 是返回类型
        {
            ///可能开启列表有东西回收 逆向遍历 满足条件就回收
            for (int i = openList.Count-1; i >=0; i--)
            {
                //为true就回收
                if (condition(openList[i]))
                {
                    openList[i].enabled = false;
                    closeList.Enqueue(openList[i]);
                    openList.RemoveAt(i);
                }
            }
        }


         /// <summary>
        /// 回收单个组件
        /// </summary>
        public void Push(T component,Action callBack=null ) 
        {
            if (openList.Contains(component))
            {
                callBack?.Invoke();
                component.enabled = false;
                openList.Remove(component);
                closeList.Enqueue(component);
            }
        }
    }
}
