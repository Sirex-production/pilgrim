using System;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
namespace Ingame.Audio
{
    [Serializable]
    public struct AudioStorageModel
    {
        [Expandable]
        public AudioContainer AudioContainer;
        public Dictionary<string, Dictionary<string, (AudioClip,AudioWrapperSettings)>> Audios;
    }
}