using System;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Serialization;

namespace Ingame.Audio
{
    [Serializable]
    public struct AudioStorageModel
    {
        [Expandable]
        public AudioContainer audioContainer;
        public Dictionary<string, Dictionary<string, (AudioClip,AudioWrapperSettings)>> audios;
    }
}