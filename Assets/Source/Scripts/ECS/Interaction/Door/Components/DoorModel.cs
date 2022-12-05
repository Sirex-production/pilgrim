using System;
using DG.Tweening;
using UnityEngine;

namespace Ingame.Interaction.Doors
{
    [Serializable]
    public struct DoorModel
    {
        public Vector3 rotationWhenOpened;
        public float openAnimationDuration;
        public Ease animationEase;
    }
}