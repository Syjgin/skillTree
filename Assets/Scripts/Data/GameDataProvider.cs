using System.Collections.Generic;
using Logic;

namespace Data
{
    public class GameDataProvider : IInitialDataProvider
    {
        private readonly IEnumerable<Skill> _skills;
        private readonly SkillTreeState _skillTreeState;
        
        public GameDataProvider(IEnumerable<Skill> skills, SkillTreeState state)
        {
            _skills = skills;
            _skillTreeState = state;
        }
        public IEnumerable<Skill> GetAllSkills()
        {
            return _skills;
        }

        public SkillTreeState GetState()
        {
            return _skillTreeState;
        }
    }
}