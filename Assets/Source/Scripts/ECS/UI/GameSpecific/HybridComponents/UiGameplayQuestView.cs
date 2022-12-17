using DG.Tweening;
using Ingame.Quests;
using NaughtyAttributes;
using Support.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ingame.UI
{
	public sealed class UiGameplayQuestView : MonoBehaviour
	{
		[BoxGroup("Configs")]
		[Required, SerializeField] private QuestsConfig questsConfig;
		
		[BoxGroup("References")]
		[Required, SerializeField] private TMP_Text questTitleText;
		[BoxGroup("References")]
		[Required, SerializeField] private TMP_Text questDescriptionText;
		[BoxGroup("References")]
		[Required, SerializeField] private Transform parentTransform;
		[BoxGroup("References")]
		[Required, SerializeField] private Image checkmarkImage;
		
		[BoxGroup("Quest display animation preferences")]
		[SerializeField] private float pauseBetweenSpawningLetters = .1f;
		[BoxGroup("Quest display animation preferences")]
		[SerializeField] private float scaleAnimationDuration = .5f;
		[BoxGroup("Quest display animation preferences")]
		
		[SerializeField] private float displayDuration = 2f;
		[BoxGroup("Checkmark animation preferences")]
		[SerializeField] private Vector3 hideCheckmarkLocalPosition;
		[BoxGroup("Checkmark animation preferences")]
		[SerializeField] private float checkmarkAnimationDuration = .5f;
		[BoxGroup("Checkmark animation preferences")]
		[SerializeField] private float checkmarkDisplayDuration = 1f;

		private Sequence _checkmarkAnimationSequence;

		public int CurrentlyDisplayedTreeId { get; private set; }
		public int CurrentlyDisplayedStepId { get; private set; }

		private void Awake()
		{
			parentTransform.SetGameObjectInactive();
			parentTransform.localScale = Vector3.zero;
			checkmarkImage.rectTransform.localPosition = hideCheckmarkLocalPosition;
		}

		public void SetQuestInfo(int treeId, int stepId)
		{
			var questStep = questsConfig.GetQuestStep(treeId, stepId);
			
			CurrentlyDisplayedTreeId = treeId;
			CurrentlyDisplayedStepId = stepId;

			questTitleText.StopAllCoroutines();
			questTitleText.SpawnTextCoroutine(questStep.title, pauseBetweenSpawningLetters);
			
			questDescriptionText.StopAllCoroutines();
			questDescriptionText.SpawnTextCoroutine(questStep.description, pauseBetweenSpawningLetters);
		}

		public void DisplayTemporaryQuestInfo()
		{
			StopAllCoroutines();
			
			parentTransform.SetGameObjectActive();
		
			DOTween.Sequence()
				.Append(parentTransform.DOScale(Vector3.one, scaleAnimationDuration).SetEase(Ease.InCirc))
				.AppendInterval(displayDuration)
				.Append(parentTransform.DOScale(Vector3.zero, scaleAnimationDuration).SetEase(Ease.OutCirc))
				.OnComplete(parentTransform.SetGameObjectInactive);
		}

		public void PlayQuestCompletedAnimation(int treeId, int stepId)
		{
			var questStep = questsConfig.GetQuestStep(treeId, stepId);

			DisplayTemporaryQuestInfo();
			
			questTitleText.StopAllCoroutines();
			questTitleText.SpawnTextCoroutine(questStep.title, pauseBetweenSpawningLetters);
			
			questDescriptionText.StopAllCoroutines();
			questDescriptionText.SpawnTextCoroutine(questStep.description, pauseBetweenSpawningLetters);
			
			if (_checkmarkAnimationSequence != null)
			{
				_checkmarkAnimationSequence.Kill();
				_checkmarkAnimationSequence = null;
			}

			checkmarkImage.rectTransform.localPosition = hideCheckmarkLocalPosition;

			_checkmarkAnimationSequence = DOTween.Sequence()
				.Append(checkmarkImage.rectTransform
					.DOLocalMove(Vector3.zero, checkmarkAnimationDuration)
					.SetEase(Ease.OutBack))
				.AppendInterval(checkmarkDisplayDuration)
				.Append(checkmarkImage.rectTransform
					.DOLocalMove(hideCheckmarkLocalPosition, checkmarkAnimationDuration))
				.OnComplete(() => SetQuestInfo(CurrentlyDisplayedTreeId, CurrentlyDisplayedStepId));
		}
	}
}