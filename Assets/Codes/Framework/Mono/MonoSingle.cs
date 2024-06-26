using UnityEngine;
namespace QFramework
{
    /// <summary>
    /// 抽象泛型类
    /// </summary>
    public abstract class MonoSingle<T> : MonoBehaviour where T : MonoBehaviour
    {
        ///懒加载方式  使用时候加载
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    var gameObject = new GameObject(typeof(T).Name);//获取当前的类型的类名作为对象名
                    instance = gameObject.AddComponent<T>();//挂载类
                    GameObject.DontDestroyOnLoad(gameObject);
                }
                return instance;
            }
        }

    }
}
