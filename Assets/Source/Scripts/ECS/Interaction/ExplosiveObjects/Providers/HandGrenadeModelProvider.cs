using Voody.UniLeo;
using Zenject;

namespace Ingame.Interaction.Explosive
{
    public class HandGrenadeModelProvider : MonoProvider<HandGrenadeModel>
    {
        [Inject]
        private void Construct()
        {
            value = new HandGrenadeModel()
            {
                GrenadeData = value.GrenadeData,
                TimeLeftToExplode = value.GrenadeData.TimeToExplode
            };
        }
    }
}