using System;
using UnityEngine;

namespace Ingame.Inventory
{
    [Serializable]
    public struct BackpackModel
    {
        public Transform[] morphineInsideBackpack;
        public Transform[] adrenalineInsideBackpack;
        public Transform[] bandagesInsideBackpack;
        public Transform[] inhalatorsInsideBackpack;
        public Transform[] energyDrinksInsideBackpack;
        public Transform[] creamTubesInsideBackpack;
        
        public Transform[] magazinesTransform;

        public int MaxAmountOfMorphine => morphineInsideBackpack.Length;
        public int MaxAmountOfAdrenaline => adrenalineInsideBackpack.Length;
        public int MaxAmountOfBandages => bandagesInsideBackpack.Length;
        public int MaxAmountOfInhalators => inhalatorsInsideBackpack.Length;
        public int MaxAmountOfEnergyDrinks => energyDrinksInsideBackpack.Length;
        public int MaxAmountOfCream => creamTubesInsideBackpack.Length;
        
        public int MaxAmountOfMagazines => magazinesTransform.Length;
    }
}