using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

namespace PlatformShoot
{
    public class Bullet : PlatformShootGameController
    {
        /// /// <summary>
        /// 使用框架QF 所以注释掉 因为不良引用
        /// </summary>
        private LayerMask layerMask;
        // private GameObject gamePass;
        private int bullteDir;

        //private IObjectPoolSystem poolSystem;
        //private IAudioMgrSystem audioMgr;
        //private ITimerSystem timerSystem;
        private Timer timer;

        private void Awake()
        {
            ///用的不频繁  就不用通过缓存来找
            //poolSystem = this.GetSystem<IObjectPoolSystem>();
            // audioMgr = this.GetSystem<IAudioMgrSystem>();
            //timerSystem = this.GetSystem<ITimerSystem>();

            layerMask = LayerMask.GetMask("Ground", "Keys");//开启特定层检测
        }
        //void Start()
        //{
        //    GameObject.Destroy(this.gameObject, 3f);//本物体3S后销毁
        //}

        // Update is called once per frame
        void Update()
        {
            //方向飞行   切记 只适用于非物理运动
            //transform.Translate(Vector3.right*Time.deltaTime,Space.Self);//方向向量和速度，坐标方式，
            //transform.Translate(new Vector3(1, 1, 1) * Time.deltaTime, Space.Self);
            transform.Translate(bullteDir * 12 * Time.deltaTime, 0, 0);//x轴方向移动

        }
        private void FixedUpdate()
        {
            var coll = Physics2D.OverlapBox(transform.position, transform.localScale, 0, layerMask);
            if (coll)
            {
                if (coll.gameObject.CompareTag("Keys"))//标签
                {
                    GameObject.Destroy(coll.gameObject);
                    this.SendCommand<ShowPassDoorCommand>();
                    this.GetSystem<IAudioMgrSystem>().PlaySound("开关");
                    //gamePass.SetActive(true);//激活
                }
                else
                {
                    this.GetSystem<IAudioMgrSystem>().PlaySound("地面");
                }
                this.GetSystem<IObjectPoolSystem>().Recovery(gameObject);
            }
        }
        //public void GetGamePass(GameObject _gamePass)//
        //{
        //    gamePass = _gamePass;
        //    Debug.Log(gamePass.name);
        //}

        //初始化
        public void InitDir(int dir)
        {
            bullteDir = dir;
        }

        public void OnEnable()
        {
            timer = this.GetSystem<ITimerSystem>().AddTimer(3, () => 
            {
                this.GetSystem<IObjectPoolSystem>().Recovery(gameObject);
            });
        }
        public void OnDisable()
        {
            timer.Stop();
        }
     
    }
}


