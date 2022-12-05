using Ingame.Data.Player;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace Ingame.DI.Installers
{
    public class PlayerLegacyInstaller : MonoInstaller
    {
        [BoxGroup("Data"), Required]
        [SerializeField] private PlayerMovementData playerMovementData;
        [BoxGroup("Transforms"), Required]
        [SerializeField] private Transform hands;
        [BoxGroup("Transforms"), Required]
        [SerializeField] private Transform hudParent;
        
        public override void InstallBindings()
        {
            Container.Bind<PlayerMovementData>()
                .FromInstance(playerMovementData)
                .AsSingle();

            Container.Bind<Transform>()
                .WithId("Hands")
                .FromInstance(hands)
                .AsCached();
            
            Container.Bind<Transform>()
                .WithId("HudParent")
                .FromInstance(hudParent)
                .AsCached();
        }
    }
}