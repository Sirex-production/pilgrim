namespace Ingame.UI
{
    public struct UiBleedingCanvasGroupComponent
    {
        public float minimumAlphaDuringBleeding;
        public float maximumAlphaDuringBleeding;
        public float fadingSpeed;
        
        public bool isLerpingTowardsPositiveValue;
    }
}