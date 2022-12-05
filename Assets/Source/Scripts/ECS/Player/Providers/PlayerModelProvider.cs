using Ingame.Data.Player;
using NaughtyAttributes;
using UnityEngine;
using Voody.UniLeo;
using Zenject;

namespace Ingame.Player
{
    public sealed class PlayerModelProvider : MonoProvider<PlayerModel>
    {
        [Required, SerializeField] private PlayerMovementData playerMovementData;
        [Required, SerializeField] private PlayerHudData playerHudData;
        [Required, SerializeField] private PlayerInventoryData playerInventoryData;
        
        [Inject]
        private void Construct()
        {
            value = new PlayerModel
            {
                playerMovementData = playerMovementData,
                playerHudData = playerHudData,
                playerInventoryData = playerInventoryData
            };
        }
    }
}