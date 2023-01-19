using NaughtyAttributes;
using UnityEngine;
using Voody.UniLeo;
using Zenject;

namespace Ingame.UI
{
	public sealed class UiGameOverScreenModelProvider : MonoProvider<UiGameOverScreenModel>
	{
		[Required, SerializeField] private UiGameOverScreen uiGameOverScreen;

		[Inject]
		private void Construct()
		{
			value = new UiGameOverScreenModel
			{
				uiGameOverScreen = uiGameOverScreen
			};
		}
	}
}