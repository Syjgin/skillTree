using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillsScheme", menuName = "Skills/New scheme", order = 0)]
public class SkillsScheme : ScriptableObject
{
    [SerializeField]
    private List<SkillSpec> _skills;
    public List<SkillSpec> Skills => _skills;
}