using NaughtyAttributes;
using UnityEngine;

namespace Ingame.Data.Player
{
    [CreateAssetMenu(menuName = "Ingame/PlayerHudData", fileName = "Ingame/NewPlayerHudData")]
    public class PlayerHudData : ScriptableObject
    {
        [BoxGroup("Head Bobbing")]
        [SerializeField, Range(0, 10f)] private float headBobbingLerpingSpeed = 5f;
        [BoxGroup("Head Bobbing")]
        [SerializeField, Range(0, 10f)] private float headBobbingSpeedModifier = .4f;
        [BoxGroup("Head Bobbing")]
        [SerializeField, Range(0, 0.1f)] private float headBobbingStrengthX = .01f;
        [BoxGroup("Head Bobbing")]
        [SerializeField, Range(0, 0.1f)] private float headBobbingStrengthY = .01f; 
        [BoxGroup("Head Bobbing")]
        [SerializeField, Range(0, 0.1f)] private float headBobbingStrengthZ = .01f;
        
        [BoxGroup("Hud bobbing"), Space]
        [SerializeField, Range(0, 10f)] private float hudBobbingLerpingSpeed = 5f;
        [BoxGroup("Hud bobbing")]
        [SerializeField, Range(0, 20f)] private float hudBobbingSpeedModifier = 3f;
        [BoxGroup("Hud bobbing")]
        [SerializeField, Range(0, .1f)] private float hudBobbingPositionStrengthY = .1f;
        [BoxGroup("Hud bobbing")]
        [SerializeField, Range(0, .1f)] private float hudBobbingRotationStrengthZ = .1f;
        
        [BoxGroup("Recoil")]
        [SerializeField, Range(0, 10f)] private float recoilStabilizationSpeed = 3f;

        public float HeadBobbingLerpingSpeed => headBobbingLerpingSpeed;
        public float HeadBobbingStrengthX => headBobbingStrengthX;
        public float HeadBobbingStrengthY => headBobbingStrengthY;
        public float HeadBobbingStrengthZ => headBobbingStrengthZ;
        public float HeadBobbingSpeedModifier => headBobbingSpeedModifier;

        public float HUDBobbingLerpingSpeed => hudBobbingLerpingSpeed;
        public float HUDBobbingSpeedModifier => hudBobbingSpeedModifier;
        public float HUDBobbingPositionStrengthY => hudBobbingPositionStrengthY;
        public float HUDBobbingRotationStrengthZ => hudBobbingRotationStrengthZ;

        public float RecoilStabilizationSpeed => recoilStabilizationSpeed;
    }
}   