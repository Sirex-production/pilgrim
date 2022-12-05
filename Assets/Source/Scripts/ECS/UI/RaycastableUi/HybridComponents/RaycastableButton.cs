using DG.Tweening;
using Support.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Ingame.UI.Raycastable
{
    [RequireComponent(typeof(Button), typeof(Image))]
    public class RaycastableButton : MonoBehaviour
    {
        [SerializeField] private Color selectColor;
        [SerializeField] [Min(00)] private float animationDuration = .1f; 
        
        private Image _buttonImage;
        private Button _button;

        private Color _initialButtonColor;
        
        private void Awake()
        {
            _buttonImage = GetComponent<Image>();
            _button = GetComponent<Button>();
            _initialButtonColor = _buttonImage.color;
        }

        public void Press()
        {
            _button.onClick.Invoke();
        }

        public void Select()
        {
            StopAllCoroutines();

            _buttonImage.DOColor(selectColor, animationDuration);
            Deselect();
        }

        private void Deselect()
        {
            StopAllCoroutines();
            _buttonImage.DOColor(_initialButtonColor, animationDuration);
        }
    }
}