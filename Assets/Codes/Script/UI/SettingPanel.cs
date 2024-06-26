using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System;

namespace PlatformShoot
{
    public class SettingPanel : PlatformShootGameController
    {
        private IGameAudioModel gameAudio;

        private void Awake()
        {
            transform.Find("CloseBtn").GetComponent<Button>()
               .onClick.AddListener(OnCloseSelf);

            gameAudio = this.GetModel<IGameAudioModel>();

            var bgm = transform.Find("BgmVolume").GetComponent<Slider>();
            bgm.value = gameAudio.BgmVolume.Value;
            bgm.onValueChanged.AddListener(OnBgmVolume);

            var sound = transform.Find("SoundVolume").GetComponent<Slider>();
            sound.value = gameAudio.SoundVolume.Value;
            sound.onValueChanged.AddListener(OnSoundVolume);

        }

        private void OnSoundVolume(float arg)
        {
            gameAudio.SoundVolume.Value = arg;
        }

        private void OnBgmVolume(float arg)
        {
            gameAudio.BgmVolume.Value = arg;
        }

        private void OnCloseSelf()
        {
            Destroy(gameObject);
        }
    }
}

