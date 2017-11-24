using Sample.Definitions.Common;

namespace Sample.BP.Common
{
    public class CreateItemResult
    {
        public bool Success { get; set; }
        public int Id { get; set; }

        public RuleViolations Errors { get; set; } 
            = new RuleViolations();
    }

}
