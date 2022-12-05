using System.Runtime.CompilerServices;
using Ingame.Animation;
using Ingame.Gunplay;
using Ingame.Hud;
using Ingame.Input;
using Ingame.Movement;
using Leopotam.Ecs;
using Support;
using Support.Extensions;
using UnityEngine;

namespace Ingame.Inventory
{
	public sealed class DropWeaponSystem : IEcsRunSystem
	{
		private readonly EcsFilter<FirearmComponent, InInventoryTag, HudIsInHandsTag> _firearmInHandsFilter;
		private readonly EcsFilter<DropWeaponInputEvent> _dropWeaponInputFilter;

		public void Run()
		{
			if(_dropWeaponInputFilter.IsEmpty())
				return;

			foreach (var i in _firearmInHandsFilter)
			{
				ref var firearmEntity = ref _firearmInHandsFilter.GetEntity(i);
				
				if(!TryModifyingComponentsOnWeapon(firearmEntity))
					continue;
				
				if(!TryAddingPhysicsForWeapon(firearmEntity))
					continue;
				
				if(!TryHidingHands(firearmEntity))
					continue;

				TryRemovingWeaponFromHands(firearmEntity);
			}
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool TryModifyingComponentsOnWeapon(in EcsEntity weaponEntity)
		{
			bool isWeaponInFirstSlot = weaponEntity.Has<FirstHudItemSlotTag>();
			bool isWeaponInSecondSlot = weaponEntity.Has<SecondHudItemSlotTag>();
			
			if (!isWeaponInFirstSlot && !isWeaponInSecondSlot)
			{
				TemplateUtils.SafeDebug($"Weapon is not assigned to any of the slots ({nameof(FirstHudItemSlotTag)}, {nameof(SecondHudItemSlotTag)})");
				return false;
			}

			if(isWeaponInFirstSlot)
				weaponEntity.Del<FirstHudItemSlotTag>();
			
			if(isWeaponInSecondSlot)
				weaponEntity.Del<SecondHudItemSlotTag>();
			
			weaponEntity.Del<InInventoryTag>();
			weaponEntity.Del<HudIsInHandsTag>();
			
			return true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool TryAddingPhysicsForWeapon(in EcsEntity weaponEntity)
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
			
			weaponColliderModel.collider.enabled = true;
			weaponRigidbodyModel.rigidbody.isKinematic = false;
			weaponAnimatorModel.animator.enabled = false;

			return true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool TryHidingHands(in EcsEntity weaponEntity)
		{
			if (!weaponEntity.Has<HudItemHandsModel>())
			{
				TemplateUtils.SafeDebug($"Weapon should have {typeof(HudItemHandsModel)}", LogType.Error);
				return false;
			}

			ref var hudItemHandsComponent = ref weaponEntity.Get<HudItemHandsModel>();
			hudItemHandsComponent.handsTransform.SetGameObjectInactive();
			
			return true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool TryRemovingWeaponFromHands(in EcsEntity weaponEntity)
		{
			ref var weaponTransformModel = ref weaponEntity.Get<TransformModel>();
			var weaponTransform = weaponTransformModel.transform;
			int weaponLayerIndex = LayerMask.NameToLayer("IgnoreCollisionWithPlayer");
			
			weaponTransform.SetParent(null);
			
			weaponTransform.gameObject.SetLayerToAllChildrenAndSelf(weaponLayerIndex);
			weaponTransform.SetGameObjectActive();
			
			return true;
		}
	}
}