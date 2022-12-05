using NaughtyAttributes;
using UnityEngine;
using Voody.UniLeo;
using Zenject;

namespace Ingame.Animation
{
    public sealed class AnimatorModelProvider : MonoProvider<AnimatorModel>
    {
        [Required, SerializeField] private Animator animator;
        
        [Inject]
        private void Construct()
        {
            value = new AnimatorModel
            {
                animator = animator
            };
        }
    }
}