using Ingame.CameraWork;
using Ingame.Hud;
using Ingame.Interaction.Common;
using Ingame.Movement;
using Ingame.Player;
using Leopotam.Ecs;
using Support.Extensions;
using UnityEngine;

namespace Ingame.UI
{
	public sealed class ShowHideInteractIconSystem : IEcsRunSystem
	{
		private readonly EcsFilter<ImageModel, UiGameplayInteractIconTag> _interactIconFilter;
		private readonly EcsFilter<PlayerModel, TransformModel> _playerModelFilter;
		private readonly EcsFilter<CameraModel, MainCameraTag> _mainCameraFilter;

		public void Run()
		{
			if(_playerModelFilter.IsEmpty() || _mainCameraFilter.IsEmpty())
				return;

			float playerInteractionDistance = _playerModelFilter.Get1(0).playerMovementData.InteractionDistance;
			var mainCamera = _mainCameraFilter.Get1(0).camera;

			foreach (var i in _interactIconFilter)
			{
				var interactionImage = _interactIconFilter.Get1(i).image;
				var ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
				int layerMask = ~LayerMask.GetMask("Helmet", "PlayerStatic", "Ignore Raycast");

				if (!Physics.Raycast(ray, out RaycastHit hit, playerInteractionDistance, layerMask))
				{
					interactionImage.SetGameObjectInactive();
					continue;
				}

				if (!hit.collider.TryGetComponent(out EntityReference entityReference))
				{
					interactionImage.SetGameObjectInactive();
					continue;
				}

				if (!entityReference.Entity.Has<InteractiveTag>() || entityReference.Entity.Has<HudIsInHandsTag>())
				{
					interactionImage.SetGameObjectInactive();
					continue;
				}

				interactionImage.SetGameObjectActive();
			}
		}
	}
}