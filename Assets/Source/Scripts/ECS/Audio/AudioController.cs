using Ingame.Enemy;
using Leopotam.Ecs;
using UnityEngine;
using Zenject;

namespace Ingame.Audio
{
    public static class AudioController
    {
        public static EcsWorld World;
        
        public static void Play(string type, string name, bool allowRepetition = true)
        {
            var newEntity = World.NewEntity();
            SetUpAudio2DRequest(ref newEntity,type,name);
            newEntity.Get<AudioPlayEvent>();
            if (!allowRepetition)
            {
                newEntity.Get<AudioDisallowRepetitionTag>();
            }
        }
        
        public static void Play3D(string type, string name, Transform transform, bool allowRepetition = true)
        {
            var newEntity = World.NewEntity();
            SetUpAudio3DRequest(ref newEntity, type, name, transform);
            newEntity.Get<AudioPlayEvent>();
            if (!allowRepetition)
            {
                newEntity.Get<AudioDisallowRepetitionTag>();
            }
        }
        
        public static void Stop(string type, string name)
        {
            var newEntity = World.NewEntity();
            SetUpAudio2DRequest(ref newEntity,type,name);
            newEntity.Get<AudioStopEvent>();
        }
        
        public static void Stop3D(string type, string name, Transform transform)
        {
            var newEntity = World.NewEntity();
            SetUpAudio3DRequest(ref newEntity, type, name, transform);
            newEntity.Get<AudioStopEvent>();
        }
        public static void Pause(string type, string name)
        {
            var newEntity = World.NewEntity();
            SetUpAudio2DRequest(ref newEntity,type,name);
            newEntity.Get<AudioPauseEvent>();   
        }
        public static void Pause3D(string type, string name, Transform transform)
        {
            var newEntity = World.NewEntity();
            SetUpAudio3DRequest(ref newEntity, type, name, transform);
            newEntity.Get<AudioPauseEvent>(); 
        }
        
        public static void Resume(string type, string name)
        {
            var newEntity = World.NewEntity();
            SetUpAudio2DRequest(ref newEntity,type,name);
            newEntity.Get<AudioResumeEvent>();   
        }
        public static void Resume3D(string type, string name, Transform transform)
        {
            var newEntity = World.NewEntity();
            SetUpAudio3DRequest(ref newEntity, type, name, transform);
            newEntity.Get<AudioResumeEvent>(); 
        }

        public static void StopAll()
        {
            World.NewEntity().Get<AudioStopEvent>();
        }
        public static void PauseAll()
        {
            World.NewEntity().Get<AudioPauseEvent>();
        }
        
        public static void ResumeAll()
        {
            World.NewEntity().Get<AudioResumeEvent>();
        }
        public static void ChangeVolume(float f)
        {
            AudioListener.volume = f;
        }
        private static void SetUpAudio2DRequest(ref EcsEntity entity, string type, string name)
        {
            ref var audioRequest =ref entity.Get<AudioComponent>();
            audioRequest.Name = name;
            audioRequest.Type = type;
        }
        private static void SetUpAudio3DRequest(ref EcsEntity entity, string type, string name,Transform transform)
        {
            SetUpAudio2DRequest(ref entity, type, name);
            ref var audio3D = ref entity.Get<Audio3DModel>();
            audio3D.Parent = transform;
        }
    }
}