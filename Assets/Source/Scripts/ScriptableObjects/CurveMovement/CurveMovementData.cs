using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Ingame.Data
{
    [Serializable]
    [CreateAssetMenu(menuName = "Ingame/CurveMovementData",fileName = "newCurveMovementData")]
    public sealed class CurveMovementData : ScriptableObject
    {
        [Header("Settings")]
        [SerializeField]
        private bool smoothMovement;
        [SerializeField]
        private bool useCharacterController;

        [SerializeField]
        [Range(0, 1)] 
        private float speedRate;

        [SerializeField] private TypeOfSmoothing typeOfSmoothing;
        
        [SerializeField]
        [ShowIf("smoothMovement")]
        private float smoothingRate;


        [SerializeField] 
        [ShowIf("IsSmoothDamp")]
        private Vector3 velocity = Vector3.zero;
        
        private bool IsSmoothDamp => typeOfSmoothing==TypeOfSmoothing.SmoothDump;
        
        public bool SmoothMovement => smoothMovement;

        public bool UseCharacterController => useCharacterController;

        public float SpeedRate => speedRate;

        public float SmoothingRate => smoothingRate;
        
        public TypeOfSmoothing TypeOfSmoothing => typeOfSmoothing ;

        public Vector3 Velocity => velocity;

    }
    public enum TypeOfSmoothing
    {
        SmoothDump,
        Lerp,
        MoveToward
    }
}