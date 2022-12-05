using Ingame.Enemy;
using Ingame.Movement;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Systems
{
    public sealed class SharedCameraDetectionSystem : IEcsRunSystem
    {   
        private const float ENEMY_HEIGHT = 1.55f;
        
        private readonly EcsFilter<EnemyStateModel,TransformModel,EnemyUseCameraRequest> _enemyFilter;
        //MUST BE only one shared camera!!!
        private readonly EcsFilter<CameraComponent,SharedCameraModel> _cameraFilter;
        
        private int _width=32, _height=32;
        private int _pixelDetectionThreshold = 10;
        [Range(0,1)]
        private float _percentageDetectionThreshold = 0.15f;
        public void Run()
        {
            if (_cameraFilter.IsEmpty() || _cameraFilter.IsEmpty())
            {
                return;
            }

            ref var camera = ref _cameraFilter.Get1(0);
            camera.Camera.backgroundColor = Color.black;
            
            ref var cameraModel = ref _cameraFilter.Get2(0);
            foreach (var enemy in _enemyFilter)
            {
                ref var model = ref _enemyFilter.Get1(enemy);
                ref var transform = ref _enemyFilter.Get2(enemy);
                
                //set camera position
                camera.Camera.transform.parent = transform.transform;
                camera.Camera.transform.localPosition = new Vector3(0, ENEMY_HEIGHT, 0);
                camera.Camera.transform.localRotation = Quaternion.Euler(0,0,0);
           
                var environment = GetRenderTexture(camera.Camera,cameraModel.MaskForEnvironment);
                var all = GetRenderTexture(camera.Camera,cameraModel.MaskForEnvironmentWithPlayer);
            
                //1st phase of player recognition
                var visibilityOfPlayer = GetNumberOfPixelsOfPLayer(camera.Camera, environment, all);
                model.VisibleTagretPixels = visibilityOfPlayer;
                if (visibilityOfPlayer>=_pixelDetectionThreshold)
                {
                    model.IsTargetDetected = true;
                    _enemyFilter.GetEntity(enemy).Del<EnemyUseCameraRequest>();
                    continue;
                }
                
                //2nd phase of player recognition
                var player = GetRenderTexture(camera.Camera,cameraModel.MaskForPlayer);
                var percentageVision = GetPercentageVisibilityOfPixelsOfPLayer(camera.Camera, player, visibilityOfPlayer);
                if (percentageVision >= _percentageDetectionThreshold)
                {
                    model.IsTargetDetected = true;
                }
                _enemyFilter.GetEntity(enemy).Del<EnemyUseCameraRequest>();
            }
        }

        private int GetNumberOfPixelsOfPLayer(Camera camera,Texture2D t1,Texture2D t2)
        {
            var enviroPixels = t1.GetPixels();
            var allPixels = t2.GetPixels();
            var displayedPlayersPixels = 0;
            for (int i = 0; i < enviroPixels.Length ; i++)
            {
                if (enviroPixels[i] != allPixels[i])
                {
                    displayedPlayersPixels++;
                }
            }

            return displayedPlayersPixels;
        }

        private float GetPercentageVisibilityOfPixelsOfPLayer(Camera camera, Texture2D t,int numberOfVisiblePixels)
        {    
            var totalNumberOfPlayersPixels = 0f;
            var bgc = camera.backgroundColor;
            var playerPixels = t.GetPixels();
            foreach (var c in playerPixels)
            {
                if (c!=bgc)
                {
                    totalNumberOfPlayersPixels ++;
                }
            }
            if (totalNumberOfPlayersPixels == 0)
            {
                return 0;
            }
            return numberOfVisiblePixels/totalNumberOfPlayersPixels;
        }
        private Texture2D GetRenderTexture(Camera camera ,LayerMask mask)
        {
            camera.cullingMask = mask;
            camera.gameObject.SetActive(true);
            camera.Render();
            RenderTexture.active = camera.targetTexture;
            var texture = new Texture2D(_width, _height, TextureFormat.RGBA32, false);
            texture.ReadPixels(new(0,0,_width,_height),0,0);
            texture.Apply();
            camera.gameObject.SetActive(false);     
            return texture;
        }
        
    }
}