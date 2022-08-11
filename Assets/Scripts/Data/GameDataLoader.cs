using System.Collections.Generic;
using Logic;
using UnityEngine;
using Zenject;

namespace Data
{
    public class GameDataLoader : MonoBehaviour
    {
        private const string SkillTreeStateKey = "SkillTreeStateKey";
        [SerializeField] private SkillsScheme _allSkills;
        [Inject] private EventBus _eventBus;
        private readonly List<Skill> _skills = new();
        private SkillTreeState _state;
        private IInitialDataProvider _gameDataProvider;

        private void OnEnable()
        {
            LoadState();
            LoadSkills();
            _gameDataProvider = new GameDataProvider(_skills, _state);
            _eventBus.SubscribeStateChanged(OnStateChangedEvent);
            _eventBus.SendInitialDataProvider(_gameDataProvider);
        }

        private void OnStateChangedEvent(SkillTreeRuntimeState skillTreeState)
        {
            _state = skillTreeState.State;
            SaveState();
        }

        private void SaveState()
        {
            var stringRepresentation = JsonUtility.ToJson(_state);
            PlayerPrefs.SetString(SkillTreeStateKey, stringRepresentation);
            PlayerPrefs.Save();
        }

        private void LoadState()
        {
            if (PlayerPrefs.HasKey(SkillTreeStateKey))
            {
                var content = PlayerPrefs.GetString(SkillTreeStateKey);
                _state = JsonUtility.FromJson<SkillTreeState>(content);
            }
            else
            {
                _state = new SkillTreeState
                {
                    FreeSkillPoints = 0,
                    KnownSkills = new List<string>()
                };
                SaveState();
            }
        }

        private void LoadSkills()
        {
            foreach (var skillSpec in _allSkills.Skills)
            {
                _skills.Add(skillSpec.ConvertToSkillStruct());
            }
        }
    }
}