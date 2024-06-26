using QFramework;

namespace PlatformShoot
{
    public class InitGameCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.GetSystem<IInputDeviceMgrSystem>().OnEnable();
            this.GetSystem<IPlayerInputSystem>().Enable();
        }
    }
}