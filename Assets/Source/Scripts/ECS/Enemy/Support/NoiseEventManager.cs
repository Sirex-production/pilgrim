using Ingame.Movement;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Enemy
{
    public static class NoiseEventManager
    {
        public static void CreateNoiseEvent(this EcsWorld w,Vector3 p)
        {
           var noiseEntity =  w.NewEntity();
           noiseEntity.Get<NoiseTagRequest>().Position = p;
        }
    }
}