using DG.Tweening;
using Ingame.Quests;
using NaughtyAttributes;
using Support;
using Support.Extensions;
using TMPro;
using UnityEngine;

namespace Ingame.UI
{
	public sealed class UiGameplayQuestView : MonoBehaviour
	{
		private enum DisplayType
		{
			None,
			ActiveQuest,
			AllQuests
		}
		
		[BoxGroup("Configs")]
		[Required, SerializeField] private QuestsConfig questsConfig;
		
		[BoxGroup("References current quest")]
		[Required, SerializeField] private RectTransform activeQuestParentTransform;
		[BoxGroup("References current quest")]
		[Required, SerializeField] private TMP_Text activeQuestTitleText;
		[BoxGroup("References current quest")]
		[SerializeField] private TMP_Text[] activeQuestStepsLabels;
		
		[BoxGroup("References all quests")]
		[Required, SerializeField] private RectTransform allQuestsQuestTransform;
		[BoxGroup("References all quests")]
		[SerializeField] private TMP_Text[] allQuestTitlesLabels;
		
		[BoxGroup("Animation properties")]
		[SerializeField] [Min(0f)] private float showHideAnimationDuration = .1f;
		[BoxGroup("Animation properties")]
		[SerializeField] [Min(0f)] private float infoDisplayTime = 2f;

		private DisplayType _currentDisplayType;

		private void Awake()
		{
			_currentDisplayType = DisplayType.None;
			
			activeQuestParentTransform.localScale = new Vector3(1, 0, 1);
			activeQuestParentTransform.SetGameObjectInactive();
			
			allQuestsQuestTransform.localScale = new Vector3(1, 0, 1);
			allQuestsQuestTransform.SetGameObjectInactive();
		}

		private void HideActiveQuest()
		{
			activeQuestParentTransform.DOScaleY(0, showHideAnimationDuration)
				.OnComplete(activeQuestParentTransform.SetGameObjectInactive)
				.OnComplete(() => _currentDisplayType = DisplayType.None);
		}

		private void HideAllQuests()
		{
			allQuestsQuestTransform.DOScaleY(0, showHideAnimationDuration)
				.OnComplete(allQuestsQuestTransform.SetGameObjectInactive)
				.OnComplete(() => _currentDisplayType = DisplayType.None);
		}

		private void ShowActiveQuest()
		{
			if(_currentDisplayType == DisplayType.ActiveQuest)
				return;
			
			if(_currentDisplayType == DisplayType.AllQuests)
				HideAllQuests();
			
			activeQuestParentTransform.SetGameObjectActive();
			activeQuestParentTransform.DOScaleY(1, showHideAnimationDuration)
				.OnComplete(() => _currentDisplayType = DisplayType.ActiveQuest);
		}

		private void ShowAllQuests()
		{
			if(_currentDisplayType == DisplayType.AllQuests)
				return;
				
			if(_currentDisplayType == DisplayType.ActiveQuest)
				HideActiveQuest();
			
			allQuestsQuestTransform.SetGameObjectActive();
			allQuestsQuestTransform.DOScaleY(1, showHideAnimationDuration)
				.OnComplete(() => _currentDisplayType = DisplayType.AllQuests);
		}

		public void TemporaryShowActiveQuest()
		{
			StopAllCoroutines();
			ShowActiveQuest();
			this.WaitAndDoCoroutine(infoDisplayTime, HideActiveQuest);
		}

		public void TemporaryShowAllQuests()
		{
			StopAllCoroutines();
			ShowAllQuests();
			this.WaitAndDoCoroutine(infoDisplayTime, HideAllQuests);
		}
		
		public void SetActiveQuestData(int questId, Bitset32 completedSteps)
		{
			string questTitle = questsConfig.GetQuestTitle(questId);
			string[] questStepDescriptions = questsConfig.GetQuestStepDescriptions(questId);

			activeQuestTitleText.SetText(questTitle);
			
			for (int i = 0; i < activeQuestStepsLabels.Length; i++)
			{
				var questStepLabel = activeQuestStepsLabels[i];
				
				if (i >= questStepDescriptions.Length)
				{
					questStepLabel.SetText("");
					questStepLabel.SetGameObjectInactive();
					
					continue;
				}
				
				questStepLabel.SetText(questStepDescriptions[i]);
				questStepLabel.SetGameObjectActive();
				
				//if(completedSteps[i])
					//Mark as completed step	
			}
		}

		public void SetAllQuestsData(int[] questIds)
		{
			if(questIds == null || questIds.Length < 1)
				return;
			
			for (int i = 0; i < allQuestTitlesLabels.Length; i++)
			{
				var titleLabel = allQuestTitlesLabels[i];

				if (i >= questIds.Length)
				{
					titleLabel.SetText("");
					titleLabel.SetGameObjectInactive();
					
					continue;
				}

				string questTitle = questsConfig.GetQuestTitle(questIds[i]);
				
				titleLabel.SetText(questTitle);
				titleLabel.SetGameObjectActive();
			}
		}
	}
}