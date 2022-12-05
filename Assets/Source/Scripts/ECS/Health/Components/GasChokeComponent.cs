namespace Ingame.Health
{
    internal struct GasChokeComponent
    {
        /// <summary>
        /// Value should be between 0 and 1 representing percentage of gas in lungs
        /// </summary>
        public float gasAmountInLungs;
        /// <summary>
        /// This value multiples with gasAmountInLungs to define final damage
        /// </summary>
        public float gasDamagePerSecond;
        /// <summary>
        /// Value should be between 0 and 1 representing percentage of gas released from lungs per second
        /// </summary>
        public float gasReleasedFromLungsPerSecond;
        public float timePassedFromLastGasReleaseFromLungs;
    }
}