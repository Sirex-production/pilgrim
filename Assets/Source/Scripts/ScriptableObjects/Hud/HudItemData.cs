using NaughtyAttributes;
using UnityEngine;

namespace Ingame.Data.Hud
{
    [CreateAssetMenu(menuName = "Ingame/HudItemData", fileName = "newHudItemData")]
    public class HudItemData : ScriptableObject
    {
        [Foldout("Hud stats (Common)")]
        [SerializeField] private bool canBeUsedAsAim = true;

        [Foldout("Hud stats (Due to player rotation)")]
        [SerializeField] [Range(0, 10)] private float rotationSpeed = 5;
        
        [Foldout("Hud stats (Due to player rotation)"), Space]
        [SerializeField] [MinMaxSlider(-50f, 50f)] private Vector2 minMaxRotationAngleX;
        [Foldout("Hud stats (Due to player rotation)")]                                           
        [SerializeField] [Range(0, 20)] private float rotationAngleMultiplierX = 20;
        [Foldout("Hud stats (Due to player rotation)")] 
        [SerializeField] private bool inverseRotationX = false;
        
        [Foldout("Hud stats (Due to player rotation)"), Space]
        [SerializeField] [MinMaxSlider(-50f, 50f)] private Vector2 minMaxRotationAngleY;
        [Foldout("Hud stats (Due to player rotation)")]                                                      
        [SerializeField] [Range(0, 40)] private float rotationAngleMultiplierY = 20;
        [Foldout("Hud stats (Due to player rotation)")] 
        [SerializeField] private bool inverseRotationY = false;

        [Foldout("Hud stats (Due to player rotation)"), Space]
        [SerializeField] [MinMaxSlider(-50f, 50f)] private Vector2 minMaxRotationAngleZ;
        [Foldout("Hud stats (Due to player rotation)")]                                                      
        [SerializeField] [Range(0, 40)] private float rotationAngleMultiplierZ = 20; 
        [Foldout("Hud stats (Due to player rotation)")] 
        [SerializeField] private bool inverseRotationZ = false;

        [Foldout("Hud stats (Due to player rotation)"), Space]
        [SerializeField] private bool isItemMovedDueToRotation = false;
        [Foldout("Hud stats (Due to player rotation)"), ShowIf("isItemMovedDueToRotation")] 
        [SerializeField] private bool isItemMovedBackToInitialPosition = false;
        [Foldout("Hud stats (Due to player rotation)"), ShowIf("isItemMovedBackToInitialPosition")]
        [SerializeField] [Range(0, 10)] private float moveToInitialPosSpeed = 3f;
        [Foldout("Hud stats (Due to player rotation)"), ShowIf("isItemMovedDueToRotation")]
        [SerializeField] [Range(0, 10)] private float moveSpeed = 5f;
        [Foldout("Hud stats (Due to player rotation)")] 
        [SerializeField] [MinMaxSlider(-2, 2)] private Vector2 minMaxMovementOffsetY = new(0, 0);
        [Foldout("Hud stats (Due to player rotation)")] 
        [SerializeField] [MinMaxSlider(-2, 2)] private Vector2 minMaxMovementOffsetX = new(0, 0);


        [Foldout("Hud stats (Due to player rotation while aiming)"), ShowIf("canBeUsedAsAim")]
        [SerializeField] [Range(0, 10)] private float aimRotationSpeed = 5;
        
        [Foldout("Hud stats (Due to player rotation while aiming)"), ShowIf("canBeUsedAsAim"), Space]
        [SerializeField] [MinMaxSlider(-50f, 50f)] private Vector2 minMaxAimRotationAngleX;
        [Foldout("Hud stats (Due to player rotation while aiming)"), ShowIf("canBeUsedAsAim")]                                           
        [SerializeField] [Range(0, 20)] private float aimRotationAngleMultiplierX = 20;
        [Foldout("Hud stats (Due to player rotation while aiming)"), ShowIf("canBeUsedAsAim")] 
        [SerializeField] private bool inverseAimRotationX = false;
        
        [Foldout("Hud stats (Due to player rotation while aiming)"), ShowIf("canBeUsedAsAim"), Space]
        [SerializeField] [MinMaxSlider(-50f, 50f)] private Vector2 minMaxAimRotationAngleY;
        [Foldout("Hud stats (Due to player rotation while aiming)"), ShowIf("canBeUsedAsAim")]                                                      
        [SerializeField] [Range(0, 40)] private float aimRotationAngleMultiplierY = 20;
        [Foldout("Hud stats (Due to player rotation while aiming)"), ShowIf("canBeUsedAsAim")] 
        [SerializeField] private bool inverseAimRotationY = false;

