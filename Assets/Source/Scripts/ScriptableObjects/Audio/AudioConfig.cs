using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Ingame.Audio
{
    [Serializable]
    [CreateAssetMenu(menuName = "Ingame/Audio/audioContainer", fileName = "audioContainer")]
    public sealed class AudioConfig : ScriptableObject
    {
        [SerializeField] private List<AudioTypeWrapper> audios;

        public List<AudioTypeWrapper> Audios => audios;
    }
    
    [Serializable]
    public sealed class AudioTypeWrapper
    {
        [SerializeField] private string audioTypeName;
        [SerializeField] private List<AudioWrapper> audioWrappers;
        
        public List<AudioWrapper> AudioWrappers => audioWrappers;
        public string AudioTypeName => audioTypeName;
    }
    
 
    [Serializable]
    public sealed class AudioWrapper
    {
        [SerializeField] private string name;
        [SerializeField] private AudioClip audioClip;
        [SerializeField] private AudioWrapperSettings audioSettings;
        
        public AudioClip AudioClip => audioClip;
        public string Name => name;

        public AudioWrapperSettings AudioSettings => audioSettings;
        
    }
    [Serializable]
    public sealed class AudioWrapperSettings
    {
        [SerializeField]
        [Foldout("Settings")]
        [Range(0, 256)]
        private int priority;
        [SerializeField]
        [Foldout("Settings")]
        [Range(0, 1)]
        private float volume;
        [SerializeField]
        [Foldout("Settings")]
        [Range(0, 3)]
        private float pitch;
        [Foldout("Settings")]
        [Range(0, 1)]
        private float spatialBlend;
        [SerializeField]
        [Foldout("Settings")]
        private bool loop = false;

        [Space(10)]
        [SerializeField]
        [Foldout("Settings 3D")]
        [Range(0, 5)]
        private float dopplerLevel = 0;
        
        [SerializeField]
        [Foldout("Settings 3D")]
        [ShowIf("Is3DSound")]
        [Range(0, 360)]
        private float spread = 0;
        
        [SerializeField]
        [ShowIf("Is3DSound")]
        [MinValue(0)]
        [Foldout("Settings 3D")]
        private float minDistance = 0;
        
        [SerializeField]
        [ShowIf("Is3DSound")]
        [MinValue(0.01f)]
        [Foldout("Settings 3D")]
        private float maxDistance = 0.01f;
        
        public float Volume => volume;
        public int Priority => priority;
        public float SpatialBlend => spatialBlend;
        public float Pitch => pitch;
        public bool Loop => loop;
        public float DopplerLevel => dopplerLevel;
        public float Spread => spread;
        public float MinDistance => minDistance;
        public float MaxDistance => maxDistance;
        
        private bool Is3DSound => DopplerLevel > 1f;
    }
}