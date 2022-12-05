using Ingame.Behaviour;
using Leopotam.Ecs;
using UnityEngine;
using  Ingame.Support;
namespace Ingame.Enemy
{
    public sealed class RagdollActionNode : ActionNode
    {
        private Animator _animator;
        private const string ANIMATION_NAME = "RAGDOLL";
        protected override void ActOnStart()
        {
            _animator = Entity.Get<AnimatorModel>().Animator;
            _animator.Play(ANIMATION_NAME);
        }

        protected override void ActOnStop()
        {
             
        }

        protected override State ActOnTick()
        {
            //is dying
            _animator.Play(ANIMATION_NAME);
            var animationState = _animator.IsAnimationPlaying(ANIMATION_NAME);
            //animation has ended
            if (!animationState)
            {
                //is already dead
                ref var enemyStateModel = ref Entity.Get<EnemyStateModel>();
                enemyStateModel.IsDead = true;
                enemyStateModel.IsDying = false;
                return State.Success;
            }
            return State.Running;
        }
    }
}