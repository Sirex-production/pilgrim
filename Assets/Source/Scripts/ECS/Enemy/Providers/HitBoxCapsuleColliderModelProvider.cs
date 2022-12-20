using System;
using Ingame.Movement;
using UnityEngine;
using Voody.UniLeo;
using Zenject;

namespace Ingame.Enemy
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class HitBoxCapsuleColliderModelProvider : MonoProvider<HitBoxCapsuleColliderModel>
    {
        [Inject]
        private void Construct()
        {
            value = new HitBoxCapsuleColliderModel
            {
                capsuleCollider = GetComponent<CapsuleCollider>()
            };
        }
    }
}