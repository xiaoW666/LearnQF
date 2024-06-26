using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PlatformShoot
{
    public class GamePassPanle : PlatformShootGameController
    {
        private void Start()
        {
            transform.Find("Button").GetComponent<Button>().onClick.AddListener(()=>
            {
                SceneManager.LoadScene("SampleScene");
            });
        }
    }
}

