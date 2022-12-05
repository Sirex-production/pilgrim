using Ingame.Data.Player;
using Ingame.Input;

namespace Ingame.Player
{
    public struct PlayerModel
    {
        public PlayerMovementData playerMovementData;
        public PlayerHudData playerHudData;
        public PlayerInventoryData playerInventoryData;

        public float currentSpeed;
        public bool isCrouching;
        public LeanDirection currentLeanDirection;
    }
}