        [Foldout("Hud stats (Due to player rotation while aiming)"), ShowIf("canBeUsedAsAim"), Space]
        [SerializeField] [MinMaxSlider(-50f, 50f)] private Vector2 minMaxAimRotationAngleZ;
        [Foldout("Hud stats (Due to player rotation while aiming)"), ShowIf("canBeUsedAsAim")]                                                      
        [SerializeField] [Range(0, 40)] private float aimRotationAngleMultiplierZ = 20; 
        [Foldout("Hud stats (Due to player rotation while aiming)"), ShowIf("canBeUsedAsAim")] 
        [SerializeField] private bool inverseAimRotationZ = false;

        
        
        [Foldout("Hud stats (Due to player movement)"), Space]
        [SerializeField] [MinMaxSlider(-50f, 50f)] private Vector2 minMaxRotationMovementAngleX;
        [Foldout("Hud stats (Due to player movement)")]
        [SerializeField] [Range(0, 40)] private float rotationMovementAngleMultiplierX = 20;
        [Foldout("Hud stats (Due to player movement)")]
        [SerializeField] private bool inverseRotationMovementX = false;
        
        [Foldout("Hud stats (Due to player movement)"), Space]
        [SerializeField] [MinMaxSlider(-50f, 50f)] private Vector2 minMaxRotationMovementAngleZ;
        [Foldout("Hud stats (Due to player movement)")]
        [SerializeField] [Range(0, 40)] private float rotationMovementAngleMultiplierZ = 20;
        [Foldout("Hud stats (Due to player movement)")]
        [SerializeField] private bool inverseRotationMovementZ = false;

        
        
        [Foldout("Hud stats (Due to player movement while aiming)"), ShowIf("canBeUsedAsAim"), Space]
        [SerializeField] [MinMaxSlider(-50f, 50f)] private Vector2 minMaxAimRotationMovementAngleX;
        [Foldout("Hud stats (Due to player movement while aiming)"), ShowIf("canBeUsedAsAim")]
        [SerializeField] [Range(0, 40)] private float aimRotationMovementAngleMultiplierX = 20;
        [Foldout("Hud stats (Due to player movement while aiming)"), ShowIf("canBeUsedAsAim")]
        [SerializeField] private bool inverseAimRotationMovementX = false;
        
        [Foldout("Hud stats (Due to player movement while aiming)"), ShowIf("canBeUsedAsAim"), Space]
        [SerializeField] [MinMaxSlider(-50f, 50f)] private Vector2 minMaxAimRotationMovementAngleY;
        [Foldout("Hud stats (Due to player movement while aiming)"), ShowIf("canBeUsedAsAim")]
        [SerializeField] [Range(0, 40)] private float aimRotationMovementAngleMultiplierY = 20;
        [Foldout("Hud stats (Due to player movement while aiming)"), ShowIf("canBeUsedAsAim")]
        [SerializeField] private bool inverseAimRotationMovementY = false;
        
        
        
        [Foldout("Hud stats (Instability)"), Space]
        [SerializeField] [Range(.1f, 1)] private float initialInstability = .2f;
        [Foldout("Hud stats (Instability)")]
        [SerializeField] [Range(0, 10)] private float instabilityBobbingSpeed;
        [Foldout("Hud stats (Instability)")]
        [SerializeField] [Range(0, 40)] private float instabilityAimStabilizationSpeed = 10f;
        [Foldout("Hud stats (Instability)")]
        [SerializeField] [Range(0, 10)] private float instabilityMovementOffset;
        [Foldout("Hud stats (Instability)")]
        [SerializeField] [MinMaxSlider(-2, 2)] private Vector2 minMaxInstabilityOffsetY = new(0, 0);
        [Foldout("Hud stats (Instability)")]
        [SerializeField] [MinMaxSlider(-2, 2)] private Vector2 minMaxInstabilityOffsetX = new(0, 0);

        [Foldout("Hud stats (Recoil)")]
        [SerializeField] [Range(0, 1f)] private float recoilOffsetZ = .1f;
        [Foldout("Hud stats (Recoil)")]
        [SerializeField] [Range(0, 20f)] private float recoilStabilizationSpeed = 3f;
        

