using Ingame.Data.Player;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace Ingame.DI.Installers
{
    public class PlayerInstaller : MonoInstaller
    {
        [Required, Expandable]
        [SerializeField] private PlayerMovementData playerMovementData;
        [Required, Expandable]
        [SerializeField] private PlayerHudData playerHudData;
        [Required, Expandable]
        [SerializeField] private PlayerInventoryData playerInventoryData;
        
        public override void InstallBindings()
        {
            Container
                .Bind<PlayerMovementData>()
                .FromInstance(playerMovementData)
                .AsSingle()
                .NonLazy();
            
            Container
                .Bind<PlayerHudData>()
                .FromInstance(playerHudData)
                .AsSingle()
                .NonLazy();

            Container
                .Bind<PlayerInventoryData>()
                .FromInstance(playerInventoryData)
                .AsSingle()
                .NonLazy();
        }
    }
}