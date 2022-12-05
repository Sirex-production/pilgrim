using NaughtyAttributes;
using UnityEngine;

public class EnemyRendererManager : MonoBehaviour
{
    [SerializeField] private int pixelDetectionThreshold = 10;
    
    [Required, SerializeField] private Camera enemyCamera;

    [SerializeField] private LayerMask maskForEnvironment;
    [SerializeField] private LayerMask maskForEnvironmentWithPlayer;
    [SerializeField] private LayerMask maskForPlayer;

#if UNITY_EDITOR
    [SerializeField] private bool shouldTexBeVisible = false;
    //todo Preview
    [SerializeField] 
    [ShowIf("shouldTexBeVisible")]
    private RenderTexture renderTexture;
#endif
    
    private Texture2D _texture;
    private int _width=256, _height=256;
    
    private void Start()
    { 
        enemyCamera.backgroundColor = new Color(0,0,0,0);
       RenderView();
    }
    
    private void RenderView()
    {
        var enviro = GetRenderTexture(maskForEnvironment);
        var all = GetRenderTexture(maskForEnvironmentWithPlayer);

        var displayedPlayersPixels = 0;
        var enviroPixels = enviro.GetPixels();
        var allPixels = all.GetPixels();
        for (int i = 0; i < enviroPixels.Length ; i++)
        {
            if (enviroPixels[i] != allPixels[i])
            {
                displayedPlayersPixels++;
            }
        }

        if (displayedPlayersPixels>=pixelDetectionThreshold)
        {
            Debug.Log($"I See You... Player's pixels:{displayedPlayersPixels}, threshold to detect:{pixelDetectionThreshold}");
            return;
        }
        Debug.Log($"I Cant See You... Player's pixels:{displayedPlayersPixels}, threshold to detect:{pixelDetectionThreshold}");
        var player = GetRenderTexture(maskForPlayer);
        var playerPixels = player.GetPixels();
        var totalNumberOfPlayersPixels = 0;
        var bgc = enemyCamera.backgroundColor;
        foreach (var c in playerPixels)
        {
            if (c!=bgc)
            {
                totalNumberOfPlayersPixels ++;
            }
        }
    }

    private Texture2D GetRenderTexture( LayerMask mask)
    {
        enemyCamera.cullingMask = mask;
        enemyCamera.gameObject.SetActive(true);
        enemyCamera.Render();
        RenderTexture.active = enemyCamera.targetTexture;
        var texture = new Texture2D(_width, _height, TextureFormat.RGBA32, false);
        texture.ReadPixels(new(0,0,_width,_height),0,0);
        texture.Apply();
        //camera.cullingMask = ;
        enemyCamera.gameObject.SetActive(false);     
        return texture;
    }
}
