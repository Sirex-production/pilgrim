using UnityEngine;

namespace Ingame.Gunplay
{
    public struct FirearmComponent
    {
        public FirearmConfig firearmConfig;
        public Vector2 currentRecoilStrength;
        public Transform barrelOrigin;
    }
}