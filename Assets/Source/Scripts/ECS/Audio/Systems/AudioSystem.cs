
using Ingame.Movement;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.Pool;

namespace Ingame.Audio 
{
    public sealed class AudioSystem : IEcsRunSystem, IEcsInitSystem
    {
        private readonly EcsFilter<AudioComponent,AudioPlayEvent>.Exclude<AudioSourceModel> _audioPlayRequestFilter;
        private readonly EcsFilter<AudioComponent,AudioStopEvent> _audioStopRequestFilter;
        private readonly EcsFilter<AudioComponent,AudioPauseEvent> _audioPauseRequestFilter;
        private readonly EcsFilter<AudioComponent,AudioResumeEvent> _audioResumeRequestFilter;
        
        private readonly EcsFilter<AudioComponent, AudioSourceModel,AudioIsPlayedTag> _audioIsPlayingFilter;
        private readonly EcsFilter<AudioComponent, AudioSourceModel,AudioIsPausedTag> _audioIsPausedFilter;
        private readonly EcsFilter<AudioComponent, AudioSourceModel> _audioInAnyStateFilter;
        
        private readonly EcsFilter<AudioStorageModel, TransformModel> _audioStorageFilter;
        
        private readonly EcsFilter<AudioPauseAllEvent> _audioPauseAllRequestFilter;
        private readonly EcsFilter<AudioResumeEvent> _audioResumeAllRequestFilter;
        private readonly EcsFilter<AudioStopAllEvent> _audioStopAllRequestFilter;
        
        private ObjectPool<AudioSource> _pool;
        private Transform _transform;
        public void Init()
        {
            
            _pool = new ObjectPool<AudioSource>(
                OnAudioSourceCreate,
                OnAudioSourceGet,
                OnAudioSourceRelease,
                OnAudioSourceDestroy
            );
            if (_audioStorageFilter.IsEmpty())
            {
                #if UNITY_EDITOR
                    UnityEngine.Debug.LogError("Audio System is not built properly");            
                #endif
                return;
            }
            ref var transformModel = ref _audioStorageFilter.Get2(0);
            _transform = transformModel.transform;
        }
        
        public void Run()
        {
            HandlePlayRequest();
            HandlePauseRequest();
            HandleResumeRequest();
            HandleStopRequest();
            
            ReleaseOnClipFinish();
            
            PauseAll();
            ResumeAll();
            StopAll();

        }

        private void HandlePlayRequest()
        {
            foreach (var i in _audioPlayRequestFilter)
            {
                if (_audioStorageFilter.IsEmpty())
                {
                    return;
                }
                ref var audioStorage = ref _audioStorageFilter.Get1(0);
                ref var audio = ref _audioPlayRequestFilter.Get1(i);
                ref var audioEntity = ref _audioPlayRequestFilter.GetEntity(i);

                if (audioEntity.Has<AudioDisallowRepetitionTag>())
                {
                    bool shouldRequestBeCancelled = false;
                    foreach (var k in _audioInAnyStateFilter)
                    {
                        ref var repeatedEntity = ref _audioInAnyStateFilter.GetEntity(k);
                        ref var repeatedAudioComponent = ref _audioInAnyStateFilter.Get1(k);
                        if (repeatedAudioComponent.Name != audio.Name || repeatedAudioComponent.Type != audio.Type)
                            continue;
                        if ((repeatedEntity.Has<Audio3DModel>() && audioEntity.Has<Audio3DModel>()))
                        {
                            ref var repeatedAudio3dModel = ref repeatedEntity.Get<Audio3DModel>();
                            ref var audio3dModel = ref audioEntity.Get<Audio3DModel>();
                            if (repeatedAudio3dModel.Parent == audio3dModel.Parent)
                            {
                                shouldRequestBeCancelled = true;
                                break;
                            }
                        }

                        shouldRequestBeCancelled = true;
                        break;
                    }

                    if (shouldRequestBeCancelled)
                    {
                        audioEntity.Destroy();
                        continue;
                    }
                }
                
                var audioSource = _pool.Get();
                audioSource.clip = audioStorage.Audios[audio.Type][audio.Name].Item1;
                audioSource.Play();
                
                ref var audioIsPlayingModel = ref audioEntity.Get<AudioSourceModel>();
                audioIsPlayingModel.AudioSource = audioSource;
                var audioSettings = audioStorage.Audios[audio.Type][audio.Name].Item2;
                
                audioIsPlayingModel.AudioSource.priority = audioSettings.Priority;
                audioIsPlayingModel.AudioSource.volume = audioSettings.Volume;
                audioIsPlayingModel.AudioSource.pitch = audioSettings.Pitch;
                audioIsPlayingModel.AudioSource.loop = audioSettings.Loop;
                audioIsPlayingModel.AudioSource.spatialBlend = audioSettings.SpatialBlend;
                audioIsPlayingModel.AudioSource.dopplerLevel = audioSettings.DopplerLevel;
                audioIsPlayingModel.AudioSource.spread = audioSettings.Spread;
                audioIsPlayingModel.AudioSource.minDistance = audioSettings.MinDistance;
                audioIsPlayingModel.AudioSource.maxDistance = audioSettings.MaxDistance;
                
                if (audioEntity.Has<Audio3DModel>())
                {
                    ref var audio3d = ref audioEntity.Get<Audio3DModel>();
                    audioSource.gameObject.transform.parent = audio3d.Parent;
                }
                else
                {
                    audioSource.gameObject.transform.parent =  _transform;
                }
                audioEntity.Get<AudioIsPlayedTag>();
                audioEntity.Del<AudioPlayEvent>();
            }
        }

