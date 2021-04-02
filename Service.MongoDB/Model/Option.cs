namespace Service.MongoDB.Model
{
    public class Option
    {
        public bool CheckContractExpired { get; set; } = true;
        public bool CheckDependings { get; set; } = true;
        public bool CheckAttemps { get; set; } = true;
        public bool CheckAllowingPeriod { get; set; } = true;
    }
}