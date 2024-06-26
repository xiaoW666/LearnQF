using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using QFramework;

namespace PlatformShoot
{
    /// <summary>
    /// 角色控制-移动
    /// </summary>
    /// /// <summary>
    /// 使用框架QF 所以注释掉 因为不良引用
    /// </summary>
    public class Player : PlatformShootGameController, ICamTarget
    {
        private Rigidbody2D rig;
        private BoxCollider2D box;
        private LayerMask layerMask;
        //private CustomWeapon mWeapon;
        private PlayerInputHandle inputHandle;

        private float pAccdelta, pDecDelta;//加速增量和减速增量 平滑移动
        private float pGroundSpeed;//地面速度
        [Header("跳跃力")]
        [SerializeField]
        private float pJumpSForce;//跳跃力
        private int faceDir = 1;//朝向

        private int inputX;
        private int inputY;
        //private ScoreMain scoreMain;//分数脚本
        //private GameObject gamePass;//通关入口
        private bool btnOn;//按键开关 因为帧率不一致导致fixupdate按键失灵
        private bool isJumping;//起跳后用于落地音效状态

        //引入系统接口
        private IObjectPoolSystem objectPool;
        private IAudioMgrSystem audioMgr;

        public Vector2 Pos => transform.position;

        private void Start()
        {
            pGroundSpeed = 8f;
            pJumpSForce = 15f;

            pAccdelta = 0.6f;
            pDecDelta = 0.9f;
            rig = GetComponent<Rigidbody2D>();//获取本挂在物体
            box = GetComponentInChildren<BoxCollider2D>();
            layerMask = LayerMask.GetMask("Ground");

            this.GetSystem<ICameraSystem>().SetTarget(this);//调用摄像机系统并给跟随的位置
            //this.RegisterEvent<DirInputEvent>(e =>
            //{
            //    inputX = e.x;
            //    inputY = e.y;
            //});
            //this.RegisterEvent<ShI>(e =>
            //{
            //    inputX = e.x;
            //    inputY = e.y;
            //});
            objectPool = this.GetSystem<IObjectPoolSystem>();
            audioMgr = this.GetSystem<IAudioMgrSystem>();

            audioMgr.PlayBgm("BGM");
            //scoreMain = GameObject.Find("ScorePanel").GetComponent<ScoreMain>();
            //gamePass = GameObject.Find("GamePass");//入口
            //gamePass.SetActive(false);//失活
        }
        private void Update()
        {
            BulletIns();
            var ground = Physics2D.OverlapBox(transform.position + 0.5f * box.size.y *
                    Vector3.down, new Vector2(box.size.x * 0.8f, 0.1f), 0, layerMask);//地面检测
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (ground)
                {
                    //播放起跳音效
                    audioMgr.PlaySound("起跳");
                    btnOn = true;
                    //起跳后状态  落地要用
                    isJumping = true;
                }
                else if (ground && isJumping)//落地音效
                {
                    audioMgr.PlaySound("落地");
                    isJumping = false;
                }

            }
        }
        //绘制
        //private void OnDrawGizmosSelected()
        //{
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawWireCube(transform.position + 0.5f * box.size.y * Vector3.down, new Vector2(box.size.x * 0.8f, 0.1f));
        //}


        private void FixedUpdate()
        {
            Move();
        }
        //主角里面不应该更新摄像机  高内聚低耦合
        //private void LateUpdate()
        //{
        //    this.GetSystem<ICameraSystem>().Update();//调用摄像机位置更新
        //}
        //检测场景转换
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("ScorePiece"))
            {//拾取道具增加积分
                GameObject.Destroy(collision.gameObject);
                this.GetModel<IGameModel>().score.Value++; //使用框架里的方式
                                                           //拾取道具音效
                audioMgr.PlaySound("吃金币");
                //scoreMain.UpdateScoreText(1);//吃一个加1分
            }
            if (collision.gameObject.CompareTag("Door"))
            {
                this.SendCommand(new NextLevelCommand("GamePassScene"));//发送命令
                                                                        //通关音效
                audioMgr.PlaySound("传送门");
            }
        }
        //防止按键失效 按键检测放入update 物理逻辑放入fixupdate
        public void Move()
        {
            //跳跃
            if (btnOn)
            {
                rig.velocity = new Vector2(rig.velocity.x, pJumpSForce);
                btnOn = false;
            }
            //移动
            float h = Input.GetAxisRaw("Horizontal");
            if (h != 0)
            {
                rig.velocity = new Vector2(Mathf.Clamp(rig.velocity.x + h * pAccdelta, -pGroundSpeed, pGroundSpeed), rig.velocity.y);
            }
            else
            {
                rig.velocity = new Vector2(Mathf.MoveTowards(rig.velocity.x, 0, pDecDelta), rig.velocity.y);
            }
            Filp(h);
        }
        public void Filp(float h)
        {
            if (h != 0 && h != faceDir)
            {
                faceDir = -faceDir;
                transform.Rotate(0, 180, 0);
            }
        }
        //子弹生成方法
       
        public void BulletIns()
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                //音效
               audioMgr.PlaySound("攻击");
                //异步加载替换
                objectPool.Get("Item/Bullet", o =>
                 {
                     o.transform.localPosition = transform.position;
                     o.GetComponent<Bullet>().InitDir(faceDir);//子弹方向
                 });
                //var bullet = Resources.Load<GameObject>("Item/Bullet");
                //bullet = GameObject.Instantiate(bullet, transform.position, Quaternion.identity);//物体生成 定位
                //var bu = bullet.GetComponent<Bullet>();
                ////bu.GetGamePass(gamePass);//传入激活失活物体
                //bu.InitDir(faceDir);//方向子弹
            }
        }

    }
}

