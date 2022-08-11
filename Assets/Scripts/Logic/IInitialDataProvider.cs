using System.Collections.Generic;

namespace Logic
{
    public interface IInitialDataProvider
    {
        IEnumerable<Skill> GetAllSkills();
        SkillTreeState GetState();
    }
}