using System;
using System.Collections;
using System.Collections.Generic;

namespace QFramework
{
    /// <summary>
    /// ��Դ�ع��� ��̬��Դ����
    /// </summary>
    public class ResPool<T> where T : UnityEngine.Object
    {
        // �ֵ�
        private Dictionary<string, T> resDic = new Dictionary<string, T>();//
        public void Get(string key, Action<T> callBack)//Action<T>�ص�  ��Ϊ�첽������Դ ʱ��Ӱ�쵱ǰ֡��һ�����ص���Դ  ����Դ�����ӳ�ʹ��
        {
            ///��Ϊout�ؼ��ֱ����뷽������һ��ʹ�ã�����ָʾ�ò�����һ�����������
            ///�����б��е�ÿ��out�����������ڷ�������֮ǰ����ֵ
            ///string Object  ������������  ���� ��out��ʶ�������ʹ���   ref out
            if (resDic.TryGetValue(key, out T data))
            {
                callBack(data);//���ûص�����
                return;
            }
            resDic.Add(key, null);
            ResHlper.AsyncLoad<T>(key, o =>
             {
                 callBack(o);
                 resDic[key] = o;
             });
        }
        ///������Դ
        public void Clear() =>resDic.Clear();

    }
}

