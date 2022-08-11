using Data;
using Logic;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI
{
    public class SelectedSkillPointIndicator : MonoBehaviour
    {
        private const string IndicatorTemplate = "selected skill cost: {0}";
        [SerializeField] private TMP_Text _indicator;
        [Inject] private EventBus _eventBus;
        
        private void OnEnable()
        {
            _eventBus.SubscribeSkillSelected(OnSkillSelected);
        }

        private void OnSkillSelected(Skill skill)
        {
            _indicator.text = string.Format(IndicatorTemplate, skill.SkillPointsToLearn);   
        }
    }
}