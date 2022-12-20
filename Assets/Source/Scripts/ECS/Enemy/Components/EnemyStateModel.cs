using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ingame.Enemy
{
    [Serializable]
    public struct EnemyStateModel
    {
        [Header("Hp")]
        public bool isDying;
        public bool isDead;
        public bool isTakingDamage;
        public float lastRememberedHP;
        public float currentRememberedHP;
        
        [Header("Attack")] 
        public bool isAttacking;
        public bool isReloading;
        public int maxAmmo;
        public int currentAmmo;
        
        [Header("Detection")]
        public bool isTargetDetected;
        public Transform target;
        public int visibleTargetPixels;
        public int totalTargetPixels;
        public bool shouldSearchForTarget;
        public bool hasDetectedNoises;
        public bool hasLostTarget;
        public Vector3? noisePosition;
        public List<Vector3> lastRememberedNoises;

        [Header("Movement")] 
        public bool isCrouching;
        
        [Header("Covers")]
        public static HashSet<Transform> OccupiedUndefinedCovers;
        public static HashSet<Transform> OccupiedCovers;
        
        public HashSet<Transform> covers;
        public HashSet<Transform> undefinedCovers;
        
        public HashSet<Transform> transparentCovers;
        public HashSet<Transform> undefinedTransparentCovers;
        public Vector3 cover;

    }
}