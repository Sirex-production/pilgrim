using Ingame.Enemy;
using Ingame.Health;
using Ingame.Movement;
using Leopotam.Ecs;
using Unity.Mathematics;
using UnityEngine;

namespace Ingame.Animation
{
    public sealed class SoldierAnimationSystem : IEcsRunSystem
    {
        private static readonly int _moveX = Animator.StringToHash("MoveX");
        private static readonly int _moveZ = Animator.StringToHash("MoveZ");
        private static readonly int _isCrouching = Animator.StringToHash("IsCrouching");
        private static readonly int _attack = Animator.StringToHash("Attack");
        private static readonly int _reload = Animator.StringToHash("Reload");
        private static readonly int _isDying = Animator.StringToHash("IsDying");
        private static readonly float _rigWeight = 0.555f;
        private static readonly int _iKLeftHandWeight = Animator.StringToHash("IKLeftHandWeight");
        private readonly EcsFilter<AnimatorModel,EnemyStateModel,NavMeshAgentModel, TransformModel> _soldierAnimationFilter;
        private float _offset = 70f;

        public void Run()
        {
           
            foreach (var i in _soldierAnimationFilter)
            {
                ref var entity = ref _soldierAnimationFilter.GetEntity(i);
                ref var animatorModel = ref _soldierAnimationFilter.Get1(i);
                ref var enemyStateModel = ref _soldierAnimationFilter.Get2(i);
                ref var navMeshAgentModel = ref _soldierAnimationFilter.Get3(i);
                ref var transform = ref entity.Get<TransformModel>();
                PerformMovementAnimations(ref entity, ref navMeshAgentModel, ref enemyStateModel,ref animatorModel);
                PerformAimingAnimations(ref entity,ref animatorModel,ref enemyStateModel, ref navMeshAgentModel);
                PerformDieAnimations(ref entity, ref animatorModel,ref enemyStateModel);
                
                if(enemyStateModel.isDead|| enemyStateModel.isDying || !enemyStateModel.isTargetDetected || enemyStateModel.isHiding || enemyStateModel.isPatrolling)
                    continue;
                
                var targetRotation = Quaternion.LookRotation(enemyStateModel.target.transform.position - transform.transform.position);
                targetRotation.x = 0; 
                targetRotation.z = 0;
                targetRotation *= quaternion.Euler(0,_offset,0);

                transform.transform.rotation =
                    Quaternion.Slerp(transform.transform.rotation, targetRotation, 7.5f * Time.deltaTime);
            }             
        }

        private void PerformDieAnimations(ref EcsEntity entity, ref AnimatorModel animatorModel,ref EnemyStateModel enemyStateModel )
        {
            if (!enemyStateModel.isDying) return;
           
            SmoothIKLeftHand(ref entity,ref animatorModel);
            var indexDie = animatorModel.animator.GetLayerIndex("DIE");
            animatorModel.animator.SetLayerWeight(indexDie, 1f);
            animatorModel.animator.SetBool(_isDying,enemyStateModel.isDying);
                
            var indexAim = animatorModel.animator.GetLayerIndex("Aim Layer");
            animatorModel.animator.SetLayerWeight(indexAim, 0f);

          
        }
        
        private void PerformMovementAnimations(ref EcsEntity entity,ref NavMeshAgentModel navMeshAgentModel,ref EnemyStateModel enemyStateModel, ref AnimatorModel animatorModel)
        {
            ref var transformModel= ref entity.Get<TransformModel>();
            navMeshAgentModel.Agent.updatePosition = false;
            var animator = animatorModel.animator;
                
            var worldDeltaPosition =
                navMeshAgentModel.Agent.nextPosition - transformModel.transform.position;
            worldDeltaPosition.y = 0;
            var groundDeltaX = Vector3.Dot(transformModel.transform.right, worldDeltaPosition);
            var groundDeltaZ = Vector3.Dot(transformModel.transform.forward, worldDeltaPosition);
                
            var velocity = (Time.deltaTime > 1e-4f) ? new Vector2(groundDeltaX,groundDeltaZ) / Time.deltaTime : Vector2.zero;
            bool shouldMove= velocity.magnitude > 0.025f &&
                             navMeshAgentModel.Agent.remainingDistance > navMeshAgentModel.Agent.radius;
            
            animator.SetFloat(_moveX,shouldMove?velocity.x:0);
            animator.SetFloat(_moveZ,shouldMove?velocity.y:0);
            animator.SetBool(_isCrouching,enemyStateModel.isCrouching);
            transformModel.transform.position = navMeshAgentModel.Agent.nextPosition;
        }

        private void PerformAimingAnimations(ref EcsEntity entity, ref AnimatorModel animatorModel, ref EnemyStateModel enemyStateModel, ref NavMeshAgentModel navMeshAgentModel)
        {
            if (entity.Has<RigModel>())
            {
                SmoothIKLeftHand(ref entity, ref animatorModel);
            }

            var animator = animatorModel.animator;
           animator.SetBool(_attack,enemyStateModel.isAttacking);
           animator.SetBool(_reload,enemyStateModel.isReloading);
        }

        private void SmoothIKLeftHand(ref EcsEntity entity ,ref AnimatorModel animatorModel)
        {
            var weight = animatorModel.animator.GetFloat(_iKLeftHandWeight);
            entity.Get<RigModel>().rig.weight = Mathf.Clamp(weight, 0f, _rigWeight);
        }
    }
}