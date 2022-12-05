using System;
using System.Collections;
using System.Collections.Generic;
using Ingame.Data;
using NaughtyAttributes;
using NaughtyBezierCurves;
using UnityEngine;

namespace Ingame.Ladder
{
    [Serializable]
    public struct LadderModel
    {
        [Expandable] 
        public CurveMovementData CurveData;

        public BezierCurve3D Curve;
        
        [HideInInspector]
        public float Progress;

         
        public bool IsProgressSetAtTheBeginning;

    }
}