using System.Collections.Generic;
using Logic;

namespace Tests
{
    public class TestInitialDataProvider : IInitialDataProvider
    {
        public IEnumerable<Skill> GetAllSkills()
        {
            var skillsScheme = new List<Skill>
            {
                new()
                {
                    Parents = new(),
                    SkillName = "One",
                    SkillPointsToLearn = 1
                },
                new()
                {
                    Parents = new(),
                    SkillName = "Two",
                    SkillPointsToLearn = 2
                },
                new()
                {
                    Parents = new(),
                    SkillName = "Three",
                    SkillPointsToLearn = 3
                }
            };
            var four = new Skill
            {
                Parents = new()
                {
                    skillsScheme[1],
                    skillsScheme[2]
                },
                SkillName = "Four",
                SkillPointsToLearn = 3
            };
            skillsScheme.Add(four);
            
            var five = new Skill
            {
                Parents = new(),
                SkillName = "Five",
                SkillPointsToLearn = 1
            };
            skillsScheme.Add(five);
            
            var six = new Skill
            {
                Parents = new()
                {
                    skillsScheme[4]
                },
                SkillName = "Six",
                SkillPointsToLearn = 1
            };
            skillsScheme.Add(six);
            
            var seven = new Skill
            {
                Parents = new(),
                SkillName = "Seven",
                SkillPointsToLearn = 1
            };
            skillsScheme.Add(seven);
            
            var eight = new Skill
            {
                Parents = new()
                {
                    skillsScheme[6]
                },
                SkillName = "Eight",
                SkillPointsToLearn = 2
            };
            skillsScheme.Add(eight);
            
            var nine = new Skill
            {
                Parents = new()
                {
                    skillsScheme[6]
                },
                SkillName = "Nine",
                SkillPointsToLearn = 2
            };
            skillsScheme.Add(nine);
            
            var ten = new Skill
            {
                Parents = new()
                {
                    skillsScheme[7],
                    skillsScheme[8],
                },
                SkillName = "Ten",
                SkillPointsToLearn = 2
            };
            skillsScheme.Add(ten);
            return skillsScheme;
        }

        public SkillTreeState GetState()
        {
            var state = new SkillTreeState
            {
                FreeSkillPoints = 0,
                KnownSkills = new List<string>
                {
                    "One"
                }
            };
            return state;
        }
    }
}