        [Foldout("Hud stats (Wall clipping)"), Space] 
        [SerializeField] [Range(0, 2f)] private float maximumClippingOffset = 1.5f;
        [Foldout("Hud stats (Wall clipping)")] 
        [SerializeField] [Range(0, 2f)] private float maximumAimClippingOffset = .3f;
        [Foldout("Hud stats (Wall clipping)")]
        [SerializeField] [Range(0, 10f)] private float clippingMovementSpeed = 3f;

        public bool CanBeUsedAsAim => canBeUsedAsAim;

        public float RotationSpeed => rotationSpeed;
        
        public Vector2 MinMaxRotationAngleX => minMaxRotationAngleX;
        public float RotationAngleMultiplierX => rotationAngleMultiplierX;
        public float InverseRotationX => inverseRotationX ? -1: 1;
        
        public Vector2 MinMaxRotationAngleY => minMaxRotationAngleY;
        public float RotationAngleMultiplierY => rotationAngleMultiplierY;
        public float InverseRotationY => inverseRotationY ? -1: 1;
        
        public Vector2 MinMaxRotationAngleZ => minMaxRotationAngleZ;
        public float RotationAngleMultiplierZ => rotationAngleMultiplierZ;
        public float InverseRotationZ => inverseRotationZ ? -1: 1;

        public bool IsItemMovedDueToRotation => isItemMovedDueToRotation;
        public bool IsItemMovedBackToInitialPosition => isItemMovedBackToInitialPosition;
        public float MoveToInitialPosSpeed => moveToInitialPosSpeed;
        public float MoveSpeed => moveSpeed;
        public Vector2 MinMaxMovementOffsetY => minMaxMovementOffsetY;
        public Vector2 MinMaxMovementOffsetX => minMaxMovementOffsetX;


        public float AimRotationSpeed => aimRotationSpeed;
        
        public Vector2 MinMaxAimRotationAngleX => minMaxAimRotationAngleX;
        public float AimRotationAngleMultiplierX => aimRotationAngleMultiplierX;
        public float InverseAimRotationX => inverseAimRotationX ? -1: 1;
        
        public Vector2 MinMaxAimRotationAngleY => minMaxAimRotationAngleY;
        public float AimRotationAngleMultiplierY => aimRotationAngleMultiplierY;
        public float InverseAimRotationY => inverseAimRotationY ? -1: 1;
        
        public Vector2 MinMaxAimRotationAngleZ => minMaxAimRotationAngleZ;
        public float AimRotationAngleMultiplierZ => aimRotationAngleMultiplierZ;
        public float InverseAimRotationZ => inverseAimRotationZ ? -1: 1;
        
        
        
        public Vector2 MinMaxRotationMovementAngleX => minMaxRotationMovementAngleX;
        public float RotationMovementAngleMultiplierX => rotationMovementAngleMultiplierX;
        public float InverseRotationMovementX => inverseRotationMovementX ? -1: 1;
        
        public Vector2 MinMaxRotationMovementAngleZ => minMaxRotationMovementAngleZ;
        public float RotationMovementAngleMultiplierZ => rotationMovementAngleMultiplierZ;
        public float InverseRotationMovementZ => inverseRotationMovementZ ? -1: 1;
        
        
        public Vector2 MinMaxAimRotationMovementAngleX => minMaxAimRotationMovementAngleX;
        public float AimRotationMovementAngleMultiplierX => aimRotationMovementAngleMultiplierX;
        public float InverseAimRotationMovementX => inverseAimRotationMovementX ? -1: 1;
        
        public Vector2 MINMaxAimRotationMovementAngleY => minMaxAimRotationMovementAngleY;
        public float AimRotationMovementAngleMultiplierY => aimRotationMovementAngleMultiplierY;
        public float InverseAimRotationMovementY => inverseAimRotationMovementY ? -1: 1;


        public float InitialInstability => initialInstability;
        public float InstabilityBobbingSpeed => instabilityBobbingSpeed;
        public float InstabilityAimStabilizationSpeed => instabilityAimStabilizationSpeed;
        public float InstabilityMovementOffset => instabilityMovementOffset;
        public Vector2 MinMaxInstabilityOffsetY => minMaxInstabilityOffsetY;
        public Vector2 MinMaxInstabilityOffsetX => minMaxInstabilityOffsetX;


        public float RecoilOffsetZ => recoilOffsetZ;
        public float RecoilStabilizationSpeed => recoilStabilizationSpeed;


        public float MaximumClippingOffset => maximumClippingOffset;
        public float MaximumAimClippingOffset => maximumAimClippingOffset;
        public float ClippingMovementSpeed => clippingMovementSpeed;
    }
}