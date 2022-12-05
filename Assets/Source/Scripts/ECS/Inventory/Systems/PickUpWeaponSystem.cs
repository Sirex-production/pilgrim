using System.Runtime.CompilerServices;
using Ingame.Animation;
using Ingame.Gunplay;
using Ingame.Hud;
using Ingame.Interaction.Common;
using Ingame.Movement;
using Leopotam.Ecs;
using Support;
using Support.Extensions;
using UnityEngine;

namespace Ingame.Inventory
{
	public sealed class PickUpWeaponSystem : IEcsRunSystem
	{
		private readonly EcsFilter<FirearmComponent, TransformModel, HudItemModel, PerformInteractionTag>.Exclude<InInventoryTag, HudIsInHandsTag> _droppedWeaponFilter;
		private readonly EcsFilter<TransformModel, HudPlayerItemContainerComponent> _hudItemContainerFilter;
		private readonly EcsFilter<FirstHudItemSlotTag> _firstSlotItemFilter;
		private readonly EcsFilter<SecondHudItemSlotTag> _secondSlotItemFilter;

		public void Run()
		{
			if(_hudItemContainerFilter.IsEmpty())
				return;

			ref var hudItemTransform = ref _hudItemContainerFilter.Get1(0);
			bool isFirstSlotAvailable = _firstSlotItemFilter.IsEmpty();
			bool isSecondSlotAvailable = _secondSlotItemFilter.IsEmpty();
			
			if(!isFirstSlotAvailable && !isSecondSlotAvailable)
				return;

			foreach (var i in _droppedWeaponFilter)
			{
				ref var weaponEntity = ref _droppedWeaponFilter.GetEntity(i);

				if(!TryModifyingComponentsOnWeapon(weaponEntity, isFirstSlotAvailable, isSecondSlotAvailable))
					continue;

				if(!TryRemovingPhysicsForWeapon(weaponEntity))
					continue;
				
				if(!TryActivatingHands(weaponEntity))
					continue;
				
				TryPlacingWeaponInHands(weaponEntity, hudItemTransform);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool TryModifyingComponentsOnWeapon(in EcsEntity weaponEntity, bool isFirstSlotAvailable, bool isSecondSlotAvailable)
		{
			if (!isFirstSlotAvailable && !isSecondSlotAvailable)
			{
				TemplateUtils.SafeDebug("There is no available slots for weapon", LogType.Error);
				return false;
			}

			weaponEntity.Del<PerformInteractionTag>();

			if (isFirstSlotAvailable)
				weaponEntity.Get<FirstHudItemSlotTag>();
			else
				weaponEntity.Get<SecondHudItemSlotTag>();
			
			weaponEntity.Get<InInventoryTag>();

			return true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool TryRemovingPhysicsForWeapon(in EcsEntity weaponEntity)
		{
			if (!weaponEntity.Has<ColliderModel>())
			{
				TemplateUtils.SafeDebug($"Weapon should have {typeof(ColliderModel)}", LogType.Error);
				return false;
			}

			if (!weaponEntity.Has<RigidbodyModel>())
			{
				TemplateUtils.SafeDebug($"Weapon should have {typeof(RigidbodyModel)}", LogType.Error);
				return false;
			}
			
			if (!weaponEntity.Has<AnimatorModel>())
			{
				TemplateUtils.SafeDebug($"Weapon should have {typeof(AnimatorModel)}", LogType.Error);
				return false;
			}
			
			ref var weaponColliderModel = ref weaponEntity.Get<ColliderModel>();
			ref var weaponRigidbodyModel = ref weaponEntity.Get<RigidbodyModel>();
			ref var weaponAnimatorModel = ref weaponEntity.Get<AnimatorModel>();

			weaponColliderModel.collider.enabled = false;
			weaponRigidbodyModel.rigidbody.isKinematic = true;
			weaponAnimatorModel.animator.enabled = true;

			return true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool TryActivatingHands(in EcsEntity weaponEntity)
		{
			if (!weaponEntity.Has<HudItemHandsModel>())
			{
				TemplateUtils.SafeDebug($"Weapon should have {typeof(HudItemHandsModel)}", LogType.Error);
				return false;
			}

			ref var hudItemHandsComponent = ref weaponEntity.Get<HudItemHandsModel>();
			hudItemHandsComponent.handsTransform.SetGameObjectActive();

			return true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool TryPlacingWeaponInHands(in EcsEntity weaponEntity, in TransformModel hudItemContainerTransformModel)
		{
			ref var weaponHudItemModel = ref weaponEntity.Get<HudItemModel>();
			ref var weaponTransformModel = ref weaponEntity.Get<TransformModel>();
			var weaponTransform = weaponTransformModel.transform;
			var itemContainerTransform = hudItemContainerTransformModel.transform;
			int hudLayerIndex = LayerMask.NameToLayer("HUD");
			
			weaponTransform.SetParent(itemContainerTransform);
			weaponTransformModel.initialLocalPos = weaponHudItemModel.localPositionInHud; 
			weaponTransformModel.initialLocalRotation = weaponHudItemModel.localRotationInHud; 
			weaponTransform.localPosition = weaponHudItemModel.localPositionInHud;
			weaponTransform.localRotation = weaponHudItemModel.localRotationInHud;
			
			weaponTransform.gameObject.SetLayerToAllChildrenAndSelf(hudLayerIndex);
			weaponTransform.SetGameObjectInactive();

			return true;
		}
	}
}