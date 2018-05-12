using Sample.Definitions.Common;

namespace Sample.BP.Common
{
    public interface ISaveResult
    {
        bool Success { get; }
        RuleViolations Errors { get; }
        int Id { get; }
    }
}
