using NaughtyAttributes;
using UnityEngine;

namespace Ingame.Data.Player
{
    [CreateAssetMenu(menuName = "Ingame/PlayerInventoryData", fileName = "Ingame/NewPlayerInventoryData")]
    public class PlayerInventoryData : ScriptableObject
    {
        [BoxGroup("Medecine")]
        [SerializeField] [Min(0)] private int maximumNumberOfMorphine = 20;
        [BoxGroup("Medecine")]
        [SerializeField] [Min(0)] private int maximumNumberOfBandages = 20;
        [BoxGroup("Medecine")]
        [SerializeField] [Min(0)] private int maximumNumberOfInhalators = 20;
        [BoxGroup("Medecine")]
        [SerializeField] [Min(0)] private int maximumNumberOfEnergyDrinks = 20;

        public int MaximumNumberOfMorphine => maximumNumberOfMorphine;
        public int MaximumNumberOfBandages => maximumNumberOfBandages;
        public int MaximumNumberOfInhalators => maximumNumberOfInhalators;
        public int MaximumNumberOfEnergyDrinks => maximumNumberOfEnergyDrinks;
    }
}