using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using System;

namespace PlatformShoot
{
    /// <summary>
    /// ��Ҫ�ͼ����¼� �̳нӿ�
    /// </summary>
    public class NextLevel : PlatformShootGameController
    {

        // Start is called before the first frame update
        void Start()
        {
            gameObject.SetActive(false);
            this.RegisterEvent<ShowPassDoorEvent>(OnCanGamePass)
                .UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void OnCanGamePass(ShowPassDoorEvent e)
        {
            gameObject.SetActive(true);
        }

      
    }

}
