using UnityEngine;
using Voody.UniLeo;
using Zenject;

namespace Ingame.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public sealed class CanvasGroupModelProvider : MonoProvider<CanvasGroupModel>
    {
        [Inject]
        private void Construct()
        {
            value = new CanvasGroupModel
            {
                canvasGroup = GetComponent<CanvasGroup>()
            };
        }
    }
}