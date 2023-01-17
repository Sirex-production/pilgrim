using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace Ingame.Audio
{
    public sealed class AudioServiceInstaller : MonoInstaller
    {
        [SerializeField] 
        [Required] 
        private AudioService audioService;
            
        public override void InstallBindings()
        {
            Container.Bind<AudioService>()
                .FromInstance(audioService)
                .AsSingle();
        }
    }
}