using Leopotam.Ecs;
using UnityEngine;
using Ingame.Movement;


namespace Ingame.Audio
{
    public sealed class AudioController
    {
        private EcsWorld _world;

        public AudioController(EcsWorld world)
        {
            _world = world;
        }

        public void Play(string type, string name, bool allowRepetition = true)
        {
            var newEntity = _world.NewEntity();
            SetUpAudio2DRequest(ref newEntity,type,name);
            newEntity.Get<AudioPlayEvent>();
            if (!allowRepetition)
            {
                newEntity.Get<AudioDisallowRepetitionTag>();
            }
        }
        
        public void Play3D(string type, string name, Transform transform, bool allowRepetition = true)
        {
            var newEntity = _world.NewEntity();
            SetUpAudio3DRequest(ref newEntity, type, name, transform);
            newEntity.Get<AudioPlayEvent>();
            if (!allowRepetition)
            {
                newEntity.Get<AudioDisallowRepetitionTag>();
            }
        }
        
        public void Stop(string type, string name)
        {
            var newEntity = _world.NewEntity();
            SetUpAudio2DRequest(ref newEntity,type,name);
            newEntity.Get<AudioStopEvent>();
        }
        
        public void Stop3D(string type, string name, Transform transform)
        {
            var newEntity = _world.NewEntity();
            SetUpAudio3DRequest(ref newEntity, type, name, transform);
            newEntity.Get<AudioStopEvent>();
        }
        public  void Pause(string type, string name)
        {
            var newEntity = _world.NewEntity();
            SetUpAudio2DRequest(ref newEntity,type,name);
            newEntity.Get<AudioPauseEvent>();   
        }
        public  void Pause3D(string type, string name, Transform transform)
        {
            var newEntity = _world.NewEntity();
            SetUpAudio3DRequest(ref newEntity, type, name, transform);
            newEntity.Get<AudioPauseEvent>(); 
        }
        
        public  void Resume(string type, string name)
        {
            var newEntity = _world.NewEntity();
            SetUpAudio2DRequest(ref newEntity,type,name);
            newEntity.Get<AudioResumeEvent>();   
        }
        public void Resume3D(string type, string name, Transform transform)
        {
            var newEntity = _world.NewEntity();
            SetUpAudio3DRequest(ref newEntity, type, name, transform);
            newEntity.Get<AudioResumeEvent>(); 
        }

        public  void StopAll()
        {
            _world.NewEntity().Get<AudioStopEvent>();
        }
        public  void PauseAll()
        {
            _world.NewEntity().Get<AudioPauseEvent>();
        }
        
        public  void ResumeAll()
        {
            _world.NewEntity().Get<AudioResumeEvent>();
        }
        public static void ChangeVolume(float f)
        {
            AudioListener.volume = f;
        }
        private static void SetUpAudio2DRequest(ref EcsEntity entity, string type, string name)
        {
            ref var audioRequest =ref entity.Get<AudioComponent>();
            audioRequest.name = name;
            audioRequest.type = type;
        }
        private static void SetUpAudio3DRequest(ref EcsEntity entity, string type, string name,Transform transform)
        {
            SetUpAudio2DRequest(ref entity, type, name);
            entity.Get<Audio3DTag>();
            ref var transformModel = ref entity.Get<TransformModel>();
            transformModel.transform = transform;
        }
    }
}