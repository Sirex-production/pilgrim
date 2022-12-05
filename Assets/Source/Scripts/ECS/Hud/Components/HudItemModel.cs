using Ingame.Data.Hud;
using UnityEngine;

namespace Ingame.Hud
{
    public struct HudItemModel
    {
        public Vector3 localPositionInHud;
        public Quaternion localRotationInHud;
        public HudItemData itemData;
    }
}