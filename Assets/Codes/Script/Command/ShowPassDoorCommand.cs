using QFramework;

namespace PlatformShoot
{
    //命令继承
    public class ShowPassDoorCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.SendEvent<ShowPassDoorEvent>();//发送事件
        }
    }
}
