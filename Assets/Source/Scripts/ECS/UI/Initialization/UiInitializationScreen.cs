using System;
using DG.Tweening;
using NaughtyAttributes;
using Support;
using Support.Extensions;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Ingame.UI.Initialization
{
	public sealed class UiInitializationScreen : MonoBehaviour
	{
		[BoxGroup("Developer options")]
		[SerializeField] private bool isIntroAnimationSkipped = false;

		[BoxGroup("References")]
		[Required, SerializeField] private Button loginButton;
		[BoxGroup("References")]
		[Required, SerializeField] private CanvasGroup loginButtonCanvasGroup;
		[BoxGroup("References")]
		[Required, SerializeField] private CanvasGroup loginIconCanvasGroup;
		[BoxGroup("References")]
		[Required, SerializeField] private CanvasGroup initializationTextCanvasGroup;
		[BoxGroup("References")]
		[Required, SerializeField] private DOTweenAnimation textAnimation;
		
		[BoxGroup("Animation properties")]
		[SerializeField] [Min(0)] private float loginAnimationDuration;
		[BoxGroup("Animation properties")]
		[SerializeField] [Min(0)] private float delayBeforeShowingLoginAfterInitializationAnimation = 1f;
		[BoxGroup("Animation properties")]
		[SerializeField] private int initialLoginElementsYOffset = -50;
		
		[BoxGroup("Loading properties")]
		[SerializeField] [Scene] private int loadSceneIndex;
		[BoxGroup("Loading properties")]
		[SerializeField] [ResizableTextArea] private string initializationText;

		private LevelManagementService _levelManagementService;
		
		private Vector3 _initialLoginIconLocalPos;
		private Vector3 _initialLoginButtonLocalPos;
		
		[Inject]
		private void Construct(LevelManagementService levelManagementService)
		{
			_levelManagementService = levelManagementService;
		}

		private void Awake()
		{
			loginButton.onClick.AddListener(OnLoginButtonClicked);
			textAnimation.onComplete.AddListener(OnTextInitializationAnimationFinished);
			textAnimation.endValueString = initializationText;

			_initialLoginButtonLocalPos = loginButtonCanvasGroup.transform.localPosition;
			_initialLoginIconLocalPos = loginIconCanvasGroup.transform.localPosition;
			
			loginButtonCanvasGroup.alpha = 0f;
			loginIconCanvasGroup.alpha = 0f;
			initializationTextCanvasGroup.alpha = 1f;

			loginButtonCanvasGroup.transform.localPosition += Vector3.down * initialLoginElementsYOffset;
			loginIconCanvasGroup.transform.localPosition += Vector3.down * initialLoginElementsYOffset;
		}
		
		private void Start()
		{
			if (isIntroAnimationSkipped)
				DisplayLoginView();
			else
				PlayIntroAnimation();
		}

		private void OnDestroy()
		{
			loginButton.onClick.RemoveListener(OnLoginButtonClicked);
			textAnimation.onComplete.RemoveListener(OnTextInitializationAnimationFinished);
		}

		private void PlayIntroAnimation()
		{
			textAnimation.CreateTween();
			textAnimation.DOPlay();
		}

		private void DisplayLoginView()
		{
			loginIconCanvasGroup.SetGameObjectActive();
			loginButtonCanvasGroup.SetGameObjectActive();
			
			loginIconCanvasGroup.alpha = 1f;
			loginButtonCanvasGroup.alpha = 1f;
			
			loginButtonCanvasGroup.transform.localPosition = _initialLoginButtonLocalPos;
			loginIconCanvasGroup.transform.localPosition = _initialLoginIconLocalPos;
		}

		private void OnLoginButtonClicked()
		{
			_levelManagementService.LoadLevel(loadSceneIndex);
		}

		private void OnTextInitializationAnimationFinished()
		{
			DOTween.Sequence()
				.SetDelay(delayBeforeShowingLoginAfterInitializationAnimation)
				.Append(initializationTextCanvasGroup.DOFade(0, loginAnimationDuration).SetEase(Ease.InOutBounce))
				.Append(loginIconCanvasGroup.DOFade(1, loginAnimationDuration))
				.Join(loginIconCanvasGroup.transform.DOLocalMoveY(_initialLoginIconLocalPos.y, loginAnimationDuration))
				.Append(loginButtonCanvasGroup.DOFade(1, loginAnimationDuration))
				.Join(loginButtonCanvasGroup.transform.DOLocalMoveY(_initialLoginButtonLocalPos.y, loginAnimationDuration));
		}
	}
}