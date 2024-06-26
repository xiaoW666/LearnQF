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
            //ע��һ������¼�
            this.GetModel<IGameModel>().score.RegisterWithInitValue(OnScoreChanged)//�������ע��ʱ��ֱ�ӽ���һ�θ�ֵ
                .UnRegisterWhenGameObjectDestroyed(gameObject); //��ǰ��Ϸ��������ʱ�Զ�ִ��ע�� ����ע��
            transform.Find("SettingBtn").GetComponent<Button>()//��������
                .onClick.AddListener(OnOpenSettingBtn);
        }
        //���ô򿪷���
        private void OnOpenSettingBtn()
        {
            //����һ��
            if (GameObject.Find("Canvas").transform.Find("SettingPanel(Clone)"))
            {
                return;
            }
            ResHlper.AsyncLoad<GameObject>("Item/SettingPanel", o => {
                o.transform.SetParent(GameObject.Find("Canvas").transform);
                (o.transform as RectTransform).anchoredPosition = Vector2.zero;//����λ��
            });
        }

        private void OnScoreChanged(int score)
        {
            scoreText.text = score.ToString();
        }

        public void UpdateScoreText(int score)//���뽱��ֵ һ�����ٷ�
        {
            int temp = int.Parse(scoreText.text);//ת��
            scoreText.text = (temp + score).ToString();//��������
        }

       
    }
}

