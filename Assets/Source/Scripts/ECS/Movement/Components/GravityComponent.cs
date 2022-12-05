using System;

namespace Ingame.Movement
{
    [Serializable]
    public struct GravityComponent
    {
        public float gravityAcceleration;
        public float maximalGravitationalForce;
    }
}