using Ingame.Input;
using Ingame.Interaction.Common;
using Ingame.Movement;
using Ingame.Player;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Dialog {
    public sealed class DialogSystem : IEcsRunSystem {
        
        private readonly EcsFilter<InteractiveTag, PerformInteractionTag,DialogComponent>.Exclude<AnswerEvent> _filterDialog;
        private readonly EcsFilter<InteractiveTag, PerformInteractionTag,DialogComponent,AnswerEvent> _filterDialogChoiceOption;
        private readonly EcsFilter<DialogCutDownDialogRequest,DialogComponent>  _cutDownDialogFilter;
        //get dialog holder
        private readonly EcsFilter<DialogSystemModel> _dialogHolderFilter;
        //get player movement
        private readonly EcsFilter<PlayerModel> _playerFilter;
      
        private readonly EcsFilter<VelocityComponent, CharacterControllerModel> _velocityFilter;
        private readonly EcsFilter<MoveInputRequest> _inputFilter;
        
        private readonly EcsFilter<AnswerEvent> _answerEventFilter;
        private readonly EcsFilter<ChangeAnswerEvent> _changeAnswerEventFilter;
 
        private float _delay = 1;
        private float _currentDelay = 0;
        
        public void Run()
        {
            //next line
            HandlePlayerDialogNextLine();
            //handle Input
            HandlePlayerInputOnAnswerPhase();
            //choose action - change action
            HandleChooseOptionEvent();
            //choose action - perform click
            HandleChooseAnswer();
        }

        private void HandlePlayerDialogNextLine()
        {
            foreach (var i in _filterDialog)
            {
          
                _velocityFilter.GetEntity(0).Get<BlockMovementRequest>();
                _playerFilter.GetEntity(0).Get<BlockRotationRequest>();
                ref var entity = ref _filterDialog.GetEntity(i);
                ref var dialog = ref _filterDialog.Get3(i);

                ref var dialogSystem = ref _dialogHolderFilter.Get1(0);
                

                //display line
 
                dialogSystem.QuestionHolder.text = dialog.DialogQuestion.Lines[dialog.LineNo];

                //trigger answer
                if (dialog.DialogQuestion.Lines.Count - 1 <= dialog.LineNo)
                {
                    if (dialog.DialogQuestion.PossibleAnswers == null || dialog.DialogQuestion.PossibleAnswers.Count<=0)
                    {
                        dialogSystem.QuestionHolder.text = "";
                        entity.Del<PerformInteractionTag>();
                        _velocityFilter.GetEntity(0).Del<BlockMovementRequest>();
                        _playerFilter.GetEntity(0).Del<BlockRotationRequest>();
                        continue;
                    }
                    for (var ansNo = 0; ansNo < dialogSystem.AnswerHolder.Count; ansNo++)
                    {
                        if (ansNo > dialog.DialogQuestion.PossibleAnswers.Count - 1)
                        {
                            dialogSystem.AnswerHolder[ansNo].text = "";
                            continue;
                        }
                        dialogSystem.AnswerHolder[ansNo].text = dialog.DialogQuestion.PossibleAnswers[ansNo].response;
  
                    }
                    dialog.LineNo = 0;
                    dialogSystem.AnswerHolder[0].color = Color.green;
                    
                    entity.Del<PerformInteractionTag>();
                    entity.Get<AnswerEvent>();
                    continue;
                }
              
                dialog.LineNo++;
                entity.Del<PerformInteractionTag>();
            }
        }

        private void HandleChooseAnswer()
        {
            //pick answer
            if (_filterDialogChoiceOption.IsEmpty())
            {
                return;
            }
            ref var entity = ref _filterDialogChoiceOption.GetEntity(0);
            ref var dialog = ref _filterDialogChoiceOption.Get3(0);

            dialog.DialogQuestion = dialog.DialogQuestion.PossibleAnswers[dialog.AnsNo].question;
            dialog.AnsNo = 0;
            dialog.LineNo = 0;
            ref var dialogSystem = ref _dialogHolderFilter.Get1(0);
            dialogSystem.QuestionHolder.text = dialog.DialogQuestion.Lines[dialog.LineNo];
            foreach (var i in dialogSystem.AnswerHolder)
            {
                i.text = "";
                i.color = Color.white;
            }

            dialog.LineNo++;
            entity.Del<AnswerEvent>();
            entity.Del<PerformInteractionTag>();
             
        }
        private void HandlePlayerInputOnAnswerPhase()
        {
            _currentDelay += Time.deltaTime;
            if (_inputFilter.IsEmpty() || _answerEventFilter.IsEmpty()) return;
            ref var input = ref _inputFilter.Get1(0);
            if (input.movementInput.x ==0)
            {
                return;
            }
            
            if (_currentDelay<_delay)
            {
                return;
            }
        
            _currentDelay = 0;
           
            ref var dialog = ref _filterDialog.Get3(0);
            dialog.AnsNo += (int)input.movementInput.x;
            dialog.AnsNo = dialog.AnsNo < 0 ? dialog.DialogQuestion.PossibleAnswers.Count - 1 :
                dialog.AnsNo > dialog.DialogQuestion.PossibleAnswers.Count - 1 ? 0 : dialog.AnsNo;
            _filterDialog.GetEntity(0).Get<ChangeAnswerEvent>();
        }
        
        private void HandleChooseOptionEvent()
        {
            if(_changeAnswerEventFilter.IsEmpty())return;
            ref var dialogSystem = ref _dialogHolderFilter.Get1(0);
            ref var dialog = ref _filterDialog.Get3(0);
            ref var entity= ref _filterDialog.GetEntity(0);
            for (var ansNo = 0; ansNo < dialogSystem.AnswerHolder.Count; ansNo++)
            {
                dialogSystem.AnswerHolder[ansNo].color = Color.white;
                if (ansNo == dialog.AnsNo)
                {
                    dialogSystem.AnswerHolder[ansNo].color = Color.green;
                }
            }
            entity.Del<ChangeAnswerEvent>();
        }
    }
}