using System.Collections;
using System.Collections.Generic;
using Ingame.Behaviour;
using UnityEngine;
using Voody.UniLeo;

public class BehaviourAgentModelProvider : MonoProvider<BehaviourAgentModel>
{
    public BehaviourAgentModel GetTree()
    {
        return value;
    }
}
