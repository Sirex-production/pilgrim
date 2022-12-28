using Ingame.Audio;
using Ingame.Comics;
using Ingame.SaveLoad;
using Leopotam.Ecs;
using UnityEngine;
using Zenject;

namespace Ingame.DI.Installers
{
    public class EcsInstaller : MonoInstaller
    { 
     
        public override void InstallBindings()
        {
            var world = new EcsWorld();
            var updateSystems = new EcsSystems(world);
            var fixedUpdateSystems = new EcsSystems(world);
            var saveLoadService = new SaveLoadService();
            
            Container.Bind<EcsWorld>()
                .FromInstance(world)
                .AsSingle()
                .NonLazy();
            
            Container.BindInterfacesAndSelfTo<SaveLoadService>()
                .FromInstance(saveLoadService)
                .AsCached()
                .NonLazy();
            
            Container.Bind<EcsSystems>()
                .WithId("UpdateSystems")
                .FromInstance(updateSystems)
                .AsCached()
                .NonLazy();
            
            Container.Bind<EcsSystems>()
                .WithId("FixedUpdateSystems")
                .FromInstance(fixedUpdateSystems)
                .AsCached()
                .NonLazy();
        }
    }
}