using UnityEngine;
using QFramework;

namespace PlatformShoot
{//继承底层
    public class PlatformShootGame : Architecture<PlatformShootGame>
    {
        protected override void Init()
        {///底层注册系统  接口和继承类绑定

            //RegisterUtility<IJsonConfig>(new LoadConfig());
            RegisterModel<IGameModel>(new GameModel());//  接口注册  外部使用它
            RegisterModel<IGameAudioModel>(new GameAudioModel());

            RegisterSystem<IInputDeviceMgrSystem>(new InputDeviceMgrSystem());
            RegisterSystem<IPlayerInputSystem>(new PlayerInputSystem());

            RegisterSystem<IObjectPoolSystem>(new ObjectPoolSystem());
            RegisterSystem<IAudioMgrSystem>(new AudioMgrSystem());
            RegisterSystem<ITimerSystem>(new TimeSystem());
            RegisterSystem<ICameraSystem>(new CameraSystem());//  接口注册  外部使用它
          

        }
    }
    public class PlatformShootGameController: MonoBehaviour ,IController
    {
        IArchitecture IBelongToArchitecture.GetArchitecture() =>PlatformShootGame.Interface;
        
    }
}
