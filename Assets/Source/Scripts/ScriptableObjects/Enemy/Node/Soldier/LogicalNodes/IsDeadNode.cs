﻿using Ingame.Behaviour;
using Ingame.Health;
using Leopotam.Ecs;

namespace Ingame.Enemy
{
    public sealed class IsDeadNode : ActionNode
    {
        protected override void ActOnStart()
        {
            
        }

        protected override void ActOnStop()
        {
             
        }

        protected override State ActOnTick()
        {
            return entity.Get<EnemyStateModel>().isDead ? State.Success : State.Failure;
        }
    }
}