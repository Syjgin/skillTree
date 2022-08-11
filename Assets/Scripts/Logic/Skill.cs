using System.Collections.Generic;

namespace Logic
{
    public struct Skill
    {
        public int SkillPointsToLearn;
        public string SkillName;
        public List<Skill> Parents;

        public override bool Equals(object obj)
        {
            if (obj is not Skill skill)
                return false;
            return SkillName == skill.SkillName;
        }

        public override int GetHashCode()
        {
            return SkillName.GetHashCode();
        }
    }
}