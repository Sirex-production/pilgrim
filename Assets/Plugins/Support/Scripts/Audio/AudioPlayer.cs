using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Support.Extensions;
namespace Support.AudioManagement
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioPlayer : MonoBehaviour
    {

        [SerializeField]
        private List<AudioWrapper> audios;

        private const int SLOT_MIN = 0;
        private const int REPETITION_MIN = 1;
        private AudioSource _source;
        private Dictionary<TypeOfSound, AudioWrapper> _audiosDict = new Dictionary<TypeOfSound, AudioWrapper>();
        private Dictionary<Audio, Queue<AudioSource>> _sourcesDict = new Dictionary<Audio, Queue<AudioSource>>();
        private int _slots = 0;
 
        private void Awake()
        {
            foreach (var wrap in audios)
            {
                _audiosDict.Add(wrap.Type, wrap);
                foreach (var sound in wrap.Audios)
                {
                    _sourcesDict.Add(sound, new Queue<AudioSource>());
                }
            }
        }
        private Audio GetAudio(TypeOfSound type, string name)
        {
            AudioWrapper wrap;
            if (!_audiosDict.TryGetValue(type, out wrap))
            {
                return null;
            }

            var audio = wrap.GetAudio(name);
            return audio;
        }

        public void Play(TypeOfSound type, string name)
        {
            Play(type, name, false);
        }
 
        public void Play(TypeOfSound type, string name, bool repeat)
        {
            //checks if such audio exists
            var audio = GetAudio(type, name);
            if (audio == null)
            {
                return;
            }
            //assign _source a AudioSource
            if (_source == null)
            {
                _source = gameObject.GetComponent<AudioSource>();
            }
            //checks if such audio can be repeated and if it's already being played
            var queue = _sourcesDict[audio];
            if (!repeat && queue.Count > REPETITION_MIN)
            {
                return;
            }
            //checks if main Audio is unoccupied
            //if not, creates a new one
            AudioSource src;
            if (!_source.isPlaying)
            {
                src = _source;
            }
            else
            {
                src = gameObject.AddComponent<AudioSource>();
            }
            //Audio Source config
            src.volume = audio.Volume;
            src.clip = audio.Clip;
            src.loop = audio.Loop;
            src.pitch = audio.Pitch;
            src.priority = audio.Prority;
            src.spatialBlend = audio.SpatialBlend;
            src.minDistance = audio.MinDistance;
            src.maxDistance = audio.MaxDistance;
            src.dopplerLevel = audio.DopplerLevel;
            src.spread = audio.Spread;

            //adds item to the list of occupied Audio Sources 
            //increases a int of slots that represent a current amount of occupied audio sources
            queue.Enqueue(src);
            _slots++;
            
            src.Play();
            if (!audio.Loop)
                this.WaitAndDoCoroutine(src.clip.length / 2, () => Stop(type, name));
        }



        /// <summary>
        /// If the found audio is still being played, forces it to stop and removes additional audio source
        /// </summary>
        public void Stop(TypeOfSound type, string name)
        {
            //checks if a such audio exists
            var audio = GetAudio(type, name);
            if (audio == null)
            {
                return;
            }
            var queue = _sourcesDict[audio];
            //Stops and removes the oldest audio then removes its source
            AudioSource res = queue.Dequeue();
            res.Stop();
            _slots--;

            //leaves only one audio manager and assigns it to the _source
            if (_slots == SLOT_MIN)
            {
                _source = res;
            }
            else
            {
                Destroy(res);
            }
        }

        public void StopAll(TypeOfSound type, string name)
        {
            var audio = GetAudio(type, name);
            if (audio == null)
            {
                return;
            }
            var queue = _sourcesDict[audio];
            //Stops and removes all Audio Sources except the last audio Source of the object
            foreach(var item in queue)
            {
                item.Stop();
                _slots--;
                if (_slots == SLOT_MIN)
                {
                    _source = item;
                }
                else
                {
                    Destroy(item);
                }
            }
            queue= new Queue<AudioSource>();
        }
    }
}