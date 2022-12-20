using System;
using NaughtyAttributes;
using UnityEngine;

namespace Ingame.Enemy
{
    [Serializable]
    public struct EnemyWeaponHolderModel
    {
        [Required]
        public Transform weapon;
    }
}