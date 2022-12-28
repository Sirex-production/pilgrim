using Ingame.Behaviour;
using Leopotam.Ecs;

namespace Ingame.Enemy
{
    public class TakeCoverActionNode : RepositionActionNode
    {
        protected override void ActOnStart()
        {
            base.ActOnStart();
            Entity.Get<EnemyStateModel>().isHiding = true;
        }

        protected override void ActOnStop()
        {
            base.ActOnStop();
            Entity.Get<EnemyStateModel>().isHiding = false;
        }
    }
}