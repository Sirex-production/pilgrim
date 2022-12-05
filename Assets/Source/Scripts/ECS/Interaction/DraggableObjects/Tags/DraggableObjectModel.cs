using System;
using UnityEngine;

namespace Ingame.Interaction.DraggableObject
{
    [Serializable]
    public struct DraggableObjectModel
    {
        [Min(0)] public float dragSpeed;
    }
}