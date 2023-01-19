using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace Ingame.Comics
{
    public sealed class ComicsInstaller : MonoInstaller
    {
        [SerializeField] 
        [Required]
        private ComicsService comicsService;
        
        public override void InstallBindings()
        {
            Container.Bind<ComicsService>()
                .FromInstance(comicsService)
                .AsSingle();
        }
    }
}