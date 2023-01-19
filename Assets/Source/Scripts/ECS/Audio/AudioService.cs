using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace Ingame.Audio
{

    public interface IAudio2DService
    {
        public void Play(string type, string name, bool allowRepetition = false);
        public void Stop(string type, string name);
        public void Pause(string type, string name);
        public void Resume(string type, string name);
    }

    public interface IAudio3DService
    {
        public void Play3D(string type, string name, Transform transform, bool allowRepetition = true); 
        public void Stop3D(string type, string name, Transform transform);
        public void Pause3D(string type, string name, Transform transform);
        public void Resume3D(string type, string name, Transform transform);
    }

    public interface IAudioMixerController 
    {
        public void StopAll();
        public void PauseAll();
        public void ResumeAll();
        
        public static void ChangeVolume(float f)
        {
            AudioListener.volume = f;
        }
    }


    public sealed class AudioService : MonoBehaviour, IAudioMixerController, IAudio3DService,IAudio2DService
    {
        private struct RuntimeAudioUnit  
        {
            private string _audioTypeName;
            private string _audioClipName;

            public RuntimeAudioUnit(string audioTypeName, string audioClipName)
            {
                _audioTypeName = audioTypeName;
                _audioClipName = audioClipName;
            }

            public string AudioTypeName => _audioTypeName;
            public string AudioClipName => _audioClipName;
        }
        
        [SerializeField] 
        private AudioConfig audioConfig;

        private Dictionary<string, Dictionary<string, RuntimeAudioData>> _storedAudios;
        private ObjectPool<AudioSource> _pool;
        
        private Dictionary<(string audioType, string audioName, GameObject container), Queue<AudioSource>> _played3dAudioSources = new();
        private Dictionary<(string audioType, string audioName), Queue<AudioSource>> _played2dAudioSources  = new();
        
        private Dictionary<(string audioType, string audioName, GameObject container), Queue<AudioSource>> _paused3dAudioSources = new();
        private Dictionary<(string audioType, string audioName), Queue<AudioSource>> _paused2dAudioSources = new();
        
        public void Awake()
        {
            Dictionary<string, Dictionary<string, RuntimeAudioData>> dict = new ();
            
            foreach (var typeWrapper in audioConfig.Audios)
            {
                Dictionary<string, RuntimeAudioData> dic = typeWrapper.AudioWrappers.ToDictionary(e=> e.Name,
                    e=> new RuntimeAudioData( e.AudioClip, e.AudioSettings));
                
                dict.Add(typeWrapper.AudioTypeName,dic);
            }
            _storedAudios = dict;
            
            _pool = new ObjectPool<AudioSource>(
                OnAudioSourceCreate,
                OnAudioSourceGet,
                OnAudioSourceRelease,
                OnAudioSourceDestroy
            );
        }
        
        private void Update()
        {
            RemoveStoppedAudioSources(ref _played3dAudioSources);
            RemoveStoppedAudioSources(ref _played2dAudioSources);
        }

        private void RemoveStoppedAudioSources<T>(ref Dictionary<T, Queue<AudioSource>> audioSources)
        {
            if(audioSources.Count<=0)
                return;
            
            List<T> ids = audioSources.Keys.ToList();
            foreach (var idKey in ids)
            {
                Queue<AudioSource> newAudios = new();
                foreach (var audio in audioSources[idKey])
                {
                    if (audio.isPlaying)
                    {
                        newAudios.Enqueue(audio);
                        continue;
                    }
                    ReleaseOnClipFinish(audio);
                }

                audioSources[idKey] = newAudios;
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
            source.transform.parent = this.transform;
        }
		
        private void OnAudioSourceDestroy(AudioSource source)
        {
            //NOTHING
        }

        private void ReleaseOnClipFinish(AudioSource source)
        {
            source.Stop();
            source.clip = null;
            source.transform.parent = this.transform;
            
            _pool.Release(source);
        }
        
        private AudioSource GetAndSetupAudioSource(string type, string name,Transform parent)
        {
            var audioOption = _storedAudios[type][name];
            var audioSettings = audioOption.AudioSettings;

            var audio = _pool.Get();
            audio.clip = audioOption.AudioClip;
            audio.priority = audioSettings.Priority;
            audio.volume = audioSettings.Volume;
            audio.pitch = audioSettings.Pitch;
            audio.loop = audioSettings.Loop;
            audio.spatialBlend = audioSettings.SpatialBlend;
            audio.dopplerLevel = audioSettings.DopplerLevel;
            audio.spread = audioSettings.Spread;
            audio.minDistance = audioSettings.MinDistance;
            audio.maxDistance = audioSettings.MaxDistance;
            audio.transform.parent = parent;
            audio.transform.localPosition = Vector3.zero;
            
            return audio;
        }
        
        public void Play3D(string type, string name, Transform parent, bool allowRepetition = true)
        {
            
            if (!allowRepetition 
                && _played3dAudioSources.ContainsKey((type,name, parent.gameObject)) 
                && _played3dAudioSources[(type,name, parent.gameObject)].Count >0)
                return;
            
            var audio = GetAndSetupAudioSource(type,name,parent);
            audio.rolloffMode = AudioRolloffMode.Linear;
            audio.Play();

            if (!_played3dAudioSources.ContainsKey((type, name, parent.gameObject)))
            {
                _played3dAudioSources.Add((type, name, parent.gameObject),new Queue<AudioSource>()); 
            }
            
            _played3dAudioSources[(type,name,parent.gameObject)].Enqueue(audio);
        }

        public void Stop3D(string type, string name, Transform parent)
        {
            if (!_played3dAudioSources.ContainsKey((type,name, parent.gameObject)) 
                || _played3dAudioSources[(type,name, parent.gameObject)].Count<=0)
                return;
            
            var audio = _played3dAudioSources[(type, name, parent.gameObject)].Dequeue();
            _pool.Release(audio);
        }

        public void Pause3D(string type, string name, Transform parent)
        {
            if (!_played3dAudioSources.ContainsKey((type,name, parent.gameObject))
                || _played3dAudioSources[(type,name, parent.gameObject)].Count<=0)
                return;
            
            var audio = _played3dAudioSources[(type, name, parent.gameObject)].Dequeue();
            audio.Pause();

            if (!_paused3dAudioSources.ContainsKey((type, name, parent.gameObject)))
            {
                _paused3dAudioSources.Add((type, name, parent.gameObject), new Queue<AudioSource>());
            }
            
            _paused3dAudioSources[(type, name, parent.gameObject)].Enqueue(audio);

        }

        public void Resume3D(string type, string name, Transform parent)
        {
            if(!_paused3dAudioSources.ContainsKey((type,name, parent.gameObject))
               || _paused3dAudioSources[(type,name, parent.gameObject)].Count<=0)
                return;
            
            var audio = _paused3dAudioSources[(type, name, parent.gameObject)].Dequeue();
            audio.Play();

            _played3dAudioSources[(type, name, parent.gameObject)].Enqueue(audio);
        }
        
        public void Play(string type, string name, bool allowRepetition = false)
        {
            
            if (!allowRepetition 
                && _played2dAudioSources.ContainsKey((type,name)) 
                && _played2dAudioSources[(type,name)].Count >0)
                return;
            
            var audio = GetAndSetupAudioSource(type,name, this.transform);
            audio.Play();

            if (!_played2dAudioSources.ContainsKey((type, name)))
            {
                _played2dAudioSources.Add((type, name),new Queue<AudioSource>()); 
            }
            
            _played2dAudioSources[(type,name)].Enqueue(audio);
        }

        public void Stop(string type, string name)
        {
            if (!_played2dAudioSources.ContainsKey((type,name)) 
                || _played2dAudioSources[(type,name)].Count<=0)
                return;
            
            var audio = _played2dAudioSources[(type, name)].Dequeue();
            _pool.Release(audio);
        }

        public void Pause(string type, string name)
        {
            if (!_played2dAudioSources.ContainsKey((type,name))
                || _played2dAudioSources[(type,name)].Count<=0)
                return;
            
            var audio = _played2dAudioSources[(type, name)].Dequeue();
            audio.Pause();

            if (!_paused2dAudioSources.ContainsKey((type, name)))
            {
                _paused2dAudioSources.Add((type, name), new Queue<AudioSource>());
            }
            
            _paused2dAudioSources[(type, name)].Enqueue(audio);
        }

        public void Resume(string type, string name)
        {
            if(!_paused2dAudioSources.ContainsKey((type,name))
               || _paused2dAudioSources[(type,name)].Count<=0)
                
                return;
            
            var audio = _paused2dAudioSources[(type, name)].Dequeue();
            audio.Play();

            _played2dAudioSources[(type, name)].Enqueue(audio);
        }
        
   
        public void StopAll()
        {
            var keys3d = _paused3dAudioSources.Keys;
            foreach (var key in keys3d)
            {
                Stop3D(key.Item1,key.Item2, key.Item3.transform);
            }
            
            var keys2d = _paused2dAudioSources.Keys;
            foreach (var key in keys2d)
            {
                Stop(key.Item1,key.Item2);
            }
            
            keys3d = _played3dAudioSources.Keys;
            foreach (var key in keys3d)
            {
                Stop3D(key.Item1,key.Item2, key.Item3.transform);
            }
            
            keys2d = _played2dAudioSources.Keys;
            foreach (var key in keys2d)
            {
                Stop(key.Item1,key.Item2);
            }
        }
        public void PauseAll()
        {
            var keys3d = _played3dAudioSources.Keys;
            foreach (var key in keys3d)
            {
                Pause3D(key.Item1,key.Item2, key.Item3.transform);
            }
            
            var keys2d = _played2dAudioSources.Keys;
            foreach (var key in keys2d)
            {
                Pause(key.Item1,key.Item2);
            }
        }

        public void ResumeAll()
        {
            var keys3d = _paused3dAudioSources.Keys;
            foreach (var key in keys3d)
            {
                Resume3D(key.Item1,key.Item2, key.Item3.transform);
            }
            
            var keys2d = _paused2dAudioSources.Keys;
            foreach (var key in keys2d)
            {
                Resume(key.Item1,key.Item2);
            }
        }
        
        private sealed class RuntimeAudioData
        {
            private AudioClip _audioClip;
            private AudioWrapperSettings _audioSettings;
            
            public RuntimeAudioData(AudioClip audioClip, AudioWrapperSettings audioSettings)
            {
                _audioClip = audioClip;
                _audioSettings = audioSettings;
            }

            public AudioClip AudioClip => _audioClip;
            public AudioWrapperSettings AudioSettings => _audioSettings;
        }
    }
}