using NaughtyAttributes;
using UnityEngine;

namespace Ingame.Data.Player
{
    [CreateAssetMenu(menuName = "Ingame/PlayerData", fileName = "Ingame/NewPlayerData")]
    public class PlayerMovementData : ScriptableObject
    {
        [BoxGroup("Movement")]
        [SerializeField][Min(0)] private float walkSpeed = 10;
        [BoxGroup("Movement")]
        [SerializeField][Min(0)] private float crouchWalkSpeed = 3;
        [BoxGroup("Movement")]
        [SerializeField][Min(0)] private float leanWalkSpeed = 4;
        [BoxGroup("Movement")]
        [SerializeField][Min(0)] private float enterCrouchStateSpeed = .5f;
        [BoxGroup("Movement")]
        [SerializeField][Min(0)] private float movementAcceleration = 100;
        [BoxGroup("Movement")]
        [SerializeField][Min(0)] private float movementFriction = 10;
        
        [BoxGroup("Movement (Jumping)"), Space]
        [SerializeField][Min(0)] private float jumpForce = 5;
        [BoxGroup("Movement (Jumping)")]
        [SerializeField][Min(0)] private float pauseBetweenJumps = .5f;
        
        [BoxGroup("Movement (Lean)"), Space]
        [SerializeField] [Range(0, 20)] private float enterLeanSpeed = 5f;
        [BoxGroup("Movement (Lean)")]
        [SerializeField] [Range(0, 1)] private float leanDistanceOffset = .3f;
        [BoxGroup("Movement (Lean)")]
        [SerializeField] [Range(0, 120)] private float leanAngleOffset = 100f;
        [BoxGroup("Movement (Lean)")]
        [SerializeField] [Range(-2, 2)] private float cameraPositionOffsetDuringTheRightLean = .3f;
        [BoxGroup("Movement (Lean)")]
        [SerializeField] [Range(-2, 2)] private float cameraPositionOffsetDuringTheLeftLean = .3f;
        
        [BoxGroup("Gravitation"), Space]
        [SerializeField][Min(0)] private float gravityAcceleration = 1;
        [BoxGroup("Gravitation")]
        [SerializeField][Min(0)] private float maximumGravitationForce = 10;
        [BoxGroup("Gravitation")]
        [SerializeField] [Range(0, 10)] private float slidingForceModifier = 5f;
        
        //todo move to the controls settings
        [BoxGroup("Controls"), Space] 
        [SerializeField] [Min(0)] private float sensitivity = 1;

        [BoxGroup("Interaction"), Space] 
        [SerializeField] [Range(0, 20)] private float interactionDistance = .5f;
        
        [BoxGroup("Interaction"), Space]
        [SerializeField] [Range(0, 3f)] private float draggableObjectDistance = 1f;

        public float WalkSpeed => walkSpeed;
        public float CrouchWalkSpeed => crouchWalkSpeed;
        public float LeanWalkSpeed => leanWalkSpeed;
        public float EnterCrouchStateSpeed => enterCrouchStateSpeed;
        public float MovementAcceleration => movementAcceleration;
        public float MovementFriction => movementFriction;
        
        public float JumpForce => jumpForce;
        public float PauseBetweenJumps => pauseBetweenJumps;

        public float EnterLeanSpeed => enterLeanSpeed;
        public float LeanDistanceOffset => leanDistanceOffset;
        public float LeanAngleOffset => leanAngleOffset;
        public float CameraPositionOffsetDuringTheRightLean => cameraPositionOffsetDuringTheRightLean;
        public float CameraPositionOffsetDuringTheLeftLean => cameraPositionOffsetDuringTheLeftLean;

        public float GravityAcceleration => gravityAcceleration;
        public float MaximumGravitationForce => maximumGravitationForce;
        public float SlidingForceModifier => slidingForceModifier;

        public float Sensitivity => sensitivity;

        public float InteractionDistance => interactionDistance;

        public float DraggableObjectDistance => draggableObjectDistance;
    }
}