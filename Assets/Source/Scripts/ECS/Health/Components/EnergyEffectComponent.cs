namespace Ingame.Health
{
    internal struct EnergyEffectComponent
    {
        public const int NUMBER_OF_ENERGY_EFFECTS_TO_DIE = 5;
        public const float DEFAULT_EFFECT_DURATION = 20f;
        public const float DEFAULT_MOVING_SPEED_SCALE = 1.2f;

        public int numberOfEffects;
        public float duration;
        public float movingSpeedScale;
    }
}