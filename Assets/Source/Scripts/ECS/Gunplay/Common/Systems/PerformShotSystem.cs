using System.Runtime.CompilerServices;
using Ingame.Enemy;
using Ingame.Extensions;
using Ingame.Health;
using Ingame.VFX;
using Leopotam.Ecs;
using Support;
using UnityEngine;

namespace Ingame.Gunplay
{
    public sealed class PerformShotSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world;
        private readonly EcsFilter<FirearmComponent, AwaitingShotTag> _shootingFirearmFilter;

        public void Run()
        {
            foreach (var i in _shootingFirearmFilter)
            {
                ref var firearmEntity = ref _shootingFirearmFilter.GetEntity(i);
                ref var firearmComponent = ref _shootingFirearmFilter.Get1(i);
                
                firearmEntity.Del<AwaitingShotTag>();
                _world.CreateNoiseEvent(firearmComponent.barrelOrigin.position);
                if (!TryPerformRaycast(firearmComponent.barrelOrigin.position, firearmComponent.barrelOrigin.forward, out RaycastHit hit))
                    continue;
                
                SendVfxRequest(hit.point, hit.normal, hit.transform.tag);
                
                if(!TryApplyDamage(hit.collider.gameObject, firearmComponent.firearmConfig.Damage) && !TryApplyDamage(hit.transform.root.gameObject, firearmComponent.firearmConfig.Damage) )
                    continue;
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool TryPerformRaycast(in Vector3 originPos, in Vector3 direction, out RaycastHit hit)
        {
            var ray = new Ray(originPos, direction);
            int layerMask = ~LayerMask.GetMask("Ignore Raycast", "PlayerStatic", "HUD", "Weapon");

            return Physics.Raycast(ray, out hit, 1000, layerMask, QueryTriggerInteraction.Ignore);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool TryApplyDamage(GameObject gameObject, float damage)
        {
            damage = Mathf.Max(0, damage);

            if (damage <= 0) 
                TemplateUtils.SafeDebug("Damage of the shot is less then 0", LogType.Warning);

            if(!gameObject.TryGetComponent(out EntityReference entityReference))
                return false;

            if(!entityReference.Entity.Has<HealthComponent>())
                return false;

            ref var appliedDamageComponent = ref entityReference.Entity.Get<DamageComponent>();
            appliedDamageComponent.damageToDeal = damage;

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SendVfxRequest(in Vector3 hitPos, in Vector3 surfaceNormalDirection, in string surfaceTag)
        {
            _world.SendSignal(new PlaceBulletVfxRequest
            {
                position = hitPos,
                surfaceNormalDirection = surfaceNormalDirection,
                surfaceTag = surfaceTag
            });
        }
    }
}