using DG.Tweening;
using UnityEngine;

namespace Ingame.Events
{
    public sealed class FleeingBirdWrapper : MonoBehaviour
    {
        private static readonly int _animatorStringFlee = Animator.StringToHash("Flee");
        
        [SerializeReference] private Animator animator;
        [SerializeReference] private Transform destination;

        private void Start()
        {
            switch (Random.Range(0, 1))
            {
                case 0:
                    animator.Play("IDLE2", 0, Random.Range(0f, 1f));
                    break;

                default:
                    animator.Play("IDLE", 0, Random.Range(0f, 1f));
                    break;
            }
        }

        public void Flee()
        {
            var position = destination.position;
            var dist = Vector3.Distance(animator.transform.position, position);
            
            float duration = Random.Range(0.9f, 1.5f) + (dist/10);
            animator.SetTrigger(_animatorStringFlee);
            
            DOTween.Sequence()
                .Append(animator.transform.DOMove(position, duration))
                .SetEase(Ease.InSine)
                .OnComplete(() =>
                {
                    animator.transform.DOMove(animator.transform.position + 10*animator.transform.forward, duration)
                        .SetEase(Ease.Linear) 
                        .OnComplete(() => animator.gameObject.SetActive(false));
                });
        }
    }
}