using System;
using UnityEngine;

namespace Ingame.Enemy
{
    [Serializable]
    public struct SharedCameraModel
    {
         public LayerMask MaskForEnvironment;
         public LayerMask MaskForEnvironmentWithPlayer;
         public LayerMask MaskForPlayer;
    }
}