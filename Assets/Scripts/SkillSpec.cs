using System.Collections.Generic;
using Logic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Skills/New skill specification", order = 0)]
public class SkillSpec : ScriptableObject
{
    [Range(1, 100)] [SerializeField] private int _skillPointsToLearn = 1;
    [SerializeField] private List<SkillSpec> _parentSkills = new();
    [SerializeField] private string _skillName;

    public Skill ConvertToSkillStruct()
    {
        var parents = new List<Skill>();
        foreach (var parentSkill in _parentSkills)
        {
            parents.Add(parentSkill.ConvertToSkillStruct());
        }
        return new Skill
        {
            Parents = parents,
            SkillName = _skillName,
            SkillPointsToLearn = _skillPointsToLearn
        };
    }
}