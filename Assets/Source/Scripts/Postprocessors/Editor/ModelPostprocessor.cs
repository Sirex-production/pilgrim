using UnityEditor;

namespace Ingame.Postprocessors
{
	public sealed class ModelPostprocessor : AssetPostprocessor
	{
		private void OnPreprocessModel()
		{
			var modelImporter = assetImporter as ModelImporter;
			if(modelImporter != null)
				modelImporter.generateSecondaryUV = true;
		}
	}
}