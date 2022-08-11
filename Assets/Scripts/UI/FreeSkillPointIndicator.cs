using Logic;
using TMPro;
using UnityEngine;
using EventHandler = Data.EventHandler;

namespace UI
{
    public class FreeSkillPointIndicator : MonoBehaviour
    {
        private const string IndicatorTemplate = "skill points: {0}";
        [SerializeField] private EventHandler _eventHandler;
        [SerializeField] private TMP_Text _indicator;

        private void OnEnable()
        {
            _eventHandler.LoadingDataFinishedEvent += OnLoadingDataFinishedEvent;
        }

        private void OnLoadingDataFinishedEvent(IInitialDataProvider dataprovider)
        {
            UpdateIndicator(dataprovider.GetState());
        }

        private void UpdateIndicator(SkillTreeState state)
        {
            _indicator.text = string.Format(IndicatorTemplate, state.FreeSkillPoints);
        }
    }
}