using Ingame.Animation;
using Ingame.Hud;
using Ingame.Inventory;
using Ingame.SupportCommunication;
using LeoEcsPhysics;
using Leopotam.Ecs;

namespace Ingame.Utils
{
    public sealed class ExternalEventsRemoverSystem : IEcsRunSystem
    {
        //Inventory
        private readonly EcsFilter<UpdateBackpackAppearanceEvent> _updateInventoryEventFilter;
        //Physics
        private readonly EcsFilter<OnTriggerEnterEvent> _filterEnter;
        private readonly EcsFilter<OnTriggerStayEvent> _onTriggerStayEventFilter;
        private readonly EcsFilter<OnTriggerExitEvent> _filterExit;
        private readonly EcsFilter<OnCollisionEnterEvent> _collisionEnterEventFilter;
        private readonly EcsFilter<OnCollisionExitEvent> _collisionExitEventFilter;
        //Support messages
        private readonly EcsFilter<LevelEndRequest> _levelEndRequestFilter;
        //Utils
        private readonly EcsFilter<UpdateSettingsRequest> _updateSettingsRequestFilter;
        //Gunplay
        private readonly EcsFilter<RecoilRequest> _recoilRequestFilter;
        //Animation
        private readonly EcsFilter<UpdateItemVisibilityAnimationCallbackEvent> _updateItemsVisibilityCallbackFilter;
        private readonly EcsFilter<PerformDistortShutterAnimationCallbackEvent> _performDistortShutterAnimationCallbackFilter;
        private readonly EcsFilter<PerformMagazineSwitchAnimationCallback> _performMagazineSwitchCallbackFilter;
        private readonly EcsFilter<PerformShutterDelayAnimationCallbackEvent> _performShutterDelayCallbackFilter;

        public void Run()
        {
            foreach (var i in _updateInventoryEventFilter)
            {
                ref var eventEntity = ref _updateInventoryEventFilter.GetEntity(i); 
                eventEntity.Del<UpdateBackpackAppearanceEvent>();
            }

            foreach (var i in _filterEnter)
            {
                ref var eventEntity = ref _filterEnter.GetEntity(i);
                eventEntity.Del<OnTriggerEnterEvent>();
            }
            
            foreach (var i in _onTriggerStayEventFilter)
            {
                ref var eventEntity = ref _onTriggerStayEventFilter.GetEntity(i);
                eventEntity.Del<OnTriggerStayEvent>();
            }

            foreach (var i in _filterExit)
            {
                ref var eventEntity = ref _filterExit.GetEntity(i);
                eventEntity.Del<OnTriggerExitEvent>();
            }

            foreach (var i in _collisionEnterEventFilter)
            {
                ref var eventEntity = ref _collisionEnterEventFilter.GetEntity(i);
                eventEntity.Del<OnCollisionEnterEvent>();
            }
            
            foreach (var i in _collisionExitEventFilter)
            {
                ref var eventEntity = ref _collisionExitEventFilter.GetEntity(i);
                eventEntity.Del<OnCollisionExitEvent>();
            }
            
            foreach (var i in _levelEndRequestFilter)
            {
                ref var eventEntity = ref _levelEndRequestFilter.GetEntity(i);
                eventEntity.Del<LevelEndRequest>();
            }
            
            foreach (var i in _updateSettingsRequestFilter)
            {
                ref var eventEntity = ref _updateSettingsRequestFilter.GetEntity(i);
                eventEntity.Del<UpdateSettingsRequest>();
            }

            foreach (var i in _recoilRequestFilter)
            {
                ref var eventEntity = ref _recoilRequestFilter.GetEntity(i);
                eventEntity.Del<RecoilRequest>();
            }
            
            foreach (var i in _updateItemsVisibilityCallbackFilter)
            {
                ref var eventEntity = ref _updateItemsVisibilityCallbackFilter.GetEntity(i);
                eventEntity.Del<UpdateItemVisibilityAnimationCallbackEvent>();
            }
            
            foreach (var i in _performDistortShutterAnimationCallbackFilter)
            {
                ref var eventEntity = ref _performDistortShutterAnimationCallbackFilter.GetEntity(i);
                eventEntity.Del<PerformDistortShutterAnimationCallbackEvent>();
            }
            
            foreach (var i in _performMagazineSwitchCallbackFilter)
            {
                ref var eventEntity = ref _performMagazineSwitchCallbackFilter.GetEntity(i);
                eventEntity.Del<PerformMagazineSwitchAnimationCallback>();
            }
            
            foreach (var i in _performShutterDelayCallbackFilter)
            {
                ref var eventEntity = ref _performShutterDelayCallbackFilter.GetEntity(i);
                eventEntity.Del<PerformShutterDelayAnimationCallbackEvent>();
            }
        }
    }
}