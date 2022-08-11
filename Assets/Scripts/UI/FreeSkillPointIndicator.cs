using Data;
using Logic;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI
{
    public class FreeSkillPointIndicator : MonoBehaviour
    {
        private const string IndicatorTemplate = "skill points: {0}";
        [SerializeField] private TMP_Text _indicator;
        [Inject] private EventBus _eventBus;
        
        private void OnEnable()
        {
            _eventBus.SubscribeInitialDataProvider(OnDataLoaded);
            _eventBus.SubscribeStateChanged(OnStateChangedEvent);
        }

        private void OnDataLoaded(IInitialDataProvider dataProvider)
        {
            if(dataProvider == null)
                return;
            UpdateIndicator(dataProvider.GetState());
        }

        private void OnStateChangedEvent(SkillTreeRuntimeState skillTreeState)
        {
            UpdateIndicator(skillTreeState.State);
        }

        private void UpdateIndicator(SkillTreeState state)
        {
            _indicator.text = string.Format(IndicatorTemplate, state.FreeSkillPoints);
        }
    }
}