        private void HandlePauseRequest()
        {
            foreach (var i in _audioPauseRequestFilter)
            {
                ref var audioPausedEntity = ref _audioPauseRequestFilter.GetEntity(i);
                ref var requestAudioComponent = ref _audioPauseRequestFilter.Get1(i);
                foreach (var j in _audioIsPlayingFilter)
                {
                    ref var audioEntity = ref _audioIsPlayingFilter.GetEntity(j);
                    ref var audioComponent = ref _audioIsPlayingFilter.Get1(j);
                    ref var sourceModel= ref _audioIsPlayingFilter.Get2(j);
                    if (requestAudioComponent.Name == audioComponent.Name &&
                        requestAudioComponent.Type == audioComponent.Type)
                    {
                        if (audioPausedEntity.Has<Audio3DModel>() && audioEntity.Has<Audio3DModel>())
                        {
                            ref var stopAudio3D = ref audioPausedEntity.Get<Audio3DModel>();
                            ref var audio3D = ref audioEntity.Get<Audio3DModel>();
                            if(audio3D.Parent != stopAudio3D.Parent)
                                continue;
                        }
                        
                        sourceModel.AudioSource.Pause();
                        audioEntity.Get<AudioIsPausedTag>();
                        audioEntity.Del<AudioIsPlayedTag>();
                    }
                }
                audioPausedEntity.Destroy();
            }
        }

