using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;

namespace Support.AudioManagement
{
    [Serializable]
    public class Audio
    {

        [SerializeField]
        private string name;
        [SerializeField]
        private AudioClip clip;


        [SerializeField]
        [Foldout("Settings")]
        [Range(0, 256)]
        private int prority;
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
        [Range(0, 360)]
        private float spread = 0;
        [SerializeField]
        [MinValue(0)]
        [Foldout("Settings 3D")]
        private float minDistance = 0;
        [SerializeField]
        [MinValue(0.01f)]
        [Foldout("Settings 3D")]
        private float maxDistance = 0.01f;

        public string Name => name;
        public AudioClip Clip => clip;
        public float Volume => volume;
        public int Prority => prority;
        public float SpatialBlend => spatialBlend;
        public float Pitch => pitch;
        public bool Loop => loop;
        public float DopplerLevel => dopplerLevel;
        public float Spread => spread;
        public float MinDistance => minDistance;
        public float MaxDistance => maxDistance;
    }
    public enum TypeOfSound
    {
        Music,
        SoundEffect,
        Dialog
    }
    [Serializable]
    public class AudioWrapper
    {
        [SerializeField]
        private TypeOfSound type;
        [SerializeField]
        private Audio[] audios;

        public TypeOfSound Type => type;
        public Audio[] Audios => audios;

        public Audio GetAudio(string s)
        {
            Audio res = Array.Find(audios, audio => audio.Name.Equals(s));
            return res;
        }
    }
}
