using TMPro;
using UnityEngine;
using Voody.UniLeo;
using Zenject;

namespace Ingame.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public sealed class TmpTextModelProvider : MonoProvider<TmpTextModel>
    {
        [Inject]
        private void Construct()
        {
            value = new TmpTextModel
            {
                tmpText = GetComponent<TMP_Text>()
            };
        }
    }
}