using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Voody.UniLeo;
using Zenject;

namespace Ingame.Audio
{
    public sealed class AudioStorageComponentProvider : MonoProvider<AudioStorageModel>
    {
        [Inject]
        private void Construct()
        {

            Dictionary<string, Dictionary<string, (AudioClip,AudioWrapperSettings)>> dict = new ();
            foreach (var typeWrapper in value.AudioContainer.Audios)
            {
                var audioClips = typeWrapper.AudioWrappers.ToDictionary(e=> e.Name, e=>( e.AudioClip, e.AudioSettings));
                dict.Add(typeWrapper.Name,audioClips);
            }
            
            value = new AudioStorageModel
            {
                AudioContainer = value.AudioContainer,
                Audios = dict
            };
        }
    }
}