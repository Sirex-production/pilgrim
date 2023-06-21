﻿using Ingame.Health;
using Ingame.Hud;
using Ingame.Inventory;
using LeoEcsPhysics;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Anomaly
{
    public sealed class AcidWaterSystem : IEcsRunSystem
    {
        private readonly EcsFilter<OnTriggerEnterEvent> _filterEnter;
        private readonly EcsFilter<OnTriggerExitEvent> _filterExit;
        private readonly EcsFilter<OnAcidWaterEffectComponent> _filterEffect;
        private readonly EcsFilter<HelmetTag, InInventoryTag, HudIsInHandsTag> _helmetInInventoryFilter;
        
        public void Run()
        {
            if(!_helmetInInventoryFilter.IsEmpty())
                return;
            
            UnityEngine.Debug.Log("AAAAAAAAA");
            
            foreach (var i in _filterEnter)
            { 
                ref var collisionEvent = ref _filterEnter.Get1(i);
                if (!collisionEvent.senderGameObject.gameObject.TryGetComponent(out EntityReference e))
                {
                    continue;
                }

                if (!e.Entity.Has<AcidWaterModel>())
                {
                    continue;
                }

                ref var acidWater = ref e.Entity.Get<AcidWaterModel>();
                if (collisionEvent.collider.gameObject.TryGetComponent(out EntityReference entityReference))
                {
                    ref var entity = ref entityReference.Entity;
                    if (!entity.Has<HealthComponent>())
                    {
                        continue;
                    }

                    if (!entity.Has<OnAcidWaterEffectComponent>())
                    {
                        ref var effect = ref entity.Get<OnAcidWaterEffectComponent>();
                        effect.WaterModel = e.Entity.Get<AcidWaterModel>();
                        effect.DamageTakenOnCooldown = 0;
                    }
                } 
            }
            
            foreach (var i in _filterExit)
            {
               
                ref var collisionEvent = ref _filterExit.Get1(i);
                if (!collisionEvent.senderGameObject.gameObject.TryGetComponent(out EntityReference e))
                {
                    continue;
                }

                if (!e.Entity.Has<AcidWaterModel>())
                {
                    continue;
                }
                if (collisionEvent.collider.gameObject.TryGetComponent(out EntityReference entityReference))
                {
                    ref var entity = ref entityReference.Entity;
                    if (!entity.Has<HealthComponent>())
                    {
                        continue;
                    }

                    if (entity.Has<OnAcidWaterEffectComponent>())
                    {
                        entity.Del<OnAcidWaterEffectComponent>();
                    }
                }
            }

            foreach (var i in _filterEffect)
            {
                ref var entity = ref _filterEffect.GetEntity(i);
                ref var effect = ref entity.Get<OnAcidWaterEffectComponent>();
                if (effect.DamageTakenOnCooldown>0)
                {
                    effect.DamageTakenOnCooldown -= Time.deltaTime;
                    continue;
                }

                effect.DamageTakenOnCooldown = effect.WaterModel.TimeFrame;
                ref var damage = ref entity.Get<DamageComponent>();
                damage.damageToDeal = effect.WaterModel.HealthTakenPerTimeFrame;
                ref var hp = ref entity.Get<HealthComponent>();
            }
        }
    }
}