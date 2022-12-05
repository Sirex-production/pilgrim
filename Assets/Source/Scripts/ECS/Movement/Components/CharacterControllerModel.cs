using UnityEngine;

namespace Ingame.Movement
{
    public struct CharacterControllerModel
    {
        public CharacterController characterController;
        public float initialHeight;
        public float slidingForceModifier;
        public bool isStandingOnFlatSurface;
    }
}