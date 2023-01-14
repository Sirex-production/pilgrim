using Ingame.Enemy;
using Ingame.Movement;
using Leopotam.Ecs;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Ingame.Systems
{
    public sealed class SharedCameraDetectionSystem : IEcsRunSystem
    {   
        private const float ENEMY_HEIGHT = 1.55f;
        private const int DETECTION_TEXTURE_WIDTH = 32;
        private const int DETECTION_TEXTURE_HEIGHT = 32;
        private const int DETECTION_THRESHOLD_IN_PIXELS = 4;
        
        private readonly EcsFilter<EnemyStateModel, TransformModel, EnemyUseCameraRequest> _enemyFilter;
        //MUST BE only one shared camera!!!
        private readonly EcsFilter<CameraComponent, SharedCameraModel> _cameraFilter;

        public void Run()
        {
            if (_cameraFilter.IsEmpty() || _cameraFilter.IsEmpty())
                return;

            ref var camera = ref _cameraFilter.Get1(0);
            ref var cameraModel = ref _cameraFilter.Get2(0);
            
            camera.Camera.backgroundColor = Color.black;
            
            foreach (var enemy in _enemyFilter)
            {
                ref var model = ref _enemyFilter.Get1(enemy);
                ref var transform = ref _enemyFilter.Get2(enemy);
                var cameraTransform = camera.Camera.transform;
                
                //set camera position
                cameraTransform.parent = transform.transform;
                cameraTransform.localPosition = new Vector3(0, ENEMY_HEIGHT, 0);
                camera.Camera.transform.LookAt(model.target);
           
                var environment = GetRenderTexture(camera.Camera,cameraModel.MaskForEnvironment);
                var all = GetRenderTexture(camera.Camera,cameraModel.MaskForEnvironmentWithPlayer);
                var visibilityOfPlayer = GetNumberOfPixelsOfPLayer(environment, all);
                
                model.visibleTargetPixels = visibilityOfPlayer;
                
                _enemyFilter.GetEntity(enemy).Del<EnemyUseCameraRequest>();
                
                if (visibilityOfPlayer >= DETECTION_THRESHOLD_IN_PIXELS) 
                    model.isTargetDetected = true;
            }
        }

        private int GetNumberOfPixelsOfPLayer(Texture2D t1, Texture2D t2)
        {
            var environmentPixels = new NativeArray<Color>(t1.GetPixels(), Allocator.TempJob);
            var allPixels = new NativeArray<Color>(t2.GetPixels(), Allocator.TempJob);
            var result = new NativeArray<int>(1, Allocator.TempJob);

            var jobHandle = new CountDifferentPixelsJob
            {
                pixels1 = environmentPixels,
                pixels2 = allPixels,
                result = result
            }.Schedule(environmentPixels.Length, 64);
            
            jobHandle.Complete();

            int amountOfDifferentPixels = result[0];

            environmentPixels.Dispose();
            allPixels.Dispose();
            result.Dispose();

            return amountOfDifferentPixels;
        }

        private Texture2D GetRenderTexture(Camera camera, LayerMask mask)
        {
            var texture = new Texture2D(DETECTION_TEXTURE_WIDTH, DETECTION_TEXTURE_HEIGHT, TextureFormat.RGBA32, false);
            
            camera.cullingMask = mask;
            camera.gameObject.SetActive(true);
            camera.Render();
            RenderTexture.active = camera.targetTexture;
            
            texture.ReadPixels(new(0,0,DETECTION_TEXTURE_WIDTH,DETECTION_TEXTURE_HEIGHT),0,0);
            texture.Apply();
            camera.gameObject.SetActive(false);     
            
            return texture;
        }
    }
    
    [BurstCompile]
    internal struct CountDifferentPixelsJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<Color> pixels1;
        [ReadOnly] public NativeArray<Color> pixels2;
     
        [NativeDisableParallelForRestriction] public NativeArray<int> result;
        
        [BurstCompile]
        public void Execute(int index)
        {
            if (pixels1[index] != pixels2[index])
                result[0]++;
        }
    }
}