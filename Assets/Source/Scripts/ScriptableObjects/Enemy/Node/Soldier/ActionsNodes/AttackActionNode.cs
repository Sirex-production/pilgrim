using Ingame.Behaviour;
using Ingame.Health;
using Ingame.Movement;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ingame.Enemy
{
    public class AttackActionNode : ActionNode
    {
        
        [SerializeField] 
        private float damageOnHit;
        [SerializeField] 
        [Range(0, 1)] 
        private float chanceToHit = 0.9f;
        [SerializeField] 
        private float shootIntervalTime = 1.5f;
        [SerializeField] 
        private LayerMask ignoredLayers;

        [SerializeField] private bool shouldIgnoreObstacles;
        private float _currentIntervalTime;
        protected override void ActOnStart()
        {
            _currentIntervalTime = shootIntervalTime;
        }

        protected override void ActOnStop()
        {
          
        }
        /// <summary>
        /// Try to attack after [shootIntervalTime] time
        /// </summary>
        /// <returns>Return Success if hit target, Failure if not and Running if it's still on a cooldown</returns>
        protected override State ActOnTick()
        {
            ref var enemyModel = ref Entity.Get<EnemyStateModel>();
            ref var transform = ref Entity.Get<TransformModel>();
            
            var lookPos = enemyModel.Target.position - transform.transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.transform.rotation = Quaternion.Slerp(transform.transform.rotation, rotation, 1.5f);
            //cooldown
            if (_currentIntervalTime>0)
            {
                _currentIntervalTime -= Time.deltaTime;
                return State.Running;
            }
            
            enemyModel.CurrentAmmo -= 1;
            
            //hit chance - do miss
            if (chanceToHit < Random.Range(0f, 1f))
            {
                return State.Failure;
            }
            
           

            //shoot
            if (shouldIgnoreObstacles)
            {
                
            }
            if (!Physics.Linecast(transform.transform.position, enemyModel.Target.position, out RaycastHit hit, ignoredLayers, QueryTriggerInteraction.Ignore))
            {
                return State.Failure;
            }

            if (!hit.transform.root.CompareTag("Player")) return State.Failure;
            
            hit.transform.root.TryGetComponent(out EntityReference reference);
            reference.Entity.Get<HealthComponent>().currentHealth -= damageOnHit;
            return State.Success;

        }
    }
}