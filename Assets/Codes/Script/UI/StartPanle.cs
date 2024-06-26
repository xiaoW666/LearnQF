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
        /// �˳��¼�
        /// </summary>
        private void OnExitBtn()
        {
            Application.Quit();
        }
        /// <summary>
        /// ��ʼ�¼�
        /// </summary>
        private void OnStartBtn()
        {
            //������ת
            this.SendCommand(new NextLevelCommand("SampleScene"));
        }
    }
}

