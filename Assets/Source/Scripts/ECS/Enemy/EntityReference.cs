using System;
using Ingame;
using Leopotam.Ecs;
using UnityEngine;

public sealed class EntityReference : MonoBehaviour
{
    public EcsEntity Entity;

    private void Awake()
    {
        if (!TryGetComponent(out EntityReferenceRequestProvider _))
            throw new NullReferenceException($"There is no {typeof(EntityReferenceRequestProvider)} attached to {gameObject.name}");
    }
}
