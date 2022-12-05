using Ingame.Data.Player;
using Voody.UniLeo;
using Zenject;

namespace Ingame.Player
{
    public sealed class PlayerModelProvider : MonoProvider<PlayerModel>
    {
        [Inject]
        private void Construct(PlayerMovementData injectedPlayerMovementData, PlayerHudData playerHudData, PlayerInventoryData playerInventoryData)
        {
            value = new PlayerModel
            {
                playerMovementData = injectedPlayerMovementData,
                playerHudData = playerHudData,
                playerInventoryData = playerInventoryData
            };
        }
    }
}