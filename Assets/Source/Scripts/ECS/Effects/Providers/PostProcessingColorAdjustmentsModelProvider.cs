using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Voody.UniLeo;
using Zenject;

namespace Ingame.Effects
{
	[RequireComponent(typeof(Volume))]
	public sealed class PostProcessingColorAdjustmentsModelProvider : MonoProvider<PostProcessingColorAdjustmentsModel>
	{
		[Inject]
		private void Construct()
		{
			if (!GetComponent<Volume>().profile.TryGet(out ColorAdjustments colorAdjustments))
				throw new NullReferenceException("ChromaticAberration effect is not present in post processing volume");
            
			value = new PostProcessingColorAdjustmentsModel
			{
				colorAdjustments = colorAdjustments,
			};
		}
	}
}