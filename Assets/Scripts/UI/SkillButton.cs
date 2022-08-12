using System;
using Data;
using Logic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class SkillButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text _skillName;
        [SerializeField] private TMP_Text _number;
        [SerializeField] private GameObject _learnedIndicator;
        [SerializeField] private GameObject _selectedIndicator;
        [SerializeField] private Button _button;
        private EventBus _eventBus;
        private Skill _currentSkill;

        public void Init(Skill skill, int number, bool isLearned, EventBus eventBus)
        {
            _number.text = $"{number}";
            _skillName.text = skill.SkillName;
            _currentSkill = skill;
            _learnedIndicator.SetActive(isLearned);
            _eventBus = eventBus;
            _eventBus.SubscribeSkillSelected(OnSkillSelected);
            _eventBus.SubscribeStateChanged(OnStateChanged);
            _button.onClick.AddListener(() =>
            {
                _eventBus.SelectSkill(_currentSkill);
            });
        }
        
        private void OnStateChanged(SkillTreeRuntimeState state)
        {
            _learnedIndicator.SetActive(state.State.KnownSkills.Contains(_currentSkill.SkillName));
        }

        private void OnSkillSelected(Skill skill)
        {
            _selectedIndicator.SetActive(skill.SkillName == _currentSkill.SkillName);
        }
    }
}