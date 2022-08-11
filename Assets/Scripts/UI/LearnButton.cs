using Data;
using Logic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class LearnButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [Inject] private EventBus _eventBus;
        
        private void OnEnable()
        {
            _eventBus.SubscribeStateChanged(OnStateChangedEvent);
            _button.interactable = false;
            _button.onClick.AddListener(() =>
            {
                _eventBus.SendCommand(new SkillTreeCommand
                {
                    CommandType = SkillTreeCommand.SkillTreeCommandType.Learn
                });
            });
        }

        private void OnStateChangedEvent(SkillTreeRuntimeState state)
        {
            _button.interactable = state.CanLearn;
        }

        private void OnDisable()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}