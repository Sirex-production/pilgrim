using System;
using Ingame.Data.Interaction.Explosive;
using NaughtyAttributes;
using UnityEngine;

namespace Ingame.Interaction.Explosive
{
    [Serializable]
    public struct HandGrenadeModel
    {
        [Expandable]
        public HandGrenadeData GrenadeData;
        [HideInInspector]
        public float TimeLeftToExplode;
        //todo
        //Add animations etc
    }
}