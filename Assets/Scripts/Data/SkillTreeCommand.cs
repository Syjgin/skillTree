namespace Data
{
    public struct SkillTreeCommand
    {
        public SkillTreeCommandType CommandType;
        public enum SkillTreeCommandType
        {
            Learn,
            Forget,
            ForgetAll,
            AddSkillPoint
        }   
    }
}