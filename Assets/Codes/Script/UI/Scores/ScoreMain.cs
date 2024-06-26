using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System;

namespace PlatformShoot
{
    public class ScoreMain : PlatformShootGameController
    {
        private Text scoreText;
        void Start()
        {
            scoreText = transform.Find("ScoreText").GetComponent<Text>();
            //注册一个变更事件
            this.GetModel<IGameModel>().score.RegisterWithInitValue(OnScoreChanged)//这个方法注册时候直接进行一次赋值
                .UnRegisterWhenGameObjectDestroyed(gameObject); //当前游戏对象被销毁时自动执行注销 方法注销
            transform.Find("SettingBtn").GetComponent<Button>()//音量设置
                .onClick.AddListener(OnOpenSettingBtn);
        }
        //设置打开方法
        private void OnOpenSettingBtn()
        {
            //生成一个
            if (GameObject.Find("Canvas").transform.Find("SettingPanel(Clone)"))
            {
                return;
            }
            ResHlper.AsyncLoad<GameObject>("Item/SettingPanel", o => {
                o.transform.SetParent(GameObject.Find("Canvas").transform);
                (o.transform as RectTransform).anchoredPosition = Vector2.zero;//重置位置
            });
        }

        private void OnScoreChanged(int score)
        {
            scoreText.text = score.ToString();
        }

        public void UpdateScoreText(int score)//传入奖励值 一个多少分
        {
            int temp = int.Parse(scoreText.text);//转换
            scoreText.text = (temp + score).ToString();//分数计算
        }

       
    }
}

