using System.Runtime.CompilerServices;
using Ingame.CameraWork;
using Ingame.Hud;
using Ingame.Interaction.Common;
using Ingame.Movement;
using Ingame.Player;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;

namespace Ingame.UI
{
	public sealed class ShowHideInteractIconSystem : IEcsRunSystem
	{
		private const float INTERACT_IMAGE_ALPHA_LERPING_SPEED = 30f;
		
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
				var interactionImageTransform = interactionImage.transform;
				var ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
				int layerMask = ~LayerMask.GetMask("Helmet", "PlayerStatic", "Ignore Raycast");
				var targetColor = interactionImage.color;

				if (!Physics.Raycast(ray, out RaycastHit hit, playerInteractionDistance, layerMask))
				{
					HideImage(interactionImage);
					continue;
				}

				if (!hit.collider.TryGetComponent(out EntityReference entityReference))
				{
					HideImage(interactionImage);
					continue;
				}

				if (!entityReference.Entity.Has<InteractiveTag>() || entityReference.Entity.Has<HudIsInHandsTag>())
				{
					HideImage(interactionImage);
					continue;
				}

				ShowImage(interactionImage);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void ShowImage(Image image)
		{
			var targetColor = image.color;
			float lerpT = INTERACT_IMAGE_ALPHA_LERPING_SPEED * Time.deltaTime;
			
			targetColor.a = 1;
			image.color = Color.Lerp(image.color, targetColor, lerpT);
			image.transform.localScale = Vector3.Lerp(image.transform.localScale, Vector3.one, lerpT);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void HideImage(Image image)
		{
			var targetColor = image.color;
			float lerpT = INTERACT_IMAGE_ALPHA_LERPING_SPEED * Time.deltaTime;
			
			targetColor.a = 0;
			image.color = Color.Lerp(image.color, targetColor, lerpT);
			image.transform.localScale = Vector3.Lerp(image.transform.localScale, Vector3.zero, lerpT);
		}
	}
}