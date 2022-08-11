using System;
using System.Collections.Generic;
using Logic;
using UnityEngine;

namespace Data
{
    public class GameDataLoader : MonoBehaviour
    {
        private const string SkillTreeStateKey = "SkillTreeStateKey";
        [SerializeField] private SkillsScheme _allSkills;
        [SerializeField] private EventHandler _eventHandler;
        private readonly List<Skill> _skills = new();
        private SkillTreeState _state;

        private void OnEnable()
        {
            LoadState();
            LoadSkills();
            _eventHandler.SaveRequestEvent += OnSaveRequestEvent;
            _eventHandler.LoadingDataFinishedEvent?.Invoke(new GameDataProvider(_skills, _state));
        }

        private void OnDisable()
        {
            _eventHandler.SaveRequestEvent -= OnSaveRequestEvent;
        }

        private void OnSaveRequestEvent(SkillTreeState skillTreeState)
        {
            _state = skillTreeState;
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