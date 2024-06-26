using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QFramework;
using UnityEngine.SceneManagement;

namespace PlatformShoot
{
    class NextLevelCommand : AbstractCommand
    {
        private readonly String sceneNmae;
        public NextLevelCommand(String _sceneNmae)
        {
            sceneNmae = _sceneNmae;
        }
        protected override void OnExecute()//执行
        {
            SceneManager.LoadScene(sceneNmae);
        }
    }
}
