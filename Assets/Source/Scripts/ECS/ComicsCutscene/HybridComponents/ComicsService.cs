using System.Collections.Generic;
using Ingame.ComicsCutscene;
using Leopotam.Ecs;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace Ingame.Comics
{
    public sealed class ComicsService
    {
        private EcsWorld _world;

        public ComicsService(EcsWorld world)
        {
            _world = world;
        }

        public void Play(string name)
        {
             var entity = _world.NewEntity();
             ref var request = ref entity.Get<PlayComicsRequest>();
             request.id = name;
        }
        public void Skip()
        {
            _world.NewEntity().Get<SkipComicsEvent>();
        }
        public void Next()
        { 
            _world.NewEntity().Get<NextPageEvent>();
        }
        
        public void Back()
        { 
            _world.NewEntity().Get<BackPageEvent>();
        }
    }

   
}