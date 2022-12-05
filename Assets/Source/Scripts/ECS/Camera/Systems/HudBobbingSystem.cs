using Ingame.Hud;
using Ingame.Movement;
using Ingame.Player;
using Ingame.Utils;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.CameraWork
{
	public sealed class HudBobbingSystem : IEcsRunSystem
	{
		private readonly EcsFilter<PlayerModel, DeltaMovementComponent, CharacterControllerModel> _playerModelFilter;
		private readonly EcsFilter<HudModel, TransformModel, BobbingComponent> _hudFilter;

		public void Run()
		{
			if(_playerModelFilter.IsEmpty())
				return;

			var deltaMovement = _playerModelFilter.Get2(0).deltaMovement;
			var hudData = _playerModelFilter.Get1(0).playerHudData;

			foreach (var i in _hudFilter)
			{
				ref var hudTransformModel = ref _hudFilter.Get2(i);
				ref var bobbingComponent = ref _hudFilter.Get3(i);
				var hudTransform = hudTransformModel.transform;
				
				bobbingComponent.timeSpentTraveling += Time.deltaTime;
				
				if(deltaMovement.sqrMagnitude < .001f)
				{
					var targetLocalEulerAngles = hudTransform.localEulerAngles;
					targetLocalEulerAngles.z = hudTransformModel.initialLocalRotation.eulerAngles.z;
					
					hudTransform.localPosition = Vector3.Lerp(hudTransform.localPosition, hudTransformModel.initialLocalPos, hudData.HUDBobbingLerpingSpeed * Time.deltaTime);
					hudTransform.localRotation = Quaternion.Slerp(hudTransform.localRotation, Quaternion.Euler(targetLocalEulerAngles), hudData.HUDBobbingLerpingSpeed * Time.deltaTime);

					bobbingComponent.timeSpentTraveling = 1f;
					continue;
				}

				var cosValue = Mathf.Cos(bobbingComponent.timeSpentTraveling * hudData.HUDBobbingSpeedModifier);
				
				var positionOffset = Vector3.up * cosValue * hudData.HUDBobbingPositionStrengthY;
				var rotationOffset = Quaternion.AngleAxis(cosValue * Random.value * hudData.HUDBobbingRotationStrengthZ, Vector3.forward);

				hudTransform.position += positionOffset;
				hudTransform.rotation *= rotationOffset;
			}
		}
	}
}