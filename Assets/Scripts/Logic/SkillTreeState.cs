using System;
using System.Collections.Generic;

namespace Logic
{
    [Serializable]
    public struct SkillTreeState
    {
        public int FreeSkillPoints;
        public List<string> KnownSkills;
    }
}