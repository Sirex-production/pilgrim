using System;
using UnityEngine;
using Voody.UniLeo;
using Zenject;

namespace Ingame.Movement
{
    public sealed class CharacterControllerModelProvider : MonoProvider<CharacterControllerModel>
    {
        [Inject]
        private void Construct()
        {
            if (!TryGetComponent(out CharacterController characterController))
                throw new NullReferenceException($"There is no CharacterController attached");
            
            value = new CharacterControllerModel
            {
                characterController = characterController
            };
        }
    }
}