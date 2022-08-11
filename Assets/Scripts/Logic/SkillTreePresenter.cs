using System;
using System.Collections.Generic;

namespace Logic
{
    public class SkillTreePresenter
    {
        public int SkillPoints { get; private set; }
        public bool CanLearn { get; private set; }
        public bool CanForget { get; private set; }
        private readonly IEnumerable<Skill> _allSkills;
        private readonly List<Skill> _knownSkills = new();
        private Skill _selectedSkill;
        
        public SkillTreePresenter(IInitialDataProvider dataProvider)
        {
            _allSkills = dataProvider.GetAllSkills();
            SkillPoints = dataProvider.GetState().FreeSkillPoints;
            foreach (var skill in _allSkills)
            {
                if (dataProvider.GetState().KnownSkills.Contains(skill.SkillName))
                {
                    _knownSkills.Add(skill);
                }   
            }
        }

        public bool IsSkillKnown(string name)
        {
            foreach (var knownSkill in _knownSkills)
            {
                if (knownSkill.SkillName == name)
                    return true;
            }
            return false;
        }

        public void AddSkillPoint()
        {
            SkillPoints++;
            RecalculateAbilities();
        }

        public void SelectSkill(string name)
        {
            foreach (var skill in _allSkills)
            {
                if (skill.SkillName != name) continue;
                _selectedSkill = skill;
                RecalculateAbilities();
                return;
            }
        }

        private void RecalculateAbilities()
        {
            RecalculateLearnAbility();
            RecalculateForgetAbility();
        }

        private void RecalculateLearnAbility()
        {
            if (SkillNotSelected())
                return;
            if (_knownSkills.Contains(_selectedSkill))
            {
                CanLearn = false;
                return;
            }

            if (_selectedSkill.SkillPointsToLearn > SkillPoints)
            {
                CanLearn = false;
                return;
            }

            CanLearn = _selectedSkill.Parents.Count == 0;
            foreach (var skillParent in _selectedSkill.Parents)
            {
                if (!_knownSkills.Contains(skillParent)) continue;
                CanLearn = true;
                break;
            }
        }

        public void Learn()
        {
            if(!CanLearn)
                return;
            SkillPoints -= _selectedSkill.SkillPointsToLearn;
            _knownSkills.Add(_selectedSkill);
            RecalculateAbilities();
        }

        private void RecalculateForgetAbility()
        {
            if (SkillNotSelected())
                return;
            if (!_knownSkills.Contains(_selectedSkill))
            {
                CanForget = false;
                return;
            }

            foreach (var knownSkill in _knownSkills)
            {
                foreach (var knownSkillParent in knownSkill.Parents)
                {
                    if (knownSkillParent.SkillName == _selectedSkill.SkillName)
                    {
                        CanForget = false;
                        return;
                    }
                }
            }
            CanForget = true;
        }

        public void Forget()
        {
            if(!CanForget)
                return;
            SkillPoints += _selectedSkill.SkillPointsToLearn;
            _knownSkills.Remove(_selectedSkill);
            RecalculateAbilities();
        }

        public void ForgetAll()
        {
            foreach (var knownSkill in _knownSkills)
            {
                SkillPoints += knownSkill.SkillPointsToLearn;
            }
            _knownSkills.Clear();
        }

        public SkillTreeState GetState()
        {
            var names = new List<string>();
            foreach (var knownSkill in _knownSkills)
            {
                names.Add(knownSkill.SkillName);
            }

            return new SkillTreeState
            {
                FreeSkillPoints = SkillPoints,
                KnownSkills = names
            };
        }

        private bool SkillNotSelected()
        {
            return _selectedSkill.SkillName == null;
        }
    }
}