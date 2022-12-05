using UnityEngine;
using Voody.UniLeo;
using Zenject;

namespace Ingame.Utils
{
    public sealed class TimerComponentProvider : MonoProvider<TimerComponent>
    {
        [SerializeField] private float initialTimePassed = 0;

        [Inject]
        private void Construct()
        {
            value = new TimerComponent
            {
                timePassed = initialTimePassed
            };
        }
    }
}