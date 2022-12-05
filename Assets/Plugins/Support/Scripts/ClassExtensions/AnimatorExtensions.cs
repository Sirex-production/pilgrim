using UnityEngine;

namespace Support.Extensions
{
    public static class AnimatorExtensions
    {
        public static bool IsAnimationPlaying(this Animator animator, string animationsName)
        {
            return IsAnimationPlaying(animator) && animator.GetCurrentAnimatorStateInfo(0).IsName(animationsName);
        }

        public static bool IsAnimationPlaying(this Animator animator)
        {
            return animator.GetCurrentAnimatorStateInfo(0).length > animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }
    }
}