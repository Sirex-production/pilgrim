using NaughtyAttributes;
using UnityEngine;

namespace Ingame.Animation
{
	public sealed class Mp5AnimationCallbacks : MonoBehaviour
	{
		[Required, SerializeField] private Animator mp5Animator;

		public void SetIsInShutterDelayPosToTrue()
		{
			mp5Animator.SetBool("IsInShutterDelayPos", true);
		}
		
		public void SetIsInShutterDelayPosToFalse()
		{
			mp5Animator.SetBool("IsInShutterDelayPos", false);
		}
	}
}