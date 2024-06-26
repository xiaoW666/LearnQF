using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QFramework;

namespace PlatformShoot
{
    public interface IGameModel : IModel//创建接口
    {
        BindableProperty<int> score { get; }//分数定义 泛型 属性
    }
    public class GameModel : AbstractModel, IGameModel
    {
        BindableProperty<int> IGameModel.score { get; } = new BindableProperty<int>(0);//实现接口成员 默认分数为零

        protected override void OnInit()
        {
            
        }
    }
}
