using System;
using System.Collections.Generic;
using Leopotam.Ecs;
using Leopotam.Ecs.UnityIntegration;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ingame
{
    public class EcsProfiler : IDisposable
    {
        private GameObject _escWorldObserver;
        private List<GameObject> _escSystemObservers = new();

        public EcsProfiler(EcsWorld world, IEcsWorldDebugListener worldDebugListener, params EcsSystems[] systemsCollection)
        {
            if(world == null)
                return;
            
            _escWorldObserver = EcsWorldObserver.Create(world);
            
            if(worldDebugListener != null)
                world.AddDebugListener(worldDebugListener);

            foreach (var systems in systemsCollection)
            {
                if(systems == null)
                    continue;
                
                _escSystemObservers.Add(EcsSystemsObserver.Create(systems));
            }
        }

        public void Dispose()
        {
            Object.Destroy(_escWorldObserver);
            
            foreach (var _escSystemObserver in _escSystemObservers)
            {
                if(_escSystemObserver == null)
                    continue;
                
                Object.Destroy(_escSystemObserver);
            }
        }
    }
}