        private void HandleResumeRequest()
        {
            foreach (var i in _audioResumeRequestFilter)
            {
                ref var audioPausedEntity = ref _audioResumeRequestFilter.GetEntity(i);
                ref var requestAudioComponent = ref _audioResumeRequestFilter.Get1(i);
                foreach (var j in _audioIsPlayingFilter)
                {
                    ref var audioEntity = ref _audioIsPlayingFilter.GetEntity(j);
                    ref var audioComponent = ref _audioIsPlayingFilter.Get1(j);
                    ref var sourceModel= ref _audioIsPlayingFilter.Get2(j);
                    if (requestAudioComponent.Name == audioComponent.Name &&
                        requestAudioComponent.Type == audioComponent.Type)
                    {
                        if (audioPausedEntity.Has<Audio3DModel>() && audioEntity.Has<Audio3DModel>())
                        {
                            ref var stopAudio3D = ref audioPausedEntity.Get<Audio3DModel>();
                            ref var audio3D = ref audioEntity.Get<Audio3DModel>();
                            if(audio3D.Parent != stopAudio3D.Parent)
                                continue;
                        }
                        
                        sourceModel.AudioSource.Play();
                        audioEntity.Get<AudioIsPlayedTag>();
                        audioEntity.Del<AudioIsPausedTag>();
                    }
                }
                audioPausedEntity.Destroy();
            }
        }
        private void HandleStopRequest()
        {
            foreach (var i in _audioStopRequestFilter)
            {
                ref var audioStopEntity = ref _audioStopRequestFilter.GetEntity(i);
                ref var requestAudioComponent = ref _audioStopRequestFilter.Get1(i);
                foreach (var j in _audioIsPlayingFilter)
                {
                    ref var audioEntity = ref _audioIsPlayingFilter.GetEntity(j);
                    ref var audioComponent = ref _audioIsPlayingFilter.Get1(j);
                    ref var sourceModel= ref _audioIsPlayingFilter.Get2(j);

                    if (requestAudioComponent.Name == audioComponent.Name && requestAudioComponent.Type == audioComponent.Type)
                    {
                        if (audioStopEntity.Has<Audio3DModel>() && audioEntity.Has<Audio3DModel>())
                        {
                            ref var stopAudio3D = ref audioStopEntity.Get<Audio3DModel>();
                            ref var audio3D = ref audioEntity.Get<Audio3DModel>();
                            if(audio3D.Parent != stopAudio3D.Parent)
                                continue;
                        }
                        sourceModel.AudioSource.Stop();
                        sourceModel.AudioSource.transform.parent = _transform;
                        sourceModel.AudioSource.clip = null;
                        _pool.Release(sourceModel.AudioSource);
                        audioEntity.Destroy();
                        break;
                    }
                }
                audioStopEntity.Destroy();
            }
        }
        private void ReleaseOnClipFinish()
        {
            foreach (var i in _audioIsPlayingFilter)
            {
                ref var entity = ref _audioIsPlayingFilter.GetEntity(i);
                ref var sourceModel = ref _audioIsPlayingFilter.Get2(i);
                
                if(sourceModel.AudioSource.isPlaying)
                    continue;
                sourceModel.AudioSource.clip = null;
                _pool.Release(sourceModel.AudioSource);
                entity.Destroy();
            }
        }

        private void StopAll()
        {
            foreach (var i in _audioPauseAllRequestFilter)
            {
                ref var eventEntity = ref _audioStopAllRequestFilter.GetEntity(i);
                foreach (var j in _audioInAnyStateFilter)
                {
                    ref var entity = ref _audioInAnyStateFilter.GetEntity(j);
                    ref var sourceModel = ref _audioInAnyStateFilter.Get2(j);
                    sourceModel.AudioSource.Stop();
                    sourceModel.AudioSource.clip = null;
                    _pool.Release(sourceModel.AudioSource);
                    entity.Destroy();
                }
                eventEntity.Destroy();
            }
        }
        private void PauseAll()
        {
            foreach (var i in _audioPauseAllRequestFilter)
            {
                ref var eventEntity = ref _audioPauseAllRequestFilter.GetEntity(i);
                foreach (var j in _audioIsPlayingFilter)
                {
                    ref var entity = ref _audioIsPlayingFilter.GetEntity(j);
                    ref var sourceModel = ref _audioIsPlayingFilter.Get2(j);
                   sourceModel.AudioSource.Pause();
                    
                   entity.Get<AudioIsPausedTag>();
                   entity.Del<AudioIsPlayedTag>();
                }
                eventEntity.Destroy();
            } 
        }
        private void ResumeAll()
        {
            foreach (var i in _audioPauseAllRequestFilter)
            {
                ref var eventEntity = ref _audioResumeAllRequestFilter.GetEntity(i);
                foreach (var j in _audioIsPausedFilter)
                {
                    ref var entity = ref _audioIsPausedFilter.GetEntity(j);
                    ref var sourceModel = ref _audioIsPausedFilter.Get2(j);
                    sourceModel.AudioSource.Play();
                    
                    entity.Get<AudioIsPlayedTag>();
                    entity.Del<AudioIsPausedTag>();
                }
                eventEntity.Destroy();
            }
        }
        private AudioSource OnAudioSourceCreate()
        {
            var go = new GameObject("Audio Source");
            var audioSource = go.AddComponent<AudioSource>();
            return audioSource;
        }

        private void OnAudioSourceGet(AudioSource source)
        {
            //NOTHING
        }

        private void OnAudioSourceRelease(AudioSource source)
        {
            source.transform.parent = _transform;
        }
		
        private void OnAudioSourceDestroy(AudioSource source)
        {
            //NOTHING
        }
        
    }
}