using Ingame.Gunplay;
using Ingame.Hud;
using Ingame.Input;
using Leopotam.Ecs;
using Support;
using Support.Extensions;
using UnityEngine;

namespace Ingame.Animation
{
    public sealed class HudItemAnimationSystem : IEcsRunSystem
    {
        private readonly EcsFilter<HudItemModel, AnimatorModel, AvailableAnimationsComponent, InInventoryTag> _gunItemModelFilter;
        
        private readonly EcsFilter<MagazineSwitchInputEvent> _magSwitchEventFilter;
        private readonly EcsFilter<DistortTheShutterInputEvent> _distortShutterEventFilter;
        private readonly EcsFilter<ShutterDelayInputEvent> _shutterDelayEventFilter;

        private const float LAYER_SWITCH_SPEED = 50f;
        private const int DISTORT_SHUTTER_LAYER = 1;
        
        public void Run()
        {
            foreach (var i in _gunItemModelFilter)
            {
                ref var hudItemEntity = ref _gunItemModelFilter.GetEntity(i);
                ref var animatorModel = ref _gunItemModelFilter.Get2(i);
                var itemAvailableAnimations = _gunItemModelFilter.Get3(i); 
                var animator = animatorModel.animator;

                if (hudItemEntity.Has<HudIsInHandsTag>()) 
                    animator.SetGameObjectActive();

                if (animator == null)
                {
                    TemplateUtils.SafeDebug($"animator is null inside {nameof(HudItemAnimationSystem)}");
                    continue;
                }
                
                if (itemAvailableAnimations.HasAnimation(AnimationType.Aim))
                    animator.SetBool("IsAiming", hudItemEntity.Has<HudIsAimingTag>());

                
                if (itemAvailableAnimations.HasAnimation(AnimationType.Reload))
                {
                    if (!_magSwitchEventFilter.IsEmpty() && hudItemEntity.Has<HudIsInHandsTag>())
                    {
                        animator.ResetTrigger("Reload");
                        animator.SetTrigger("Reload");
                    }
                }
                
                if (itemAvailableAnimations.HasAnimation(AnimationType.DistortTheShutter))
                {
                    if (!_distortShutterEventFilter.IsEmpty() && hudItemEntity.Has<HudIsInHandsTag>())
                    {
                        animator.ResetTrigger("DistortTheShutter");
                        animator.SetTrigger("DistortTheShutter");
                    }
                }
                
                if (itemAvailableAnimations.HasAnimation(AnimationType.ShutterDelay))
                {
                    if (!_shutterDelayEventFilter.IsEmpty() && hudItemEntity.Has<HudIsInHandsTag>())
                    {
                        animator.ResetTrigger("ShutterDelay");
                        animator.SetTrigger("ShutterDelay");
                    }
                }

                if (itemAvailableAnimations.HasAnimation(AnimationType.ShutterDelayLayer))
                {
                    float targetWeight = hudItemEntity.Has<ShutterIsInDelayPositionTag>() ? 1f : 0f;

                    float previousDistortShutterLayerWeight = animator.GetLayerWeight(DISTORT_SHUTTER_LAYER);
                    float currentDistortShutterLayerWeight = Mathf.Lerp(previousDistortShutterLayerWeight, targetWeight, LAYER_SWITCH_SPEED * Time.deltaTime);

                    animator.SetLayerWeight(DISTORT_SHUTTER_LAYER, currentDistortShutterLayerWeight);
                }
            }
        }
    }
}