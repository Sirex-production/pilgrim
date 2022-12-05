using Ingame.Utils;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace Ingame.DI.Installers
{
    public sealed class GunInstaller : MonoInstaller
    {
        [BoxGroup("Components"), Required]
        [SerializeField] private SurfaceDetector surfaceDetector;
        
        public override void InstallBindings()
        {
            Container.Bind<SurfaceDetector>()
                .FromInstance(surfaceDetector)
                .AsSingle();
        }
    }
}