using QFramework;

namespace PlatformShoot
{
    public class PlayerInputHandle : PlatformShootGameController
    {
       public bool JumpInput { get; set; }
       public int InputX { get; private set; }
       public int InputY { get; private set; }
       public bool AttackInput { get; private set; }

        private void Start()
        {
            this.RegisterEvent<DirInputEvent>(e =>
            {
                InputX = e.x;
                InputY = e.y;
            })
            .UnRegisterWhenGameObjectDestroyed(gameObject);

            this.RegisterEvent<ShootInputEvent>(e =>
            {
                AttackInput = e.isTrigger;
            })
            .UnRegisterWhenGameObjectDestroyed(gameObject);

            this.RegisterEvent<JumpInputEvent>(e =>
            {
                JumpInput = true;
            })
            .UnRegisterWhenGameObjectDestroyed(gameObject);
        }
    }
}