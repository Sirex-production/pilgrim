using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame.Enemy
{
    [Serializable]
    public struct EnemyStateModel
    {
        //hp
        [Header("Hp")]
        public bool IsDying;
        public bool IsDead;
        public bool IsTakingDamage;
        public float LastRememberedHP;
        public float CurrentRememberedHP;
        
        //attack
        [Header("Ammo")] 
        public int MaxAmmo;
        public int CurrentAmmo;
        
        //detection of player
        [Header("Detection")]
        public bool IsTargetDetected;
        public Transform Target;

        public int VisibleTagretPixels;
        //special detections
        public bool ShouldSearchForTarget;
        public bool HasDetectedNoises;
        public bool HasLostTarget;
        
        public Vector3? NoisePosition;
        public List<Vector3> LastRememberedNoises;
        
        //public Vector3 Location;
        
        //Covers
        public HashSet<Transform> Covers;
        public HashSet<Transform> UndefinedCovers;

        //Transparent Covers
        public HashSet<Transform> TransparentCovers;
        public HashSet<Transform> UndefinedTransparentCovers;
        
        //Occupied Covers
        public static HashSet<Transform> OccupiedUndefinedCovers;
        public static HashSet<Transform> OccupiedCovers;
        
        public Vector3 Cover;

    }
}