using Ingame.Data.Hud;
using NaughtyAttributes;
using UnityEngine;
using Voody.UniLeo;
using Zenject;

namespace Ingame.Hud
{
    public sealed class HudItemModelProvider : MonoProvider<HudItemModel>
    {
        [SerializeField] private Vector3 localPositionInHud;
        [SerializeField] private Quaternion localRotationInHud;
        [Required, SerializeField] private HudItemData itemData;

        [Inject]
        private void Construct()
        {
            value = new HudItemModel
            {
                localPositionInHud = localPositionInHud,
                localRotationInHud = localRotationInHud,
                itemData = itemData
            };
        }
    }
}