using System.Collections.Generic;
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
            
        }
        public void Skip()
        {
            
        }
        public void Next()
        {
            
        }
        private class CurrentComics
        {
            public ComicsData comicsData;
            public int page;
        }

     
    }

   
}