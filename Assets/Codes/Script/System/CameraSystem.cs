using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QFramework;
using UnityEngine;

namespace PlatformShoot
{
    /// <summary>
    /// 摄像机系统
    /// </summary>
    interface ICameraSystem : ISystem
    {
        void SetTarget(ICamTarget target);
    }
    public interface ICamTarget
    {
        Vector2 Pos { get; }
    }
    public class CameraSystem : AbstractSystem, ICameraSystem
    {

        private ICamTarget mTarget;

        private Vector3 mTargetPos;

        private float mSmoothTime = 2;
       
        /// <summary>
        /// 跟随范围 上下左右极限长度
        /// </summary>
        private float minX = -100f, minY = -100f, maxX = 100f, maxY = 100f;

        public void SetTarget(ICamTarget target)
        {
            mTarget = target;
        }

        protected override void OnInit()
        {
            //帧更新
            PublicMono.Instance.OnFixedUpdate += Update;//方法绑定
            mTargetPos.z = -10;//z轴赋值 不变
        }

        private void Update()
        {
            if (mTarget.Equals(null)) return;
            ///范围限制 相机
            mTargetPos.x = Math.Clamp(mTarget.Pos.x, minX, maxX);
            mTargetPos.y = Math.Clamp(mTarget.Pos.y, minY, maxY);
            //缓存摄像机  做缓入缓出
            var cam = Camera.main.transform;
            //j计算两点模长获取距离
            if ((cam.position - mTargetPos).sqrMagnitude < 0.01f) return;
            ///插值速度
            cam.localPosition = Vector3.Lerp(cam.position, mTargetPos, mSmoothTime * Time.deltaTime);
        
        }
    }
}
