using UnityEngine;

namespace Ingame.Enemy
{
    [SerializeField]
    public struct HitBoxCapsuleColliderModel
    {
        [HideInInspector]
        public CapsuleCollider capsuleCollider;
    }
}