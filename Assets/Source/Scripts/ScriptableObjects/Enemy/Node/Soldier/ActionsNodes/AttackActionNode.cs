using System;
using Ingame.Behaviour;
using Ingame.Health;
using Ingame.Movement;
using Ingame.Player;
using Leopotam.Ecs;
using Unity.Mathematics;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Ingame.Enemy
{
    public class AttackActionNode : ActionNode
    {
        
        enum TypeOfAttack
        {
            UnfairAccuracy,
            ProgressiveAccuracy,
            SimplifiedRayCast
        }

        [SerializeField] 
        private TypeOfAttack typeOfAttack;
        [SerializeField] 
        private float damageOnHit;
        [SerializeField] 
        [Range(0, 1)] 
        private float chanceToHit = 0.9f;
        [SerializeField] 
        private float shootIntervalTime = 1.5f;
        
        [SerializeField] 
        [Range(0, 1)] 
        private float chanceToInflictBleed  = 0.215f;
        
         [SerializeField] 
         [Min(0)] 
         private float damageOnBleed = 3.25f;
         
         private float _minAccuracy = 0.15f;
         private float _accurencyRate = 0.045f;
         private float _currentIntervalTime;
      
        protected override void ActOnStart()
        {
            _currentIntervalTime = shootIntervalTime;
            entity.Get<EnemyStateModel>().isAttacking = true;
        }

        protected override void ActOnStop()
        {
            entity.Get<EnemyStateModel>().isAttacking = false;
        }
        /// <summary>
        /// Try to attack after [shootIntervalTime] time
        /// </summary>
        /// <returns>Return Success if hit target, Failure if not and Running if it's still on a cooldown</returns>
        protected override State ActOnTick()
        {
            ref var enemyModel = ref entity.Get<EnemyStateModel>();
            ref var transformModel = ref entity.Get<TransformModel>();

            //cooldown
            if (_currentIntervalTime>0)
            {
                _currentIntervalTime -= Time.deltaTime;
                return State.Running;
            }
            
            enemyModel.currentAmmo -= 1;
            
            //hit chance - do miss
            audioService.Play3D("gun","shoot",transformModel.transform, true);
            if (typeOfAttack == TypeOfAttack.UnfairAccuracy)
            {
                if (chanceToHit < Random.Range(0f, 1f))
                {
                    return State.Failure;
                }
            }

            if (typeOfAttack == TypeOfAttack.ProgressiveAccuracy)
            {
                float chanceToHit = enemyModel.visibleTargetPixels * _accurencyRate;
                float totalAcc = Math.Clamp(chanceToHit,_minAccuracy,this.chanceToHit);
                if (totalAcc < Random.Range(0f, 1f))
                {
                    return State.Failure;
                }
            }
            
            if (typeOfAttack == TypeOfAttack.SimplifiedRayCast)
            {
                if (chanceToHit < Random.Range(0f, 1f) || !enemyModel.isTargetVisible)
                {
                    return State.Failure;
                }
            }
            
            if (!enemyModel.target.TryGetComponent(out EntityReference targetEntityReference) || !targetEntityReference.Entity.Has<HealthComponent>())
                return State.Failure;

            if (Random.Range(0f, 1f) > chanceToInflictBleed && targetEntityReference.Entity.Has<PlayerModel>())
            {
                ref var bleedingComponent = ref targetEntityReference.Entity.Get<BleedingComponent>();
                bleedingComponent.healthTakenPerSecond = damageOnBleed;
            }
            
            targetEntityReference.Entity.Get<HealthComponent>().currentHealth -= damageOnHit;
            return State.Success;

        }
    }
}