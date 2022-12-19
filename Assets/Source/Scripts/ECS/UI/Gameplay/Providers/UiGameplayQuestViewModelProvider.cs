using NaughtyAttributes;
using UnityEngine;
using Voody.UniLeo;
using Zenject;

namespace Ingame.UI
{
	public sealed class UiGameplayQuestViewModelProvider : MonoProvider<UiGameplayQuestViewModel>
	{
		[Required, SerializeField] private UiGameplayQuestView uiGameplayQuestView;

		[Inject]
		private void Construct()
		{
			value = new UiGameplayQuestViewModel
			{
				uiGameplayQuestView = uiGameplayQuestView
			};
		}
	}
}