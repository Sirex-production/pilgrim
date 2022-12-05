using System;
using System.Collections;
using System.Collections.Generic;
using Ingame.Data;
using Ingame.Input;
using Ingame.Interaction.Common;
using Ingame.Movement;
using Ingame.Player;
using LeoEcsPhysics;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Ladder
{
    public sealed class LadderSystem : IEcsRunSystem
    {
        private readonly EcsFilter<LadderModel, PerformInteractionTag> _ladderFilter;
        private readonly EcsFilter<CharacterControllerModel > _playerFilter;

        private readonly EcsFilter<MoveInputRequest> _inputFilter;

        private const float SMOOTHING_THRESHOLD = 0.01f;
        public void Run()
        {
            foreach (var i in _ladderFilter)
            {
                HandlePlayerMovement(i);
            }
        }

        private void BlockPlayerMovement(ref EcsEntity player)
        {
            if (!player.Has<BlockMovementRequest>())
            {
                player.Get<BlockMovementRequest>();
            }
          //todo 
          //limit players vision
        }
        private void HandlePlayerMovement(int i)
        {
            
            if (_playerFilter.IsEmpty()) return;
            
            ref var ladderEntity = ref _ladderFilter.GetEntity(i);
            ref var ladder =  ref _ladderFilter.Get1(i);
            
            ref var character = ref _playerFilter.Get1(0);
            ref var characterEntity = ref _playerFilter.GetEntity(0);
            
            character.characterController.transform.rotation = Quaternion.Euler(0,  ladder.Curve.transform.eulerAngles.y+90 , 0);
            
            if (!characterEntity.Has<VelocityComponent>())
            {
                return;
            }
            
            //define staring position
            if (!ladder.IsProgressSetAtTheBeginning)
            {
                var firstPoint = ladder.Curve.KeyPoints[0];
                var lastPoint = ladder.Curve.KeyPoints[^1];
                var position = character.characterController.transform.position;
                
                ladder.Progress =
                    Mathf.Abs(lastPoint.transform.position.y - position.y) <
                    Mathf.Abs(firstPoint.transform.position.y - position.y)
                        ? 1f
                        : 0f;
                ladder.IsProgressSetAtTheBeginning = true;
            }
            
            BlockPlayerMovement(ref characterEntity);

            var playerPosition = character.characterController.transform.position;
            var input = _inputFilter.IsEmpty() ? 0 : _inputFilter.Get1(0).movementInput.y;

            Vector3 newPosition = Vector3.zero;
            //smooth or teleportation
            if (ladder.CurveData.SmoothMovement)
            {
                var pointToMove = ladder.Curve.GetPoint(ladder.Progress);
                
                if(ladder.CurveData.TypeOfSmoothing == TypeOfSmoothing.Lerp)
                    newPosition = Vector3.Lerp(playerPosition,pointToMove,Time.deltaTime*ladder.CurveData.SmoothingRate);
                
                if(ladder.CurveData.TypeOfSmoothing == TypeOfSmoothing.MoveToward)
                    newPosition = Vector3.MoveTowards(playerPosition,pointToMove,Time.deltaTime*ladder.CurveData.SmoothingRate);    
                
                if(ladder.CurveData.TypeOfSmoothing == TypeOfSmoothing.SmoothDump)
                {
                    var curveDataVelocity = ladder.CurveData.Velocity;
                    newPosition = Vector3.SmoothDamp(playerPosition,pointToMove,ref curveDataVelocity,Time.deltaTime*ladder.CurveData.SmoothingRate);
                }

                if (Vector3.Distance(playerPosition,pointToMove)<SMOOTHING_THRESHOLD)
                {
                    ladder.Progress += ladder.CurveData.SpeedRate*input;
                }
            }
            else
            {
                newPosition = ladder.Curve.GetPoint(ladder.Progress);
                ladder.Progress += ladder.CurveData.SpeedRate*input;
            }
            //stop climbing
            if (ladder.Progress is > 1 or < 0)
            {
                character.characterController.enabled = true;
                ladderEntity.Del<PerformInteractionTag>();
                characterEntity.Del<BlockRotationRequest>();
                characterEntity.Del<BlockMovementRequest>();
                ladder.IsProgressSetAtTheBeginning = false;
                return;
            }
            
            if (ladder.CurveData.UseCharacterController)
            {
              //todo
              //handle character controller 
            }
            else
            {
                character.characterController.enabled = false;
                character.characterController.transform.position = newPosition;
            }
        }
    }
}
