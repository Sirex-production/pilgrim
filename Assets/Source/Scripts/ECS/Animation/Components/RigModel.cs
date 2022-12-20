using System;
using NaughtyAttributes;
using UnityEngine.Animations.Rigging;

namespace Ingame.Animation
{
    [Serializable]
    public struct RigModel
    {
        [Required]
        public Rig rig;
    }
}