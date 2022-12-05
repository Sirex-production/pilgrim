using System;
using Ingame.Breakable;
using Ingame.Enemy;
using Ingame.Health;
 
using Ingame.Movement;
using Leopotam.Ecs;
using UnityEngine;
using static UnityEngine.Time;
using Object = UnityEngine.Object;


namespace Ingame.Interaction.Explosive
{
    public sealed class ExplosionSystem : IEcsRunSystem
    {
        private EcsFilter<HandGrenadeModel,TransformModel,GrenadeTriggered> _handGrenadeFilter; 
        
        public void Run()
        {
            foreach (var i in _handGrenadeFilter)
            {
                ref var grenadeModel = ref _handGrenadeFilter.Get1(i);
                ref var grenadeEntity= ref _handGrenadeFilter.GetEntity(i);
                grenadeModel.TimeLeftToExplode -= deltaTime;
 
                
                if (grenadeModel.TimeLeftToExplode <= 0)
                {
                    ref var transformModel = ref _handGrenadeFilter.Get2(i);

                    var grenadeData = grenadeModel.GrenadeData;
                    
                    Collider[] hitColliders = Physics.OverlapSphere(transformModel.transform.position, grenadeData.Range);
                    foreach (var hit in hitColliders)
                    {
                        if (hit.gameObject.TryGetComponent(out Rigidbody rb))
                        {
                            rb.AddExplosionForce(grenadeData.KnockbackForce,transformModel.transform.position,grenadeData.Range);
                        }
                        if (!hit.gameObject.TryGetComponent(out EntityReference entityRef))
                        {
                            continue;
                        }
                        ref var hitEntity = ref entityRef.Entity;
           
                        if (hitEntity.Has<BreakableModel>())
                        {
                            hitEntity.Get<BreakableModel>();
                        }
                        
                        if (hitEntity.Has<HealthComponent>())
                        {
                            ref var health = ref hitEntity.Get<HealthComponent>();
                            health.currentHealth -= grenadeData.DamageOnBlast;
                        }
                    }
                    Object.Destroy(transformModel.transform.gameObject);
                    grenadeEntity.Destroy();
                }
            }
        }
    }
}