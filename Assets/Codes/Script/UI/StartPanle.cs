using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace PlatformShoot
{
    public class StartPanle : PlatformShootGameController
    {
        private void Awake()
        {
            transform.Find("StartBtn").GetComponent<Button>().onClick.AddListener(OnStartBtn);
            transform.Find("ExitBtn").GetComponent<Button>().onClick.AddListener(OnExitBtn);
        }
        /// <summary>
        /// 退出事件
        /// </summary>
        private void OnExitBtn()
        {
            Application.Quit();
        }
        /// <summary>
        /// 开始事件
        /// </summary>
        private void OnStartBtn()
        {
            //场景跳转
            this.SendCommand(new NextLevelCommand("SampleScene"));
        }
    }
}

