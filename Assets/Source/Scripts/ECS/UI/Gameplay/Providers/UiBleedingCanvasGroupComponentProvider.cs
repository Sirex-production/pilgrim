using NaughtyAttributes;
using UnityEngine;
using Voody.UniLeo;
using Zenject;

namespace Ingame.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public sealed class UiBleedingCanvasGroupComponentProvider : MonoProvider<UiBleedingCanvasGroupComponent>
    {
        [SerializeField] [MinMaxSlider(0, 1)] private Vector2 minMaxAlphaValuesDuringBleeding;
        [SerializeField] [Min(0)] private float fadingSpeed = 3f;

        [Inject]
        private void Construct()
        {
            value = new UiBleedingCanvasGroupComponent
            {
                minimumAlphaDuringBleeding = minMaxAlphaValuesDuringBleeding.x,
                maximumAlphaDuringBleeding = minMaxAlphaValuesDuringBleeding.y,
                fadingSpeed = this.fadingSpeed
            };
        }
    }
}