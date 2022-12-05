using System;
using UnityEngine;

namespace Ingame.Anomaly
{
    [Serializable]
    public struct OnAcidWaterEffectComponent
    {
        [HideInInspector]
        public float DamageTakenOnCooldown;

        [HideInInspector]  
        public  AcidWaterModel WaterModel;
    }
}