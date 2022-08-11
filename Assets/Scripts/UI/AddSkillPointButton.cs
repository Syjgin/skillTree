using Data;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class AddSkillPointButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [Inject] private EventBus _eventBus;
        
        private void OnEnable()
        {
            _button.onClick.AddListener(() =>
            {
                _eventBus.SendCommand(new SkillTreeCommand
                {
                    CommandType = SkillTreeCommand.SkillTreeCommandType.AddSkillPoint
                });
            });
        }

        private void OnDisable()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}