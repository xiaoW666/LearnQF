using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformShoot
{
    public class CameraMove : MonoBehaviour
    {
        private Transform playerTage;
        private void Start()
        {
            playerTage = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void LateUpdate()//LateUpdate函数是在所有Update函数执行完毕后被调用的
        {
            MobileFollow();
        }

        public void MobileFollow()//移动跟随
        {
            transform.localPosition = new Vector3(playerTage.position.x,playerTage.position.y,-10);//localPosition相比较父级的位置
        }
    }

}
