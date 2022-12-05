using UnityEngine;
#if UNITY_EDITOR
using Ingame.Health;
using Ingame.Player;
using Leopotam.Ecs;
using NaughtyAttributes;
using Zenject;
#endif

namespace Ingame.Debug.HybridComponents
{
    public sealed class PlayerEffectsDebugger : MonoBehaviour
    {
#if UNITY_EDITOR
        [BoxGroup("Damage")] 
        [SerializeField] [Range(0, 100)] private float appliedDamage = 10f;
        [BoxGroup("Bleeding")] 
        [SerializeField] [Range(0, 100)] private float healthTakenPerSecondFromBleeding = 10f;
        [BoxGroup("Gas Choke")] 
        [SerializeField] [Range(0, 1)] private float gasAmountInLungs = .5f;
        [BoxGroup("Gas Choke")] 
        [SerializeField] [Range(0, 1)] private float gasReleasedFromLungsPerSecond = .1f;
        [BoxGroup("Gas Choke")] 
        [SerializeField] [Range(0, 100)] private float gasDamagePerSecond = 2f;
        [BoxGroup("Energy effect")] 
        [SerializeField] [Min(0)] private int numberOfEffects = 1;
        [BoxGroup("Energy effect")]
        [SerializeField] [Min(0)] private float duration = 20f;
        [BoxGroup("Energy effect")]
        [SerializeField] [Min(0)] private float movingSpeedScale = 1.2f;
        
        [Inject] private readonly EcsWorld _world;
        
        [Button]
        private void DealDamage()
        {
            ref var playerEntity = ref _world.GetFilter(typeof(EcsFilter<PlayerModel, HealthComponent>)).GetEntity(0);
            ref var bleedingComponent = ref playerEntity.Get<DamageComponent>();

            bleedingComponent.damageToDeal = appliedDamage;
        }

        [Button]
        private void AddBleeding()
        {
            ref var playerEntity = ref _world.GetFilter(typeof(EcsFilter<PlayerModel, HealthComponent>)).GetEntity(0);
            ref var bleedingComponent = ref playerEntity.Get<BleedingComponent>();

            bleedingComponent.healthTakenPerSecond = healthTakenPerSecondFromBleeding;
        }

        [Button]
        private void AddGasChoke()
        {
            ref var playerEntity = ref _world.GetFilter(typeof(EcsFilter<PlayerModel, HealthComponent>)).GetEntity(0);
            ref var gasChokeComponent = ref playerEntity.Get<GasChokeComponent>();

            gasChokeComponent.gasAmountInLungs = gasAmountInLungs;
            gasChokeComponent.gasReleasedFromLungsPerSecond = gasReleasedFromLungsPerSecond;
            gasChokeComponent.gasDamagePerSecond = gasDamagePerSecond;
        }

        [Button]
        private void AddEnergyDrinkEffect()
        {
            ref var playerEntity = ref _world.GetFilter(typeof(EcsFilter<PlayerModel, HealthComponent>)).GetEntity(0);
            ref var energyEffect = ref playerEntity.Get<EnergyEffectComponent>();

            energyEffect.numberOfEffects = numberOfEffects;
            energyEffect.duration = duration;
            energyEffect.movingSpeedScale = movingSpeedScale;
        }
#endif
    }
}